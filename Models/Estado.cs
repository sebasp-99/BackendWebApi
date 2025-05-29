using System.Collections.Generic;
using System.Threading;

namespace TodoApi.Models
{
    public class Estado
    {
        public int Id { get; set; }
        public string NombreEstado { get; set; } = string.Empty; // Ej: Pendiente, En Progreso, Completada

        public ICollection<Tarea> Tareas { get; set; }
    }
}
