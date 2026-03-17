using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaleriaArte.Models
{
    public class Obra
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Titulo { get; set; }
        [StringLength(50)]
        public string Estilo { get; set; }
        [StringLength(250)]
        public string UrlImagen { get; set; }
        [ForeignKey("Artista")]
        public string ArtistaId { get; set; }

        public Artista? Artista { get; set; }
    }
}
