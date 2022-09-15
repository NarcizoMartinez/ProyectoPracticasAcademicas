using System;
using System.Collections.Generic;

#nullable disable

namespace SistemaPracticasAcademicas.Models
{
    public partial class Asignatura
    {
        public Asignatura()
        {
            Practicas = new HashSet<Practica>();
        }

        public int Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Practica> Practicas { get; set; }
    }
}
