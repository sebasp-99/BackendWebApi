using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]  // Solo admins autorizados
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _context.Usuarios
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Correo = u.Correo,
                    FechaCreacion = u.FechaCreacion,
                    Rol = u.Rol.ToString()
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/admin/tasks/user/{userId}
        [HttpGet("tasks/user/{userId}")]
        public async Task<ActionResult<List<AssignmentDto>>> GetTasksByUser(int userId)
        {
            var tasks = await _context.Tareas
                .Where(t => t.UsuarioId == userId)
                .Select(t => new AssignmentDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion,
                    CategoriaId = t.CategoriaId,
                    EstadoId = t.EstadoId,
                    FechaCreacion = t.FechaCreacion,
                    FechaVencimiento = t.FechaVencimiento
                })
                .ToListAsync();

            return Ok(tasks);
        }
    }
}
