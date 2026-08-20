public interface IRepositorioInquilino
{
    Task<List<Inquilino>> ObtenerTodosAsync();  
    Task<Inquilino?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Inquilino inquilino);
    Task ActualizarAsync(Inquilino inquilino);
    Task EliminarAsync(int id);
}