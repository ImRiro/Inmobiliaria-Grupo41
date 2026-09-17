using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class PagosController : Controller
{
    private readonly IRepositorioPago repositorioPago;
    private readonly IRepositorioReserva repositorioReserva;

    public PagosController(IRepositorioPago repositorioPago, IRepositorioReserva repositorioReserva)
    {
        this.repositorioPago = repositorioPago;
        this.repositorioReserva = repositorioReserva;
    }

    public async Task<IActionResult> Index(int idReserva)
    {
        var reserva = await repositorioReserva.ObtenerPorIdAsync(idReserva);
        if (reserva == null) return NotFound();

        ViewBag.Reserva = reserva;
        ViewBag.TotalPagado = await repositorioPago.ObtenerTotalPagadoAsync(idReserva);
        ViewBag.EsAdministrador = User.IsInRole("Administrador");

        var pagos = await repositorioPago.ObtenerPorReservaAsync(idReserva);
        return View(pagos);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pago pago)
    {
        var reserva = await repositorioReserva.ObtenerPorIdAsync(pago.IdReserva);
        if (reserva == null) return NotFound();

        ModelState.Remove(nameof(Pago.Anulado));
        ModelState.Remove(nameof(Pago.FechaAnulacion));

        if (pago.Fecha > DateTime.Today)
        {
            ModelState.AddModelError(nameof(Pago.Fecha), "La fecha de pago no puede ser futura.");
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "No se pudo cargar el pago: revisá los datos.";
            return RedirectToAction(nameof(Index), new { idReserva = pago.IdReserva });
        }

        pago.IdUsuarioCreador = ObtenerIdUsuarioActual();
        await repositorioPago.CrearAsync(pago);

        return RedirectToAction(nameof(Index), new { idReserva = pago.IdReserva });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var pago = await repositorioPago.ObtenerPorIdAsync(id);
        if (pago == null) return NotFound();
        if (pago.Anulado)
        {
            TempData["Error"] = "No se puede editar un pago anulado.";
            return RedirectToAction(nameof(Index), new { idReserva = pago.IdReserva });
        }
        return View(pago);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string concepto, int idReserva)
    {
        if (string.IsNullOrWhiteSpace(concepto))
        {
            TempData["Error"] = "El concepto no puede estar vacío.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        await repositorioPago.ActualizarConceptoAsync(id, concepto);
        return RedirectToAction(nameof(Index), new { idReserva });
    }

    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var pago = await repositorioPago.ObtenerPorIdAsync(id);
        if (pago == null) return NotFound();
        return View(pago);
    }

    [HttpPost, ActionName("Delete"), Authorize(Policy = "Administrador"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, int idReserva)
    {
        await repositorioPago.AnularAsync(id, ObtenerIdUsuarioActual());
        return RedirectToAction(nameof(Index), new { idReserva });
    }

    private int? ObtenerIdUsuarioActual()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
