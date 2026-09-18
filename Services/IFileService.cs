public interface IFileService
{
    Task<string?> GuardarAvatarAsync(IFormFile? archivo, string? avatarActual);
    void EliminarAvatar(string? rutaRelativa);

    Task<string?> GuardarPortadaInmuebleAsync(IFormFile? archivo, string? portadaActual);
    void EliminarPortadaInmueble(string? rutaRelativa);
    
    Task<string?> GuardarImagenGaleriaAsync(IFormFile archivo);
    void EliminarImagenGaleria(string? rutaRelativa);
}