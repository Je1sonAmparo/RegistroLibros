using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models
{
    public class Estudiantes
    {
        [Key]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del estudiante")]
        public string? Nombres { get; set; }

        [Required(ErrorMessage = "Debe ingresar una dirección")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "Debe ingresar un formato de email válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha de nacimiento")]
        public DateTime? FechaNacimiento { get; set; }
    }
}
