using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Solo usuarios autenticados pueden usar
    public class AssignmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las tareas del usuario autenticado
        [HttpGet]
        public async Task<ActionResult<List<AssignmentDto>>> Get()
        {
            var userId = GetUserId();

            var tareas = await _context.Tareas
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

            return Ok(tareas);
        }

        // Obtener tarea por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentDto>> GetById(int id)
        {
            var userId = GetUserId();

            var tarea = await _context.Tareas
                .Where(t => t.Id == id && t.UsuarioId == userId)
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
                .FirstOrDefaultAsync();

            if (tarea == null)
                return NotFound();

            return Ok(tarea);
        }

        // Crear nueva tarea
        [HttpPost]
        public async Task<ActionResult> Create(CreateAssignmentDto dto)
        {
            var userId = GetUserId();

            var tarea = new Tarea
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId,
                EstadoId = dto.EstadoId,
                UsuarioId = userId,
                FechaCreacion = DateTime.UtcNow

            };

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tarea.Id }, tarea);
        }

        // Actualizar tarea
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CreateAssignmentDto dto)
        {
            var userId = GetUserId();

            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

            if (tarea == null)
                return NotFound();

            tarea.Titulo = dto.Titulo;
            tarea.Descripcion = dto.Descripcion;
            tarea.CategoriaId = dto.CategoriaId;
            tarea.EstadoId = dto.EstadoId;
          

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Eliminar tarea
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

            if (tarea == null)
                return NotFound();

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método privado para obtener el UserId del token
        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("UserId claim no encontrado");

            return int.Parse(userIdClaim.Value);
        }
    }
}
