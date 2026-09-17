using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Pago
{
    [Key]
    public int IdPago { get; set; }

    [Required, ForeignKey(name: "IdReserva")]
    public int IdReserva { get; set; }

    [Required, StringLength(200), Display(Name = "Concepto")]
    public string Concepto { get; set; } = "";

    [Required, Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0"), Display(Name = "Monto")]
    public decimal Monto { get; set; }

    [Required, Display(Name = "Fecha de pago")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Display(Name = "Anulado")]
    public bool Anulado { get; set; } = false;

    [Display(Name = "Fecha de anulación")]
    public DateTime? FechaAnulacion { get; set; }

    public int? IdUsuarioCreador { get; set; }
    public int? IdUsuarioAnulador { get; set; }

    [NotMapped, Display(Name = "Inquilino")]
    public string? NombreInquilino { get; set; }

    [NotMapped, Display(Name = "Inmueble")]
    public string? DireccionInmueble { get; set; }

    [NotMapped, Display(Name = "Creado por")]
    public string? UsuarioCreadorNombre { get; set; }

    [NotMapped, Display(Name = "Anulado por")]
    public string? UsuarioAnuladorNombre { get; set; }
}
