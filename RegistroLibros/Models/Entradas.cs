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

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El total debe ser mayor que cero")]
    public decimal Total { get; set; }
}
