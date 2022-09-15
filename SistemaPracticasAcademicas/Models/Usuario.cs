using System;
using System.Collections.Generic;

#nullable disable

namespace SistemaPracticasAcademicas.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Divisions = new HashSet<Division>();
            Practicas = new HashSet<Practica>();
        }

        public int Id { get; set; }
        public string NumeroControl { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; }
        public int IdRol { get; set; }
        public int IdDivision { get; set; }

        public virtual Division IdDivisionNavigation { get; set; }
        public virtual Role IdRolNavigation { get; set; }
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual ICollection<Practica> Practicas { get; set; }
    }
}
