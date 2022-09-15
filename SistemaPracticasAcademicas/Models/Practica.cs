using System;
using System.Collections.Generic;

#nullable disable

namespace SistemaPracticasAcademicas.Models
{
    public partial class Practica
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tema { get; set; }
        public string NombreUnidad { get; set; }
        public int NumUnidad { get; set; }
        public string Planteamiento { get; set; }
        public string Periodo { get; set; }
        public string Objetivo { get; set; }
        public string Resultado { get; set; }
        public ulong? Activo { get; set; }
        public int IdAsignatura { get; set; }
        public int IdUsuario { get; set; }

        public virtual Asignatura IdAsignaturaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
