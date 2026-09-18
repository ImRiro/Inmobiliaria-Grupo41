using SixLabors.ImageSharp;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<FileService> _logger;

    private static readonly string[] ExtensionesPermitidas =
        { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    private const long TamanoMaximo = 2 * 1024 * 1024;
    private const int DimensionMaxima = 2000;
    public FileService(IWebHostEnvironment env, ILogger<FileService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string?> GuardarAvatarAsync(IFormFile? archivo, string? avatarActual)
    {
        if (archivo == null || archivo.Length == 0)
            return avatarActual;

        var ext = Path.GetExtension(archivo.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(ext))
            throw new InvalidOperationException("Formato no permitido (jpg, jpeg, png, gif, webp).");

        if (archivo.Length > TamanoMaximo)
            throw new InvalidOperationException("La imagen no puede superar los 2 MB.");

        try
        {
            using var streamValidacion = archivo.OpenReadStream();
            using var image = await Image.LoadAsync(streamValidacion);

            if (image.Width > DimensionMaxima || image.Height > DimensionMaxima)
                throw new InvalidOperationException(
                    $"La imagen no puede superar {DimensionMaxima}x{DimensionMaxima} px.");
        }
        catch (UnknownImageFormatException)
        {
            throw new InvalidOperationException("El archivo no es una imagen válida.");
        }
        catch (InvalidImageContentException)
        {
            throw new InvalidOperationException("El archivo no es una imagen válida.");
        }

        var carpeta = Path.Combine(_env.WebRootPath, "avatars");
        Directory.CreateDirectory(carpeta);

        var nombreArchivo = $"{Guid.NewGuid():N}{ext}";
        var rutaFisica = Path.Combine(carpeta, nombreArchivo);

        using (var stream = new FileStream(rutaFisica, FileMode.CreateNew))
        {
            await archivo.CopyToAsync(stream);
        }

        EliminarAvatar(avatarActual);

        return $"/avatars/{nombreArchivo}";
    }

    public void EliminarAvatar(string? rutaRelativa)
    {
        if (string.IsNullOrWhiteSpace(rutaRelativa)) return;
        if (!rutaRelativa.StartsWith("/avatars/")) return;

        try
        {
            var nombre = Path.GetFileName(rutaRelativa);
            if (string.IsNullOrEmpty(nombre)) return;

            var carpetaAvatars = Path.GetFullPath(Path.Combine(_env.WebRootPath, "avatars"));
            var rutaFisica = Path.GetFullPath(Path.Combine(carpetaAvatars, nombre));

            if (!rutaFisica.StartsWith(carpetaAvatars + Path.DirectorySeparatorChar))
                return;

            if (File.Exists(rutaFisica))
                File.Delete(rutaFisica);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo eliminar el avatar {Ruta}", rutaRelativa);
        }
    }
}