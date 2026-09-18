using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_.Net_Core.Models;

[Authorize]
public class PropietariosController : Controller
{

    private readonly IRepositorioPropietario repositorio;
    private readonly IConfiguration config;
    private readonly ILogger<PropietariosController> logger;

    public PropietariosController(IRepositorioPropietario repo, IConfiguration config, ILogger<PropietariosController> logger)
		{
			this.repositorio = repo;
			this.config = config;
			this.logger = logger;
		}

    public async Task<IActionResult> Index(int pagina = 1, int tamanoPagina = 10)
    {
        if (pagina < 1) pagina = 1;
        if (tamanoPagina < 1 || tamanoPagina > 100) tamanoPagina = 10;

        var total = await repositorio.ContarAsync();
        var totalPaginas = (int)Math.Ceiling((double)total / tamanoPagina);

        if (pagina > totalPaginas && totalPaginas > 0)
            pagina = totalPaginas;

        var propietarios = await repositorio.ObtenerPaginadoAsync(pagina, tamanoPagina);

        var vm = new PaginadoViewModel<Propietario>
        {
            Elementos = propietarios,
            PaginaActual = pagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = total
        };

        return View(vm);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Propietario propietario)
    {
        if (!ModelState.IsValid) return View(propietario);
        await repositorio.CrearAsync(propietario);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var propietario = await repositorio.ObtenerPorIdAsync(id);
        if (propietario == null) return NotFound();
        return View(propietario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Propietario propietario)
    {
        if (!ModelState.IsValid) return View(propietario);
        await repositorio.ActualizarAsync(propietario);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var propietario = await repositorio.ObtenerPorIdAsync(id);
        if (propietario == null) return NotFound();
        return View(propietario);
    }

    [HttpPost, ActionName("Delete"), Authorize(Policy = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}