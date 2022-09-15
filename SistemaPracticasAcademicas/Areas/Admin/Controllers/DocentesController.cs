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
    [Authorize]
    public class DocentesController : Controller
    {
        public sistema_academicoContext Context { get; }

        public DocentesController(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public IActionResult Index(string buscar)
        {
            DocentesRepository _repo = new(Context);
            
            var _entity = _repo.GetAllPersonal();
            if (string.IsNullOrWhiteSpace(buscar))
                return View(_entity);
            else
                return View(_repo.SearchPersonalByString(_entity, buscar));
                //return View(_entity.Where(n=>n.Nombre.ToUpper().Contains(id.ToUpper())));
        }
        public IActionResult Agregar()
        {
            DocenteViewModel _docenteVM = new();
            DocentesRepository _repo = new(Context);
            DivisionesRepository _repoDivision = new(Context);

            _docenteVM.Divisiones = _repoDivision.GetDivisiones();
            if (User.IsInRole("Administrador"))
                _docenteVM.Roles = _repo.GetAllRoles();
            else
                _docenteVM.Roles = _repo.GetRolDocente();
            return View(_docenteVM);
        }
        [HttpPost]
        public IActionResult Agregar(DocenteViewModel _docenteVM)
        {
            DocentesRepository _repo = new(Context);
            DivisionesRepository _repoDivision = new(Context);

            _docenteVM.Divisiones = _repoDivision.GetDivisiones();
            if (User.IsInRole("Administrador"))
                _docenteVM.Roles = _repo.GetAllRoles();
            else
                _docenteVM.Roles = _repo.GetRolDocente();
            _repo.Validar(_docenteVM);
            if(!_repo.Success)
            {
                foreach (var _error in _repo.Error)
                    ModelState.AddModelError("", _error);
                return View(_docenteVM);
            }
            else
            {
                _docenteVM.Usuario.Contrasena = _docenteVM.Usuario.NumeroControl;
                _docenteVM.Usuario.CorreoElectronico = _docenteVM.Usuario.NumeroControl + "@rcarbonifera.tecnm.mx";
                _repo.Agregar(_docenteVM);
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int _id)
        {
            DocenteViewModel _docenteVM = new();
            DocentesRepository _repo = new(Context);
            DivisionesRepository _repoDivision = new(Context);

            _docenteVM.Divisiones = _repoDivision.GetDivisiones();
            if (User.IsInRole("Administrador"))
                _docenteVM.Roles = _repo.GetAllRoles();
            else
                _docenteVM.Roles = Context.Roles.Where(r => r.Rol == "Docente");
            _docenteVM.Usuario = _repo.GetDocenteById(_id);
            if (_docenteVM == null)
                return RedirectToAction("Index");
            return View(_docenteVM);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(DocenteViewModel _docenteVM)
        {
            DocentesRepository _repo = new(Context);
            DivisionesRepository _repoDivision = new(Context);

            _docenteVM.Divisiones = _repoDivision.GetDivisiones();
            var _entity = Context.Usuarios.FirstOrDefault(u => u.Id == _docenteVM.Usuario.Id);
            _repo.Validar(_docenteVM);
            if (_repo.Success != true)
            {
                foreach (var _error in _repo.Error)
                    ModelState.AddModelError("", _error);
                return View(_docenteVM);
            }
            else
            {
                _entity.NumeroControl = _docenteVM.Usuario.NumeroControl;
                _entity.Nombre = _docenteVM.Usuario.Nombre;
                _entity.Contrasena = _docenteVM.Usuario.NumeroControl;
                _entity.CorreoElectronico = _docenteVM.Usuario.NumeroControl + "@rcarbonifera.tecnm.mx";
                _entity.IdDivision = _docenteVM.Usuario.IdDivision;
                _entity.IdRol = _docenteVM.Usuario.IdRol;
                _repo.Editar(_entity);
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles ="Administrador")]
        public IActionResult Eliminar(int _id)
        {
            DocentesRepository _repo = new(Context);

            var _entity = _repo.GetDocenteByIdWithIdRolNaviagation(_id);
            if (_entity == null)
                return RedirectToAction("Index");
            if (_entity.IdRolNavigation.Rol == "Jefe Departamento")
                ModelState.AddModelError("", "No puede eliminar al usuario porque es jefe de una división.");
            return View(_entity);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(Usuario _usuario)
        {
            DocentesRepository _repo = new(Context);

            var _entity = _repo.GetDocenteByIdWithIdRolNaviagation(_usuario.Id);
            if (_entity == null)
                ModelState.AddModelError("", "No se ha encontrado el usuario ó ya ha sido eliminado.");
            else
            {
                if (_entity.IdRolNavigation.Rol == "Jefe Departamento")
                    return RedirectToAction("Index");
                else
                {
                    _repo.Eliminar(_entity);
                    return RedirectToAction("Index");
                }
            }
            return View(_usuario);
        }
    }
}
