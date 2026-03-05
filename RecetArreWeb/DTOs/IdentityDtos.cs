using System.ComponentModel.DataAnnotations;
using System.Data;

namespace RecetArreWeb.DTOs
{
    public class CredencialesUsuario
    {
        // Anotaciones   // Fluent Api
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email es inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
    }

    public class RespuestaAutenticacion
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
    
    }
}