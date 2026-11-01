using System;
using System.Collections.Generic;

namespace RegistroLibros.Data.Models;

public partial class Devolucione
{
    public int DevolucionId { get; set; }

    public DateOnly Fecha { get; set; }

    public int PrestamoId { get; set; }

    public virtual Prestamo Prestamo { get; set; } = null!;
}
