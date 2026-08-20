using Microsoft.AspNetCore.Mvc;

public class InquilinosController : Controller
{

    private readonly IRepositorioInquilino repositorio;
    private readonly IConfiguration config;
    private readonly ILogger<InquilinosController> logger;

    public InquilinosController(IRepositorioInquilino repo, IConfiguration config, ILogger<InquilinosController> logger)
		{
			this.repositorio = repo;
			this.config = config;
			this.logger = logger;
		}

        public async Task<IActionResult> Index()
    {
        var inquilinos = await repositorio.ObtenerTodosAsync();
        return View(inquilinos);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Inquilino inquilino)
    {
        if (!ModelState.IsValid) return View(inquilino);
        await repositorio.CrearAsync(inquilino);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var inquilino = await repositorio.ObtenerPorIdAsync(id);
        if (inquilino == null) return NotFound();
        return View(inquilino);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Inquilino inquilino)
    {
        if (!ModelState.IsValid) return View(inquilino);
        await repositorio.ActualizarAsync(inquilino);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var inquilino = await repositorio.ObtenerPorIdAsync(id);
        if (inquilino == null) return NotFound();
        return View(inquilino);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}