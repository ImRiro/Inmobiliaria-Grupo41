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

    [Required]
    [StringLength(255, ErrorMessage = "La dirección no puede superar los 255 caracteres")]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; } = "";

    [Required]
    [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90")]
    [Display(Name = "Latitud")]
    public decimal Latitud { get; set; } = 0;

    [Required]
    [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180")]
    [Display(Name = "Longitud")]
    public decimal Longitud { get; set; } = 0;

    [Required]
    [Display(Name = "Activo")]
    public bool Activo { get; set; } = false;

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Las habitaciones no pueden ser negativas")]
    [Display(Name = "Habitaciones")]
    public int Habitaciones { get; set; } = 0;

    [Required]
    [Range(0, 99999999.99, ErrorMessage = "Los metros cuadrados deben estar entre 0 y 99.999.999,99")]
    [Display(Name = "Metros Cuadrados")]
    public decimal Metros_Cuadrados { get; set; } = 0;


    [Display(Name = "Propietario")]
    public string? NombrePropietario { get; set; }

    [Display(Name = "Tipo")]
    public string? NombreTipoInmueble { get; set; }
}