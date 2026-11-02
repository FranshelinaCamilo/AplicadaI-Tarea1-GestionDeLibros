namespace RegistroLibros.Api.DTOs;

public class EstudianteDto
{
    public int EstudianteId { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateOnly FechaNacimiento { get; set; }
}
