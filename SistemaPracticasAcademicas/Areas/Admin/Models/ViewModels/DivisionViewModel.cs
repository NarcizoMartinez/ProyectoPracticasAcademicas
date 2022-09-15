using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;

namespace SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels
{
    public class DivisionViewModel
    {
        public Division Divisiones { get; set; }
        public IEnumerable<Usuario> Docentes { get; set; }
    }
}
