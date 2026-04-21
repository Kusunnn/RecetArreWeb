using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IRankingService
    {
        Task<List<RankingRecetaDto>> ObtenerRankingSemanal(DateTime inicioSemanaUtc, DateTime finSemanaUtc, int limite = 10);
    }

    public class RankingService : IRankingService
    {
        private readonly IRecetaService recetaService;
        private readonly IRatingService ratingService;

        public RankingService(IRecetaService recetaService, IRatingService ratingService)
        {
            this.recetaService = recetaService;
            this.ratingService = ratingService;
        }

        public virtual async Task<List<RankingRecetaDto>> ObtenerRankingSemanal(DateTime inicioSemanaUtc, DateTime finSemanaUtc, int limite = 10)
        {
            if (inicioSemanaUtc >= finSemanaUtc)
            {
                return new List<RankingRecetaDto>();
            }

            try
            {
                var recetas = await recetaService.ObtenerTodas();
                var ratings = await ratingService.ObtenerTodos();
                var recetasPublicadas = recetas
                    .Where(r => r.EstaPublicado)
                    .ToDictionary(r => r.Id);

                var ranking = ratings
                    .Where(r => r.CreadoUtc >= inicioSemanaUtc && r.CreadoUtc < finSemanaUtc)
                    .Where(r => recetasPublicadas.ContainsKey(r.RecetaId))
                    .GroupBy(r => r.RecetaId)
                    .Select(grupo =>
                    {
                        var receta = recetasPublicadas[grupo.Key];

                        return new RankingRecetaDto
                        {
                            RecetaId = receta.Id,
                            Titulo = receta.Titulo,
                            Descripcion = receta.Descripcion,
                            PromedioRating = grupo.Average(r => r.Estrellas),
                            TotalRatings = grupo.Count(),
                            TiempoPreparacionMinutos = receta.TiempoPreparacionMinutos,
                            TiempoCoccionMinutos = receta.TiempoCoccionMinutos,
                            Porciones = receta.Porciones,
                            ModificadoUtc = receta.ModificadoUtc,
                            UltimoRatingUtc = grupo.Max(r => r.CreadoUtc)
                        };
                    })
                    .OrderByDescending(r => r.PromedioRating)
                    .ThenByDescending(r => r.TotalRatings)
                    .ThenByDescending(r => r.UltimoRatingUtc)
                    .ThenBy(r => r.Titulo)
                    .Take(limite <= 0 ? 10 : limite)
                    .ToList();

                for (var indice = 0; indice < ranking.Count; indice++)
                {
                    ranking[indice].Posicion = indice + 1;
                }

                return ranking;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al calcular ranking semanal: {ex.Message}");
                return new List<RankingRecetaDto>();
            }
        }
    }

    public class RankingServiceTest : RankingService
    {
        public RankingServiceTest(IRecetaService recetaService, IRatingService ratingService)
            : base(recetaService, ratingService)
        {
        }
    }
}
