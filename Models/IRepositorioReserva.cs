public interface IRepositorioReserva
{
    Task<List<Reserva>> ObtenerTodosAsync();
    Task<Reserva?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Reserva reserva, int? idUsuario);
    Task ActualizarAsync(Reserva reserva);
    Task EliminarAsync(int id);
    Task<bool> ExisteSolapamientoAsync(int idInmueble, DateTime desde, DateTime hasta, int? idReservaExcluir = null);
    Task FinalizarConMultaAsync(int idReserva, DateTime fechaFinalizacion, decimal montoMulta, int? idUsuario);
    Task<List<InmuebleConReservas>> ObtenerMasReservadosAsync(int dias = 365, int top = 10);
    Task<List<Inmueble>> ObtenerSinReservasAsync(int dias = 30);
    Task<List<ReservaVigente>> ObtenerVigentesAsync();
}