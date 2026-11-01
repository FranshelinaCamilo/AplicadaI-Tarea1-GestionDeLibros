using System;
using System.Collections.Generic;

namespace RegistroLibros.Data;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    public int EstudianteId { get; set; }

    public int LibroId { get; set; }

    public DateOnly Fecha { get; set; }

    public virtual ICollection<Devolucione> Devoluciones { get; set; } = new List<Devolucione>();

    public virtual Estudiante Estudiante { get; set; } = null!;

    public virtual Libro Libro { get; set; } = null!;
}
