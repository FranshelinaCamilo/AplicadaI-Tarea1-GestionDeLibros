namespace RegistroLibros.Api.DTOs;

public class PrestamoDto
{
    public int PrestamoId { get; set; }

    public int EstudianteId { get; set; }

    public int LibroId { get; set; }

    public DateOnly Fecha { get; set; }
}
