using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models;

public class EntradasDetalle
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public int EntradaId { get; set; }

    [ForeignKey("EntradaId")]
    [InverseProperty("EntradasDetalles")]
    public virtual Entradas? Entrada { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro valido.")]
    public int LibroId { get; set; }

    [ForeignKey("LibroId")]
    public virtual Libros? Libro { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser mayor que cero.")]
    public int Cantidad { get; set; }
}
