public interface IRepositorioTipoInmueble
{
    Task<List<TipoInmueble>> ObtenerTodosAsync();
    Task<TipoInmueble?> ObtenerPorIdAsync(int id);
    Task CrearAsync(TipoInmueble tipoInmueble);
    Task ActualizarAsync(TipoInmueble tipoInmueble);
    Task EliminarAsync(int id);
}
