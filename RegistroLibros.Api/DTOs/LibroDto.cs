namespace RegistroLibros.Api.DTOs;

public class LibroDto
{
    public int LibroId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public int AnioPublicacion { get; set; }

    public bool Disponible { get; set; }

    public int Cantidad { get; set; }
}
