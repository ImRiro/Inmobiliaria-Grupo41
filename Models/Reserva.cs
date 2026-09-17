using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Reserva
{
    [Key]
    public int IdReserva {get;set;}

    [Required,ForeignKey(name: "IdInmueble")]
    public int IdInmueble { get; set; }

    [Required,ForeignKey(name: "IdInquilino")]
    public int IdInquilino { get; set; }

    [Required, Display(Name = "Fecha Desde")]
    public DateTime Fecha_Desde { get; set; }

    [Required, Display(Name = "Fecha Hasta")]
    public DateTime Fecha_Hasta { get; set; }

    [Display(Name = "Fecha Cancelacion")]
    public DateTime? Fecha_Cancelacion { get; set; }

    [Required, Display(Name = "Monto Diario")]
    public decimal Monto_Diario { get; set; }

    [Required, Display(Name = "Costo Total")]
    public decimal Costo_Total { get; set; }

    
    [Display(Name = "Inmueble")]
    public string? DireccionInmueble { get; set; }

    [Display(Name = "Inquilino")]
    public string? NombreInquilino { get; set; }

    public int? IdUsuarioCreador { get; set; }
    public int? IdUsuarioFinalizador { get; set; }
    
    [NotMapped, Display(Name = "Creado por")]
    public string? UsuarioCreadorNombre { get; set; }

    [NotMapped, Display(Name = "Anulado por")]
    public string? UsuarioFinalizadorNombre { get; set; }

    // "Si se cumplió menos de la mitad del tiempo original de alquiler, deberá pagar
    //  el 50% restante de alquiler. Caso contrario, sólo 25%."
    public ResultadoMulta CalcularMulta(DateTime fechaFinalizacion)
    {
        var diasTotales = (Fecha_Hasta.Date - Fecha_Desde.Date).Days;
        if (diasTotales < 1) diasTotales = 1;

        var diasCumplidos = (fechaFinalizacion.Date - Fecha_Desde.Date).Days;
        if (diasCumplidos < 0) diasCumplidos = 0;
        if (diasCumplidos > diasTotales) diasCumplidos = diasTotales;

        var diasRestantes = diasTotales - diasCumplidos;
        var montoRestante = diasRestantes * Monto_Diario;

        var porcentaje = diasCumplidos < diasTotales / 2.0 ? 0.50m : 0.25m;
        var monto = Math.Round(montoRestante * porcentaje, 2);

        return new ResultadoMulta
            {
                DiasTotales = diasTotales,
                DiasCumplidos = diasCumplidos,
                DiasRestantes = diasRestantes,
                Porcentaje = porcentaje,
                Monto = monto
            };
        }
    }

    public class ResultadoMulta
    {
        public int DiasTotales { get; set; }
        public int DiasCumplidos { get; set; }
        public int DiasRestantes { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Monto { get; set; }
    }
