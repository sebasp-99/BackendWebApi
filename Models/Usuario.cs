using TodoApi.Enums;

namespace TodoApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string ContrasenaHash { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public UserRole Rol { get; set; } = UserRole.User; // 👈 Nuevo campo
    }
}

