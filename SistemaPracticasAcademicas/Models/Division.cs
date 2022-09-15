using System;
using System.Collections.Generic;

#nullable disable

namespace SistemaPracticasAcademicas.Models
{
    public partial class Division
    {
        public Division()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public int IdJefe { get; set; }

        public virtual Usuario IdJefeNavigation { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
