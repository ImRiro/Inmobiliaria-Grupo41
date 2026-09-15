using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inmueble
{
    [Key]
    public int IdInmueble {get;set;}

    [Required,ForeignKey(name: "IdPropietario")]
    public int IdPropietario { get; set; }

    [Required,ForeignKey(name: "IdTipoInmueble")]
    public int IdTipoInmueble { get; set; }

    [Required, StringLength(50), Display(Name = "Direccion")]
    public string Direccion { get; set; } = "";

    [Required, Display(Name = "Latitud")]
    public decimal Latitud { get; set; } = 0;

    [Required, Display(Name = "Longitud")]
    public decimal Longitud { get; set; } = 0;

    [Required, Display(Name = "Activo")]
    public bool Activo { get; set; } = false;

    [Required, Range(0, int.MaxValue, ErrorMessage = "Las habitaciones no pueden ser negativas"), Display(Name = "Habitaciones")]
    public int Habitaciones { get; set; } = 0;

    [Required, Range(0, double.MaxValue, ErrorMessage = "Los metros cuadrados no pueden ser negativos"), Display(Name = "Metros Cuadrados")]
    public decimal Metros_Cuadrados { get; set; } = 0;


    [Display(Name = "Propietario")]
    public string? NombrePropietario { get; set; }

    [Display(Name = "Tipo")]
    public string? NombreTipoInmueble { get; set; }
}