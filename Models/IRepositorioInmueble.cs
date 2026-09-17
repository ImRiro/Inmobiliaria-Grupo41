public interface IRepositorioInmueble
{
    Task<List<Inmueble>> ObtenerTodosAsync(bool? disponible = null, int? idPropietario = null);
    Task<Inmueble?> ObtenerPorIdAsync(int id);
    Task<List<Inmueble>> ObtenerDisponiblesAsync(DateTime desde, DateTime hasta, int? idTipoInmueble = null);
    Task CrearAsync(Inmueble inmueble);
    Task ActualizarAsync(Inmueble inmueble);
    Task EliminarAsync(int id);

    
}