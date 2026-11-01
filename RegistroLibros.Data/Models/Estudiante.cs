using System;
using System.Collections.Generic;

namespace RegistroLibros.Data.Models;

public partial class Estudiante
{
    public int EstudianteId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
