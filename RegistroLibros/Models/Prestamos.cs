using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models
{
    public class Prestamos
    {
        [Key]
        public int PrestamoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante válido")]
        public int EstudianteId { get; set; }

        [ForeignKey("EstudianteId")]
        public virtual Estudiantes? Estudiante { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido")]
        public int LibroId { get; set; }

        [ForeignKey("LibroId")]
        public virtual Libros? Libro { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha de devolución")]
        public DateTime FechaDevolucion { get; set; } = DateTime.Now;

        public DateTime FechaPrestamo { get; set; } = DateTime.Now;

        public bool Devuelto { get; set; } = false;
    }
}
