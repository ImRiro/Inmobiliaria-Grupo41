using System.ComponentModel.DataAnnotations;

public class Inquilino
{
    [Key]
    public int IdInquilino { get; set; }

    [Required, StringLength(10, MinimumLength = 7), RegularExpression(@"^\d+$", ErrorMessage = "El DNI debe contener solo números"), Display(Name = "DNI")]
    public string DNI { get; set; } = "";

    [Required, StringLength(100), EmailAddress(ErrorMessage = "Email inválido"), Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Required, StringLength(50), RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre no puede contener números"), Display(Name = "Nombre")]
    public string Nombre { get; set; } = "";

    [Required, StringLength(50), RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido no puede contener números"), Display(Name = "Apellido")]
    public string Apellido { get; set; } = "";

    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
}