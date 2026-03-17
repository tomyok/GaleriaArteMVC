using System.ComponentModel.DataAnnotations;

namespace GaleriaArte.Models
{
    public class Artista
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public string Pais { get; set; }
    }
}
