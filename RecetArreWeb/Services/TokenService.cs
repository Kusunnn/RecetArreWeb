using Microsoft.JSInterop;

namespace RecetArreWeb.Services
{
    public interface ITokenService
    {
        Task GuardarToken(string token, DateTime expiracion);
        Task<string?> ObtenerToken();
        Task<DateTime?> ObtenerExpiracion();
        Task<bool> EstaAutenticado();
        Task EliminarToken();

    }
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime jSRuntime;
        private const string TOKEN_KEY = "authToken";
        private const string EXPIRACION_KEY = "tokenExpiracion";

        public TokenService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }
        public async Task EliminarToken()
        {
            await jSRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await jSRuntime.InvokeVoidAsync("localStorage.removeItem", EXPIRACION_KEY);

        }

        public async Task<bool> EstaAutenticado()
        {
            var token = await ObtenerToken();
            return !string.IsNullOrEmpty(token);
            
        }

        public async Task GuardarToken(string token, DateTime expiracion)
        {
            await jSRuntime.InvokeVoidAsync("localStorage.setItem",TOKEN_KEY,token);
            await jSRuntime.InvokeVoidAsync("localStorage.setItem",EXPIRACION_KEY,expiracion.ToString("o"));
            //Formato ISO 8601 (2024-12-15T10:30:00Z)
            
        }

        public async Task<DateTime?> ObtenerExpiracion()
        {
            try
            {
                var expiracionStr = await jSRuntime.InvokeAsync<string?>("localStorage.getItem",EXPIRACION_KEY);
                if(string.IsNullOrEmpty(expiracionStr))
                    return null;
                if(DateTime.TryParse(expiracionStr, out var expiracion))
                    return expiracion;

                return null;
            }
            catch
            {
                return null;
            }
        }

        // Task<string> ITokenService.ObtenerToken()
        // {
        //     throw new NotImplementedException();
        // }

        public async Task<string?> ObtenerToken()
        {
            try
            {
                //1. Leer el token de localStorage
                var token = await jSRuntime.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);

                //2. Si no hay token, devolver null
                if(string.IsNullOrEmpty(token))
                    return null;

                // Verificar si el token expiro
                var expiracion = await ObtenerExpiracion();
                if(expiracion.HasValue && expiracion.Value < DateTime.UtcNow)
                {
                    //Token expirado, eliminarlo
                    await EliminarToken();
                    return null;
                }
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el token: {ex.Message}");
                return null;
            }
        }
    }
}