public interface IRepositorioReserva
{
    Task<List<Reserva>> ObtenerTodosAsync();
    Task<Reserva?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Reserva reserva);
    Task ActualizarAsync(Reserva reserva);
    Task EliminarAsync(int id);
}