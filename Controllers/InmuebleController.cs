using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class InmueblesController : Controller
{
    private readonly IRepositorioInmueble repositorio;
    private readonly IRepositorioPropietario repositorioPropietario;
    private readonly IRepositorioTipoInmueble repositorioTipoInmueble;
    private readonly IRepositorioImagenInmueble repositorioImagen;
    private readonly IConfiguration config;
    private readonly ILogger<InmueblesController> logger;
    private readonly IFileService fileService;

    public InmueblesController(
        IRepositorioInmueble repo,
        IRepositorioPropietario repoPropietario,
        IRepositorioTipoInmueble repoTipoInmueble,
        IRepositorioImagenInmueble repoImagenInmueble,
        IConfiguration config,
        ILogger<InmueblesController> logger,
        IFileService fileService)
    {
        this.repositorio = repo;
        this.repositorioPropietario = repoPropietario;
        this.repositorioTipoInmueble = repoTipoInmueble;
        this.repositorioImagen = repoImagenInmueble;
        this.config = config;
        this.logger = logger;
        this.fileService = fileService;
    }

    private async Task CargarSelectsAsync(int? idPropietarioSeleccionado = null, int? idTipoSeleccionado = null)
    {
        var propietarios = await repositorioPropietario.ObtenerTodosAsync();
        var tipos = await repositorioTipoInmueble.ObtenerTodosAsync();

        ViewBag.Propietarios = new SelectList(propietarios, "IdPropietario", "NombreCompleto", idPropietarioSeleccionado);
        ViewBag.TiposInmueble = new SelectList(tipos, "IdTipoInmueble", "Nombre", idTipoSeleccionado);
    }

    public async Task<IActionResult> Index(bool? disponible, int? idPropietario)
    {
        var inmuebles = await repositorio.ObtenerTodosAsync(disponible, idPropietario);
        ViewBag.Disponible = disponible;
        ViewBag.IdPropietario = idPropietario;

        if (idPropietario.HasValue)
        {
            var propietario = await repositorioPropietario.ObtenerPorIdAsync(idPropietario.Value);
            ViewBag.NombrePropietario = propietario?.NombreCompleto;
        }

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
    public async Task<IActionResult> Edit(Inmueble inmueble, IFormFile? archivoPortada)
    {
        if (!ModelState.IsValid)
        {
            await CargarSelectsAsync(inmueble.IdPropietario, inmueble.IdTipoInmueble);
            return View(inmueble);
        }

        var actual = await repositorio.ObtenerPorIdAsync(inmueble.IdInmueble);

        if (archivoPortada != null)
        {
            try
            {
                inmueble.RutaPortada = await fileService.GuardarPortadaInmuebleAsync(archivoPortada, actual?.RutaPortada);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarSelectsAsync(inmueble.IdPropietario, inmueble.IdTipoInmueble);
                inmueble.RutaPortada = actual?.RutaPortada;
                return View(inmueble);
            }
        }
        else
        {
            inmueble.RutaPortada = actual?.RutaPortada;
        }

        await repositorio.ActualizarAsync(inmueble);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var inmueble = await repositorio.ObtenerPorIdAsync(id);
        if (inmueble == null) return NotFound();
        return View(inmueble);
    }

    [HttpPost, ActionName("Delete"), Authorize(Policy = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Imagenes(int id)
    {
        var inmueble = await repositorio.ObtenerPorIdAsync(id);
        if (inmueble == null) return NotFound();

        ViewBag.Inmueble = inmueble;
        var imagenes = await repositorioImagen.ObtenerPorInmuebleAsync(id);
        return View(imagenes);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarImagen(int idInmueble, IFormFile archivo)
    {
        var inmueble = await repositorio.ObtenerPorIdAsync(idInmueble);
        if (inmueble == null) return NotFound();

        try
        {
            var ruta = await fileService.GuardarImagenGaleriaAsync(archivo);
            if (ruta != null)
            {
                await repositorioImagen.AltaAsync(new ImagenInmueble { IdInmueble = idInmueble, Url = ruta });
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorImagen"] = ex.Message;
        }

        return RedirectToAction(nameof(Imagenes), new { id = idInmueble });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarImagen(int id, int idInmueble)
    {
        var imagen = await repositorioImagen.ObtenerPorIdAsync(id);
        if (imagen != null && imagen.IdInmueble == idInmueble)
        {
            fileService.EliminarImagenGaleria(imagen.Url);
            await repositorioImagen.EliminarAsync(id);
        }

        return RedirectToAction(nameof(Imagenes), new { id = idInmueble });
    }
}
