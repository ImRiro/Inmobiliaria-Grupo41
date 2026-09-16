using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class UsuariosController : Controller
{
    private readonly IRepositorioUsuario repositorio;
    private readonly ILogger<UsuariosController> logger;
    private static readonly PasswordHasher<Usuario> hasher = new();

    public UsuariosController(IRepositorioUsuario repo, ILogger<UsuariosController> logger)
    {
        this.repositorio = repo;
        this.logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string clave, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(clave))
        {
            ModelState.AddModelError(string.Empty, "Ingresá email y contraseña.");
            return View();
        }

        var usuario = await repositorio.ObtenerPorEmailAsync(email);
        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            return View();
        }

        var resultado = hasher.VerifyHashedPassword(usuario, usuario.Clave, clave);
        if (resultado == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            return View();
        }

        if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
        {
            usuario.Clave = hasher.HashPassword(usuario, clave);
            await repositorio.ActualizarClaveAsync(usuario.IdUsuario, usuario.Clave);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Name, usuario.Email),
            new("FullName", usuario.NombreCompleto),
            new(ClaimTypes.Role, usuario.Rol),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Restringido()
    {
        return View();
    }

    public async Task<IActionResult> Perfil()
    {
        var usuario = await ObtenerUsuarioActualAsync();
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Perfil(Usuario form)
    {
        var usuario = await ObtenerUsuarioActualAsync();
        if (usuario == null) return NotFound();

        ModelState.Remove(nameof(Usuario.Rol));
        ModelState.Remove(nameof(Usuario.Activo));
        ModelState.Remove(nameof(Usuario.Clave));
        ModelState.Remove(nameof(Usuario.Email));

        if (string.IsNullOrWhiteSpace(form.Nombre) || string.IsNullOrWhiteSpace(form.Apellido))
        {
            ModelState.AddModelError(string.Empty, "Nombre y apellido son obligatorios.");
        }

        if (!ModelState.IsValid)
        {
            form.IdUsuario = usuario.IdUsuario;
            form.Email = usuario.Email;
            form.Rol = usuario.Rol;
            return View(form);
        }

        usuario.Nombre = form.Nombre;
        usuario.Apellido = form.Apellido;
        usuario.AvatarUrl = form.AvatarUrl;
        await repositorio.ActualizarPerfilAsync(usuario);

        if (!string.IsNullOrWhiteSpace(form.ClavePlano))
        {
            var nuevoHash = hasher.HashPassword(usuario, form.ClavePlano);
            await repositorio.ActualizarClaveAsync(usuario.IdUsuario, nuevoHash);
        }

        TempData["Mensaje"] = "Perfil actualizado correctamente.";
        return RedirectToAction(nameof(Perfil));
    }

    private async Task<Usuario?> ObtenerUsuarioActualAsync()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idClaim == null || !int.TryParse(idClaim, out var id)) return null;
        return await repositorio.ObtenerPorIdAsync(id);
    }

    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Index()
    {
        var usuarios = await repositorio.ObtenerTodosAsync();
        return View(usuarios);
    }

    [Authorize(Policy = "Administrador")]
    public IActionResult Create()
    {
        return View(new Usuario { Rol = "Empleado" });
    }

    [HttpPost, Authorize(Policy = "Administrador"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        ModelState.Remove(nameof(Usuario.Clave));

        if (string.IsNullOrWhiteSpace(usuario.ClavePlano) || usuario.ClavePlano.Length < 6)
        {
            ModelState.AddModelError(nameof(Usuario.ClavePlano), "La contraseña debe tener al menos 6 caracteres.");
        }

        var existente = await repositorio.ObtenerPorEmailAsync(usuario.Email);
        if (existente != null)
        {
            ModelState.AddModelError(nameof(Usuario.Email), "Ya existe un usuario con ese email.");
        }

        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        usuario.Clave = hasher.HashPassword(usuario, usuario.ClavePlano!);
        usuario.Activo = true;

        await repositorio.CrearAsync(usuario);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await repositorio.ObtenerPorIdAsync(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, Authorize(Policy = "Administrador"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Usuario usuario)
    {
        ModelState.Remove(nameof(Usuario.Clave));
        ModelState.Remove(nameof(Usuario.ClavePlano));
        ModelState.Remove(nameof(Usuario.ConfirmarClave));

        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        var idActual = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idActual == usuario.IdUsuario.ToString() && usuario.Rol != "Administrador")
        {
            ModelState.AddModelError(nameof(Usuario.Rol), "No podés quitarte tu propio rol de Administrador.");
            return View(usuario);
        }

        await repositorio.ActualizarAsync(usuario);

        if (!string.IsNullOrWhiteSpace(usuario.ClavePlano))
        {
            var nuevoHash = hasher.HashPassword(usuario, usuario.ClavePlano);
            await repositorio.ActualizarClaveAsync(usuario.IdUsuario, nuevoHash);
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await repositorio.ObtenerPorIdAsync(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, ActionName("Delete"), Authorize(Policy = "Administrador"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var idActual = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idActual == id.ToString())
        {
            TempData["Error"] = "No podés desactivar tu propio usuario.";
            return RedirectToAction(nameof(Index));
        }

        await repositorio.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
