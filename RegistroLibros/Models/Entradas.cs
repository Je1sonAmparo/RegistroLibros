using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Debe ingresar un concepto")]
    public string? Concepto { get; set; }

    public decimal Total { get; set; }
}
