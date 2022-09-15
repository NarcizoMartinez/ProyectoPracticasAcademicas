using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;

namespace SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels
{
    public class DocenteViewModel
    {
        public Usuario Usuario { get; set; }
        public IEnumerable<Division> Divisiones { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}
