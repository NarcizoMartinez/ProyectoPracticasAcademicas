using System;
using System.Collections.Generic;

#nullable disable

namespace SistemaPracticasAcademicas.Models
{
    public partial class Role
    {
        public Role()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Rol { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
