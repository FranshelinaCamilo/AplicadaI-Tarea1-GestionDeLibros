using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models
{
    public class Estudiantes
    {
        [Key]
        public int EstudianteId { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombres { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Direccion { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateOnly FechaNacimiento { get; set; }
    }
}
