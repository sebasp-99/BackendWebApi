using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;          // Para acceder a ApplicationDbContext
using TodoApi.DTOs;          // Para usar los DTOs de Login y Registro
using TodoApi.Models;        // Para usar el modelo Usuario
using TodoApi.Services;      // Para usar TokenService
using System.Linq;           // Para consultas LINQ
using TodoApi.Enums;

namespace TodoApi.Controllers
{
    // Esta clase es un controlador API
    [ApiController]

    // Ruta base para todos los endpoints de este controlador
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        // Inyectamos las dependencias (DbContext y TokenService) en el constructor
        public AccountController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // Endpoint para registrar un nuevo usuario
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            // Verifica si el nombre de usuario ya existe
            if (_context.Usuarios.Any(u => u.NombreUsuario == dto.NombreUsuario))
                return BadRequest("El nombre de usuario ya está en uso.");

            // Crea un nuevo usuario con la contraseña hasheada
            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Correo = dto.Correo,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                FechaCreacion = DateTime.UtcNow,
                Rol = Enum.TryParse<UserRole>(dto.Rol, true, out var rol) ? rol : UserRole.User
            };

            // Agrega y guarda en la base de datos
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuario registrado exitosamente.");
        }

        // Endpoint para login
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            // Busca usuario por nombre de usuario
            var usuario = _context.Usuarios.SingleOrDefault(u => u.NombreUsuario == dto.NombreUsuario);

            // Verifica si el usuario existe y la contraseña coincide
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.ContrasenaHash))
                return Unauthorized("Credenciales inválidas.");

            // Genera un token JWT
            var token = _tokenService.CreateToken(usuario);

            // Retorna el token al cliente
            return Ok(new { token });
        }
    }
}
