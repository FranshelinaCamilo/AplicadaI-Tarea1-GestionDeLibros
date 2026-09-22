using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models
{
    public class Jugadores
    {
        [Key]
        public int JugadorId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string nombre { get; set; } = null!;
    }
}
