using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Administrador")]
public class TiposInmuebleController : Controller
{
    private readonly IRepositorioTipoInmueble repositorio;
    private readonly IConfiguration config;
    private readonly ILogger<TiposInmuebleController> logger;

    public TiposInmuebleController(IRepositorioTipoInmueble repo, IConfiguration config, ILogger<TiposInmuebleController> logger)
    {
        this.repositorio = repo;
        this.config = config;
        this.logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var tipos = await repositorio.ObtenerTodosAsync();
        return View(tipos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var tipo = await repositorio.ObtenerPorIdAsync(id);
        if (tipo == null) return NotFound();
        return View(tipo);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TipoInmueble tipoInmueble)
    {
        if (!ModelState.IsValid) return View(tipoInmueble);
        await repositorio.CrearAsync(tipoInmueble);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var tipo = await repositorio.ObtenerPorIdAsync(id);
        if (tipo == null) return NotFound();
        return View(tipo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TipoInmueble tipoInmueble)
    {
        if (!ModelState.IsValid) return View(tipoInmueble);
        await repositorio.ActualizarAsync(tipoInmueble);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var tipo = await repositorio.ObtenerPorIdAsync(id);
        if (tipo == null) return NotFound();
        return View(tipo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
