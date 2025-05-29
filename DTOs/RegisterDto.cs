namespace TodoApi.DTOs
{
    public class RegisterDto
    {
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }


        public string Rol { get; set; } = "User"; // 👈 Opcional, valor por defecto
    }
}