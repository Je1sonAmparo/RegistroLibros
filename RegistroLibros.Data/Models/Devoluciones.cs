using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Data.Models;

public class Devoluciones
{
    [Key]
    public int DevolucionId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un préstamo válido")]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage = "Debe ingresar la fecha de devolución")]
    public DateTime FechaDevolucion { get; set; } = DateTime.Now;

    [ForeignKey("PrestamoId")]
    public virtual Prestamos? Prestamo { get; set; }
}
