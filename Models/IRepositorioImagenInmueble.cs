public interface IRepositorioImagenInmueble
{
    Task<List<ImagenInmueble>> ObtenerPorInmuebleAsync(int idInmueble);
    Task<ImagenInmueble?> ObtenerPorIdAsync(int id);
    Task<int> AltaAsync(ImagenInmueble imagen);
    Task<int> EliminarAsync(int id);
}