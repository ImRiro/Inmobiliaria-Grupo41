using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class InmueblesController : Controller
{
    private readonly IRepositorioInmueble repositorio;
    private readonly IRepositorioPropietario repositorioPropietario;
    private readonly IRepositorioTipoInmueble repositorioTipoInmueble;
    private readonly IConfiguration config;
    private readonly ILogger<InmueblesController> logger;

    public InmueblesController(
        IRepositorioInmueble repo,
        IRepositorioPropietario repoPropietario,
        IRepositorioTipoInmueble repoTipoInmueble,
        IConfiguration config,
        ILogger<InmueblesController> logger)
    {
        this.repositorio = repo;
        this.repositorioPropietario = repoPropietario;
        this.repositorioTipoInmueble = repoTipoInmueble;
        this.config = config;
        this.logger = logger;
    }

    private async Task CargarSelectsAsync(int? idPropietarioSeleccionado = null, int? idTipoSeleccionado = null)
    {
        var propietarios = await repositorioPropietario.ObtenerTodosAsync();
        var tipos = await repositorioTipoInmueble.ObtenerTodosAsync();

        ViewBag.Propietarios = new SelectList(propietarios, "IdPropietario", "NombreCompleto", idPropietarioSeleccionado);
        ViewBag.TiposInmueble = new SelectList(tipos, "IdTipoInmueble", "Nombre", idTipoSeleccionado);
    }

    public async Task<IActionResult> Index()
    {
        var inmuebles = await repositorio.ObtenerTodosAsync();
        return View(inmuebles);
    }

    public async Task<IActionResult> Details(int id)
    {
        var inmueble = await repositorio.ObtenerPorIdAsync(id);
        if (inmueble == null) return NotFound();
        return View(inmueble);
    }

    public async Task<IActionResult> Create()
    {
        await CargarSelectsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Inmueble inmueble)
    {
        if (!ModelState.IsValid)
        {
            await CargarSelectsAsync(inmueble.IdPropietario, inmueble.IdTipoInmueble);
            return View(inmueble);
        }
        await repositorio.CrearAsync(inmueble);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var inmueble = await repositorio.ObtenerPorIdAsync(id);
        if (inmueble == null) return NotFound();
        await CargarSelectsAsync(inmueble.IdPropietario, inmueble.IdTipoInmueble);
        return View(inmueble);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Inmueble inmueble)
    {
        if (!ModelState.IsValid)
        {
            await CargarSelectsAsync(inmueble.IdPropietario, inmueble.IdTipoInmueble);
            return View(inmueble);
        }
        await repositorio.ActualizarAsync(inmueble);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var inmueble = await repositorio.ObtenerPorIdAsync(id);
        if (inmueble == null) return NotFound();
        return View(inmueble);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
