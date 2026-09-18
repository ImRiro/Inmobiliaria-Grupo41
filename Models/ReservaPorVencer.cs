public class ReservaPorVencer
{
    public int IdReserva { get; set; }

    public string DireccionInmueble { get; set; } = "";

    public string NombreInquilino { get; set; } = "";

    public DateTime Fecha_Desde { get; set; }

    public DateTime Fecha_Hasta { get; set; }

    public decimal Monto_Diario { get; set; }

    public decimal Costo_Total { get; set; }
}