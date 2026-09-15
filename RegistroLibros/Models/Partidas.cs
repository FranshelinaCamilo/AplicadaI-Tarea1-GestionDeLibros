using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models
{
    public class Partidas
    {
        [Key]
        public int PartidaId { get; set; }

        [ForeignKey("Jugador")]
        public int JugadorId { get; set; }

        public int Puntuacion { get; set; }

        public DateTime Fecha { get; set; }
    }
}
