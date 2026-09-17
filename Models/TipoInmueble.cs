using System.ComponentModel.DataAnnotations;

public class TipoInmueble
{
    [Key]
    public int IdTipoInmueble { get; set; }

    [Required, StringLength(50), Display(Name = "Nombre")]
    public string Nombre { get; set; } = "";

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;
}
