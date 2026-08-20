using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index()
    {
        var propietarios = await repositorio.ObtenerTodosAsync();
        return View(propietarios);
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

    public async Task<IActionResult> Delete(int id)
    {
        var propietario = await repositorio.ObtenerPorIdAsync(id);
        if (propietario == null) return NotFound();
        return View(propietario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}