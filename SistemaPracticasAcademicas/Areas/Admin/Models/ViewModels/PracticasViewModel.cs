using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;

namespace SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels
{
    public class PracticasViewModel
    {
        public Practica Practica { get; set; }
        public IEnumerable<Asignatura> Asignaturas { get; set;}
    }
}
