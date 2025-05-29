using System;
using System.Collections.Generic;
using System.Threading;

namespace TodoApi.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación
        public ICollection<Tarea> Tareas { get; set; }
    }
}
