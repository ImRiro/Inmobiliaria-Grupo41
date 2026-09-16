using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required, StringLength(100), EmailAddress(ErrorMessage = "Email inválido"), Display(Name = "Email")]
    public string Email { get; set; } = "";

// No es requerida desde DataAnnotations pero desde la practica si es requerida.

    public string Clave { get; set; } = "";

// Para Formularios, No persiste luego de ser hasheada.

    [NotMapped]
    [DataType(DataType.Password), Display(Name = "Contraseña")]
    public string? ClavePlano { get; set; }

    [NotMapped]
    [DataType(DataType.Password), Display(Name = "Confirmar contraseña")]
    [Compare(nameof(ClavePlano), ErrorMessage = "Las contraseñas no coinciden")]
    public string? ConfirmarClave { get; set; }


    [Required, StringLength(20), Display(Name = "Rol")]
    public string Rol { get; set; } = "Empleado"; // "Administrador" | "Empleado"

    [Required, StringLength(50), Display(Name = "Nombre")]
    public string Nombre { get; set; } = "";

    [Required, StringLength(50), Display(Name = "Apellido")]
    public string Apellido { get; set; } = "";

    [StringLength(500), Display(Name = "Avatar")]
    public string? AvatarUrl { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

    public bool EsAdministrador => Rol == "Administrador";
}