namespace RegistroLibros.API.DTOs;

public class PrestamoDto
{
    public int PrestamoId { get; set; }
    public int EstudianteId { get; set; }
    public int LibroId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public bool Devuelto { get; set; }
}
