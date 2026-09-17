using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class InformesController : Controller
{
    private readonly IRepositorioReserva repositorioReserva;

    public InformesController(IRepositorioReserva repositorioReserva)
    {
        this.repositorioReserva = repositorioReserva;
    }

    [HttpGet]
    public async Task<IActionResult> MasReservados(int dias = 365)
    {
        ViewBag.Dias = dias;
        var lista = await repositorioReserva.ObtenerMasReservadosAsync(dias);
        return View(lista);
    }
}