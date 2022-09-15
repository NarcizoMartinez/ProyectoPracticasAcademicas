using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPracticasAcademicas.Areas.Repository;
using SistemaPracticasAcademicas.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace SistemaPracticasAcademicas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AsignaturasController : Controller
    {
        public AsignaturasController(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public sistema_academicoContext Context { get; }
        public IActionResult Index(string id)
        {
            AsignaturasRepository _repo = new(Context);

            var _entity = _repo.GetAsignaturas();
            if (string.IsNullOrEmpty(id))
                return View(_entity);
            else
                return View(_entity.Where(n=>n.Nombre.ToUpper().Contains(id.ToUpper())));
        }
        [Authorize(Roles ="Administrador")]
        public IActionResult Agregar()
        {
            return View(new Asignatura());
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar(Asignatura _asignatura)
        {
            AsignaturasRepository _repo = new(Context);

            _repo.Validar(_asignatura);
            if(!_repo.Success)
            {
                foreach (var _error in _repo.Error)
                    ModelState.AddModelError("", _error);
                return View(_asignatura);
            }
            else
            {
                _repo.Agregar(_asignatura);
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int _id)
        {
            AsignaturasRepository _repo = new(Context);

            var _entity = _repo.GetAsignaturaById(_id);
            if (_entity == null)
                return RedirectToAction("Index");
            return View(_entity);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(Asignatura _asignatura)
        {
            AsignaturasRepository _repo = new(Context);

            var _entity = _repo.GetAsignaturaById(_asignatura.Id);
            if (_entity == null)
                ModelState.AddModelError("", "La asignatura no se ha encontrado ó ya fue eliminada.");
            else
            {
                _repo.Validar(_asignatura);
                if(!_repo.Success)
                {
                    foreach (var _error in _repo.Error)
                        ModelState.AddModelError("", _error);
                    return View(_asignatura);
                }
                else
                {
                    _entity.Clave = _asignatura.Clave;
                    _entity.Nombre = _asignatura.Nombre;
                    _repo.Editar(_entity);
                    return RedirectToAction("Index");
                }
            }
            return View(_asignatura);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int _id)
        {
            AsignaturasRepository _repo = new(Context);

            var entity = _repo.GetAsignaturaById(_id);
            if (entity == null)
            {
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(Asignatura _asignatura)
        {
            AsignaturasRepository _repo = new AsignaturasRepository(Context);

            var _entity = _repo.GetAsignaturaById(_asignatura.Id);
            if (_entity == null)
            {
                ModelState.AddModelError("", "La asignatura no se ha encontrado ó ya fue eliminada.");
                return View(_asignatura);
            }
            else
            {
                _repo.Eliminar(_entity);
                return RedirectToAction("Index");
            }
        }
    }
}
