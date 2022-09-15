using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPracticasAcademicas.Areas.Admin.Repository;
using SistemaPracticasAcademicas.Areas.Repository;
using SistemaPracticasAcademicas.Models;
using System.Security.Claims;

namespace SistemaPracticasAcademicas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ConfiguracionController : Controller
    {
        public ConfiguracionController(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public sistema_academicoContext Context { get; }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string _pass, string _pass2)
        {
            DocentesRepository _repoDocente = new(Context);
            ConfiguracionRepository _repo =new(Context);
           
            var _idDocente = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _entity = _repoDocente.GetDocenteById(int.Parse(_idDocente));
            _repo.ValidarContraseña(_pass, _pass2);
            if(!_repo.Success)
            {
                foreach (var _error in _repo.Error)
                    ModelState.AddModelError("", _error);
                return View();
            }
            else
            {
                _entity.Contrasena = _pass;
                _repo.Update(_entity);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
