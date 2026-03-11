

using System.Net.Http.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ObtenerTodas();
        Task<CategoriaDto?> ObtenerCategoriaPorId(int id);
        Task<CategoriaDto?> CrearCategoria(CategoriaDto categoriaDto);
        Task<bool> ActualizarCategoria(int id, CategoriaModificacionDto categoriaModificacionDto);
        Task<bool> EliminarCategoria(int id);
    }
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Categorias";

        public CategoriaService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<CategoriaDto>> ObtenerTodasCategorias()
        {
            try
            {
                var categorias = await httpClient.GetFromJsonAsync<List<CategoriaDto>>(endpoint);
                return categorias ?? new List<CategoriaDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorias: {ex.Message}");
                return new List<CategoriaDto>();
            }
        }

        public async Task<CategoriaDto?> ObtenerCategoriaPorId(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<CategoriaDto>($"{endpoint}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categoria {id}: {ex.Message}");
                return null;
            }
        }
        public async Task<CategoriaDto?> CrearCategoria(CategoriaDto categoriaDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, categoriaDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CategoriaDto>();
                }
            
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear categoria: {error}");
                return null;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear categoria: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ActualizarCategoria(int id, CategoriaModificacionDto categoriaDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{endpoint}/{id}", categoriaDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar categoria {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarCategoria(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error al eliminar categoria {id}: {ex.Message}");
                return false;
            }
        }

        public Task<List<CategoriaDto>> ObtenerTodas()
        {
            throw new NotImplementedException();
        }

    }
}
