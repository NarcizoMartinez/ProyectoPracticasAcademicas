using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPracticasAcademicas.Models;
using System.Linq;

namespace SistemaPracticasAcademicas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        public AdminController(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public sistema_academicoContext Context { get; }
        [Route("/Admin/Dashboard")]
        public IActionResult Index()
        {
            ViewBag.TotalPracticas = Context.Practicas.Count();
            ViewBag.PracticaActiva = Context.Practicas.Where(p => p.Activo == 1).Count();
            ViewBag.PracticaInactiva = Context.Practicas.Where(p => p.Activo == 0).Count();
            ViewBag.TotalDocentes=Context.Usuarios.Count();
            return View();
        }
    }
}
