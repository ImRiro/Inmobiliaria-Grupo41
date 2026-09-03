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
    public DateTime Fecha_Cancelacion { get; set; }

    [Required, Display(Name = "Monto Diario")]
    public decimal Monto_Diario { get; set; }

    [Required, Display(Name = "Costo Total")]
    public decimal Costo_Total { get; set; }
}