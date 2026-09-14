using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models
{
    public class Libro
    {
        [Key]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Autor { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int? AnioPublicacion { get; set; }

    }
}
