public interface IRepositorioInmueble
{
    Task<List<Inmueble>> ObtenerTodosAsync();
    Task<Inmueble?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Inmueble inmueble);
    Task ActualizarAsync(Inmueble inmueble);
    Task EliminarAsync(int id);
}