using System;
using System.Collections.Generic;

namespace RegistroLibros.Data.Models;

public partial class Libro
{
    public int LibroId { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public int AnioPublicacion { get; set; }

    public bool Disponible { get; set; }

    public int Cantidad { get; set; }

    public virtual ICollection<EntradasDetalle> EntradasDetalles { get; set; } = new List<EntradasDetalle>();

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
