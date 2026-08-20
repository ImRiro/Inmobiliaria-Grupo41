using System.ComponentModel.DataAnnotations;

public class Propietario
{
    [Key]
    public int IdPropietario { get; set; }

    [Required, StringLength(10), Display(Name = "dni")]
    public string DNI { get; set; } = "";

    [Required, StringLength(50), Display(Name = "Nombre")]
    public string Nombre { get; set; } = "";

    [Required, StringLength(50), Display(Name = "Apellido")]
    public string Apellido { get; set; } = "";

    [Required, StringLength(100), Display(Name = "Email")]
    public string Email { get; set; } = "";
}