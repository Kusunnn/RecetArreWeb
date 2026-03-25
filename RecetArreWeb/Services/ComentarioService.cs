using System.Net.Http.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IComentarioService
    {
        Task<List<ComentarioDto>> ObtenerTodos();
        Task<List<ComentarioDto>> ObtenerPorReceta(int recetaId);
        Task<ComentarioDto?> CrearComentario(ComentarioCreacionDto comentarioDto);
    }

    public class ComentarioService : IComentarioService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Comentarios";

        public ComentarioService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<ComentarioDto>> ObtenerTodos()
        {
            try
            {
                var comentarios = await httpClient.GetFromJsonAsync<List<ComentarioDto>>(endpoint);
                return comentarios ?? new List<ComentarioDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener comentarios: {ex.Message}");
                return new List<ComentarioDto>();
            }
        }

        public async Task<List<ComentarioDto>> ObtenerPorReceta(int recetaId)
        {
            try
            {
                var comentarios = await httpClient.GetFromJsonAsync<List<ComentarioDto>>($"{endpoint}/receta/{recetaId}");
                return comentarios ?? new List<ComentarioDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener comentarios de la receta {recetaId}: {ex.Message}");
                return new List<ComentarioDto>();
            }
        }

        public async Task<ComentarioDto?> CrearComentario(ComentarioCreacionDto comentarioDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, comentarioDto);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al crear comentario: {error}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<ComentarioDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear comentario: {ex.Message}");
                return null;
            }
        }
    }
}
