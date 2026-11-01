namespace RegistroLibros.API.DTOs.Estudiantes;

public class EstudianteSalidaDto
{
    public int EstudianteId { get; set; }
    public string? Nombres { get; set; }
    public string? Direccion { get; set; }
    public string? Email { get; set; }
    public DateTime? FechaNacimiento { get; set; }
}
