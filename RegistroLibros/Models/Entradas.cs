using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }

    [Required(ErrorMessage = "Debe ingresar la fecha de entrada")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Debe ingresar un concepto")]
    public string? Concepto { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor que cero")]
    public decimal Total { get; set; }

    public virtual ICollection<EntradasDetalle> EntradasDetalle { get; set; } = new List<EntradasDetalle>();
}
