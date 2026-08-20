public interface IRepositorioPropietario
{
    Task<List<Propietario>> ObtenerTodosAsync();
    Task<Propietario?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Propietario propietario);
    Task ActualizarAsync(Propietario propietario);
    Task EliminarAsync(int id);
}