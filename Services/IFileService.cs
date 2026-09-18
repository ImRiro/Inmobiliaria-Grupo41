public interface IFileService
{
    Task<string?> GuardarAvatarAsync(IFormFile? archivo, string? avatarActual);
    void EliminarAvatar(string? rutaRelativa);
}