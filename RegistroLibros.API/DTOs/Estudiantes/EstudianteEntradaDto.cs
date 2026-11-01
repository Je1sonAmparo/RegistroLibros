using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.API.DTOs.Estudiantes;

public class EstudianteEntradaDto
{
    [Required(ErrorMessage = "Debe ingresar el nombre del estudiante")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe ingresar una dirección")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe ingresar un email")]
    [EmailAddress(ErrorMessage = "Debe ingresar un formato de email válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe ingresar la fecha de nacimiento")]
    public DateTime? FechaNacimiento { get; set; }
}
