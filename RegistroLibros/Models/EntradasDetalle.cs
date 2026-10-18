using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models;

public class EntradasDetalle
{
    [Key]
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una entrada válida")]
    public int EntradaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido")]
    public int LibroId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
    public int Cantidad { get; set; }

    [ForeignKey("EntradaId")]
    public virtual Entradas? Entrada { get; set; }

    [ForeignKey("LibroId")]
    public virtual Libros? Libro { get; set; }
}
