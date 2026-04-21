using System.ComponentModel.DataAnnotations;

namespace RecetArreWeb.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public decimal Estrellas { get; set; }
        public DateTime CreadoUtc { get; set; }
        public int RecetaId { get; set; }
        public string UsuarioId { get; set; } = default!;
    }

    public class RatingCreacionDto
    {
        [Required]
        [Range(typeof(decimal), "0.5", "5.0")]
        public decimal Estrellas { get; set; }

        [Required]
        public int RecetaId { get; set; }
    }
}
