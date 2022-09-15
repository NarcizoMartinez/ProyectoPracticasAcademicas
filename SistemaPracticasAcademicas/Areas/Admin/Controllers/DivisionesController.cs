using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels;
using SistemaPracticasAcademicas.Areas.Repository;
using SistemaPracticasAcademicas.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace SistemaPracticasAcademicas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class DivisionesController : Controller
    {
        public DivisionesController(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public sistema_academicoContext Context { get; }
        public IActionResult Index(string id)
        {
            DivisionesRepository _repo = new(Context);

            var _entity = _repo.GetDivisionesWithIdJefeNavigation();
            if (string.IsNullOrWhiteSpace(id))
                return View(_entity);
            else
                return View(_entity.Where(n => n.Nombre.ToUpper().Contains(id.ToUpper())));
        }
        public IActionResult Agregar()
        {
            DivisionViewModel _docenteVM = new();
            DivisionesRepository _repo = new(Context);

            _docenteVM.Docentes = _repo.GetAllJefeDepartamento();
            return View(_docenteVM);
        }
        [HttpPost]
        public IActionResult Agregar(DivisionViewModel _divisionVM)
        {
            DivisionesRepository _repo = new(Context);

            _divisionVM.Docentes = _repo.GetAllJefeDepartamento();
            _repo.Validar(_divisionVM);
            if (!_repo.Success)
            {
                foreach (var _error in _repo.Error)
                    ModelState.AddModelError("", _error);
                return View(_divisionVM);
            }
            else
            {
                _repo.Agregar(_divisionVM);
                return RedirectToAction("Index");
            }
        }
        public IActionResult Editar(int _id)
        {
            DivisionViewModel _divisionVM = new();
            DivisionesRepository _repo = new(Context);

            _divisionVM.Docentes = _repo.GetAllJefeDepartamento();
            _divisionVM.Divisiones = _repo.GetDivisionById(_id);

            if (_divisionVM == null)
                return RedirectToAction("Index");
            return View(_divisionVM);
        }
        [HttpPost]
        public IActionResult Editar(DivisionViewModel _divisionVM)
        {
            DivisionesRepository _repo = new(Context);

            _divisionVM.Docentes = _repo.GetAllJefeDepartamento();
            var _division = _repo.GetDivisionById(_divisionVM.Divisiones.Id);
            if (_division == null)
                ModelState.AddModelError("", "No se ha encontrado la division ó ya ha sido eliminada.");
            else
            {
                _repo.Validar(_divisionVM);
                if (!_repo.Success)
                {
                    foreach (var _error in _repo.Error)
                        ModelState.AddModelError("", _error);
                    return View(_divisionVM);
                }
                else
                {
                    _division.Clave = _divisionVM.Divisiones.Clave;
                    _division.Nombre = _divisionVM.Divisiones.Nombre;
                    _division.IdJefe = _divisionVM.Divisiones.IdJefe;
                    _repo.Editar(_division);
                    _repo.Save();
                    return RedirectToAction("Index");
                }
            }
            return View(_divisionVM);
        }
        public IActionResult Eliminar(int _id)
        {
            DivisionesRepository _repo = new(Context);

            var _entity = _repo.GetDivisionById(_id);
            if (_entity == null)
                return RedirectToAction("Index");
            return View(_entity);
        }
        [HttpPost]
        public IActionResult Eliminar(Division _division)
        {
            DivisionesRepository _repo = new(Context);

            var _entity = _repo.GetDivisionById(_division.Id);
            if (_entity == null)
                ModelState.AddModelError("", "No se ha encontrado la división ó ya ha sido eliminada.");
            else
            {
                _repo.Elminar(_entity);
                return RedirectToAction("Index");
            }
            return View(_division);
        }
    }
}
