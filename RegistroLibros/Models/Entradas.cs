using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public string? Concepto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser mayor que cero.")]
    public int Total { get; set; }

    [InverseProperty("Entrada")]
    public ICollection<EntradasDetalle> EntradasDetalle { get; set; } = new List<EntradasDetalle>();
}
