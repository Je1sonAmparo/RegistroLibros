using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models
{
    public class Libros
    {
        [Key]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "Debe ingresar un titulo")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "Debe ingresar un autor")]
        public string? Autor { get; set; }

        [Required(ErrorMessage = "Debe ingresar un año de publicacion")]
        public int? AnoPublicacion { get; set; }
    }
}
