using System.Net.Http.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IRatingService
    {
        Task<List<RatingDto>> ObtenerTodos();
        Task<List<RatingDto>> ObtenerPorReceta(int recetaId);
        Task<RatingDto?> CrearOActualizarRating(RatingCreacionDto ratingDto);
    }

    public class RatingService : IRatingService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Ratings";

        public RatingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<RatingDto>> ObtenerTodos()
        {
            try
            {
                var ratings = await httpClient.GetFromJsonAsync<List<RatingDto>>(endpoint);
                return ratings ?? new List<RatingDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ratings: {ex.Message}");
                return new List<RatingDto>();
            }
        }

        public async Task<List<RatingDto>> ObtenerPorReceta(int recetaId)
        {
            try
            {
                var ratings = await httpClient.GetFromJsonAsync<List<RatingDto>>($"{endpoint}/receta/{recetaId}");
                return ratings ?? new List<RatingDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ratings de la receta {recetaId}: {ex.Message}");
                return new List<RatingDto>();
            }
        }

        public async Task<RatingDto?> CrearOActualizarRating(RatingCreacionDto ratingDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, ratingDto);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al guardar rating: {error}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<RatingDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar rating: {ex.Message}");
                return null;
            }
        }
    }
}
