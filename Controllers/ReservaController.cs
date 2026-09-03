using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ReservasController : Controller
{
    private readonly IRepositorioReserva repositorio;
    private readonly IRepositorioInmueble repositorioInmueble;
    private readonly IRepositorioInquilino repositorioInquilino;
    private readonly IConfiguration config;
    private readonly ILogger<ReservasController> logger;

    public ReservasController(
        IRepositorioReserva repo,
        IRepositorioInmueble repoInmueble,
        IRepositorioInquilino repoInquilino,
        IConfiguration config,
        ILogger<ReservasController> logger)
    {
        this.repositorio = repo;
        this.repositorioInmueble = repoInmueble;
        this.repositorioInquilino = repoInquilino;
        this.config = config;
        this.logger = logger;
    }

    private async Task CargarSelectAsync(int? idInmuebleSeleccionado = null, int? idInquilinoSeleccionado = null)
    {
        var inmuebles = await repositorioInmueble.ObtenerTodosAsync();
        var inquilinos = await repositorioInquilino.ObtenerTodosAsync();

        ViewBag.Inmuebles = new SelectList(inmuebles, "IdInmueble", "Direccion", idInmuebleSeleccionado);
        ViewBag.Inquilinos = new SelectList(inquilinos, "IdInquilino", "NombreCompleto", idInquilinoSeleccionado);
    }

    private static void CalcularCostoTotal(Reserva reserva)
    {
        var dias = (reserva.Fecha_Hasta.Date - reserva.Fecha_Desde.Date).Days;
        if (dias < 1) dias = 1;
        reserva.Costo_Total = dias * reserva.Monto_Diario;
    }

    public async Task<IActionResult> Index()
    {
        var reservas = await repositorio.ObtenerTodosAsync();
        return View(reservas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var reserva = await repositorio.ObtenerPorIdAsync(id);
        if (reserva == null) return NotFound();
        return View(reserva);
    }

    public async Task<IActionResult> Create()
    {
        await CargarSelectAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Reserva reserva)
    {
        if (reserva.Fecha_Hasta <= reserva.Fecha_Desde)
        {
            ModelState.AddModelError(nameof(reserva.Fecha_Hasta), "La fecha hasta debe ser posterior a la fecha desde");
        }

        if (!ModelState.IsValid)
        {
            await CargarSelectAsync(reserva.IdInmueble, reserva.IdInquilino);
            return View(reserva);
        }

        CalcularCostoTotal(reserva);
        await repositorio.CrearAsync(reserva);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var reserva = await repositorio.ObtenerPorIdAsync(id);
        if (reserva == null) return NotFound();
        await CargarSelectAsync(reserva.IdInmueble, reserva.IdInquilino);
        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Reserva reserva)
    {
        if (reserva.Fecha_Hasta <= reserva.Fecha_Desde)
        {
            ModelState.AddModelError(nameof(reserva.Fecha_Hasta), "La fecha hasta debe ser posterior a la fecha desde");
        }

        if (!ModelState.IsValid)
        {
            await CargarSelectAsync(reserva.IdInmueble, reserva.IdInquilino);
            return View(reserva);
        }

        CalcularCostoTotal(reserva);
        await repositorio.ActualizarAsync(reserva);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var reserva = await repositorio.ObtenerPorIdAsync(id);
        if (reserva == null) return NotFound();
        return View(reserva);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
