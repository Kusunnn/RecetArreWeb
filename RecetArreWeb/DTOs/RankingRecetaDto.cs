namespace RecetArreWeb.DTOs
{
    public class RankingRecetaDto
    {
        public int Posicion { get; set; }
        public int RecetaId { get; set; }
        public string Titulo { get; set; } = default!;
        public string? Descripcion { get; set; }
        public decimal PromedioRating { get; set; }
        public int TotalRatings { get; set; }
        public int TiempoPreparacionMinutos { get; set; }
        public int TiempoCoccionMinutos { get; set; }
        public int Porciones { get; set; }
        public DateTime ModificadoUtc { get; set; }
        public DateTime UltimoRatingUtc { get; set; }
    }
}
