
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models;

public class Prestamos
{
    [Key]
    public int PrestamoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un estudiante válido")]
    public int EstudianteId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un libro válido")]
    public int LibroId { get; set; }

    [ForeignKey(nameof(EstudianteId))]
    public virtual Estudiantes Estudiante { get; set; } = null!;

    [ForeignKey(nameof(LibroId))]
    public virtual Libros Libro { get; set; } = null!;
}
