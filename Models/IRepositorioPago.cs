public interface IRepositorioPago
{
    // Todos los pagos (incluye los anulados porque la narrativa lo pide)
    Task<List<Pago>> ObtenerPorReservaAsync(int idReserva);

    Task<Pago?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(Pago pago);

    Task ActualizarConceptoAsync(int id, string concepto);

    Task AnularAsync(int id, int? idUsuarioAnulador);

    Task<decimal> ObtenerTotalPagadoAsync(int idReserva);
}
