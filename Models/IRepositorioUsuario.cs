public interface IRepositorioUsuario
{
    Task<List<Usuario>> ObtenerTodosAsync();
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task<int> CrearAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
    Task ActualizarClaveAsync(int id, string claveHasheada);
    Task ActualizarPerfilAsync(Usuario usuario);
    Task EliminarAsync(int id);
    Task<int> ContarAsync();
}
