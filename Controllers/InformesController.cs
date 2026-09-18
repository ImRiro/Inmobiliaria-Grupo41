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

    [HttpGet]
    public async Task<IActionResult> SinReservas(int dias = 30)
    {
        ViewBag.Dias = dias;

        var lista = await repositorioReserva.ObtenerSinReservasAsync(dias);

        return View(lista);
    }

    [HttpGet]
    public async Task<IActionResult> Vigentes()
    {
        var lista = await repositorioReserva.ObtenerVigentesAsync();

        return View(lista);
    }
}