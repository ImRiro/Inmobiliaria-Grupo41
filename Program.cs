using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositorioInquilino, RepositorioInquilino>();   
builder.Services.AddScoped<IRepositorioPropietario, RepositorioPropietario>();
builder.Services.AddScoped<IRepositorioReserva, RepositorioReserva>();
builder.Services.AddScoped<IRepositorioInmueble, RepositorioInmueble>();
builder.Services.AddScoped<IRepositorioTipoInmueble, RepositorioTipoInmueble>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioPago, RepositorioPago>();
builder.Services.AddScoped<IRepositorioImagenInmueble, RepositorioImagenInmueble>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
        options.AccessDeniedPath = "/Usuarios/Restringido";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Administrador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// De no existir ningun usuario crea un admin por defecto, no se me ocurrio a mi pero me gusto la idea asi que la puse

using (var scope = app.Services.CreateScope())
{
    var repositorioUsuario = scope.ServiceProvider.GetRequiredService<IRepositorioUsuario>();
    if (await repositorioUsuario.ContarAsync() == 0)
    {
        var admin = new Usuario
        {
            Email = "admin@inmobiliaria.com",
            Rol = "Administrador",
            Nombre = "Admin",
            Apellido = "Sistema",
            Activo = true
        };
        var hasher = new PasswordHasher<Usuario>();
        admin.Clave = hasher.HashPassword(admin, "Admin123!");
        await repositorioUsuario.CrearAsync(admin);
        Console.WriteLine("Usuario admin creado -> admin@inmobiliaria.com / Admin123!  (cambiá esta contraseña)");
    }
}

app.Run();
