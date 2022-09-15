using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels;
using SistemaPracticasAcademicas.Areas.Repository;
using SistemaPracticasAcademicas.Models;
using System;
using System.Linq;

namespace SistemaPracticasAcademicas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PracticasController : Controller
    {
        public sistema_academicoContext Context { get; }

        public PracticasController(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public IActionResult Index(string buscar)
        {
            PracticasRepository _repo = new(Context);
            var _entity = _repo.GetAllPracticasWithRelacion();
            if (string.IsNullOrWhiteSpace(buscar))
                return View(_entity);
            else
                return View(_repo.SearchPracticaByString(_entity, buscar));
        }
        public IActionResult Agregar()
        {
            PracticasViewModel _practicaVM = new();
            AsignaturasRepository _repo = new(Context);

            _practicaVM.Asignaturas = _repo.GetAsignaturas();
            return View(_practicaVM);
        }
        [HttpPost]
        public IActionResult Agregar(PracticasViewModel _practicaVM)
        {
            PracticasRepository _repo = new(Context);
            AsignaturasRepository _repoAsignatura = new(Context);

            var _docente = Context.Usuarios.SingleOrDefault(u => u.Nombre == User.Identity.Name);

            _practicaVM.Asignaturas = _repoAsignatura.GetAsignaturas();
            _repo.Validar(_practicaVM);
            if (!_repo.Success)
            {
                foreach (var _error in _repo.Error)
                    ModelState.AddModelError("", _error);
                return View(_practicaVM);
            }
            else
            {
                _practicaVM.Practica.Activo = 1;
                _practicaVM.Practica.IdUsuario = _docente.Id;
               _repo.Agregar(_practicaVM);
                return RedirectToAction("Index");
            }
        }
        public IActionResult Editar(int _id)
        {
            PracticasViewModel _practicaVM = new();
            PracticasRepository _repo = new(Context);
            AsignaturasRepository _repoAsignatura = new(Context);

            _practicaVM.Asignaturas = _repoAsignatura.GetAsignaturas();
            _practicaVM.Practica = _repo.GetPracticaByidWithRelacion(_id);
            if (_practicaVM == null)
                return RedirectToAction("Index");
            return View(_practicaVM);
        }
        [HttpPost]
        public IActionResult Editar(PracticasViewModel _practicaVM)
        {
            PracticasRepository _repo = new(Context);
            AsignaturasRepository _repoAsignatura = new(Context);

            var _docente = Context.Usuarios.SingleOrDefault(u => u.Nombre == User.Identity.Name);
            _practicaVM.Asignaturas = _repoAsignatura.GetAsignaturas();
            var _entity = _repo.GetPracticaByidWithRelacion(_practicaVM.Practica.Id);
            if (_entity == null)
                ModelState.AddModelError("", "No se ha encontrado la práctica ó ya ha sido eliminada.");
            if (_entity.IdUsuarioNavigation.IdDivision == _docente.IdDivision)
            {
                _repo.Validar(_practicaVM);
                if (!_repo.Success)
                {
                    foreach (var _error in _repo.Error)
                        ModelState.AddModelError("", _error);
                    return View(_practicaVM);
                }
                else
                {
                    _entity.Activo = 1;
                    _entity.Nombre = _practicaVM.Practica.Nombre;
                    _entity.NombreUnidad = _practicaVM.Practica.NombreUnidad;
                    _entity.NumUnidad = _practicaVM.Practica.NumUnidad;
                    _entity.Tema = _practicaVM.Practica.Tema;
                    _entity.Planteamiento = _practicaVM.Practica.Planteamiento;
                    _entity.Objetivo = _practicaVM.Practica.Objetivo;
                    _entity.Periodo = _practicaVM.Practica.Periodo + $"{DateTime.Today.Year}";
                    _entity.Resultado = _practicaVM.Practica.Resultado;
                    _repo.Editar(_entity);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "No puede editar una práctica que no pertenece a su división.");
                return View(_practicaVM);
            }
            
        }
        public IActionResult Eliminar(int _id)
        {
            PracticasRepository _repo = new(Context);

            var _entity = _repo.GetPracticaByidWithRelacion(_id);
            if (_entity == null)
                return RedirectToAction("Index");
            return View(_entity);
        }
        [HttpPost]
        public IActionResult Eliminar(Practica _practica)
        {
            PracticasRepository _repo = new(Context);

            var _docente = Context.Usuarios.SingleOrDefault(u => u.Nombre == User.Identity.Name);
            var _entity = _repo.GetPracticaByidWithRelacion(_practica.Id);
            if (_entity == null)
                ModelState.AddModelError("", "No se ha encontrado la práctica o ya ha sido eliminada.");
            else
            {
                if (_entity.IdUsuarioNavigation.IdDivision == _docente.IdDivision)
                {
                    _entity.Activo = 0;
                    _repo.Editar(_entity);
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", "No puede eliminar una práctica que no pertenece a su división");
            }
            return View(_practica);
        }
        public IActionResult EliminarFisica(int _id)
        {
            PracticasRepository _repo = new(Context);

            var _entity = _repo.GetPracticaById(_id);
            if (_entity == null)
                return RedirectToAction("Index");
            return View(_entity);
        }
        [HttpPost]
        public IActionResult EliminarFisica(Practica _practica)
        {
            PracticasRepository _repo = new(Context);

            var _entity = _repo.GetPracticaById(_practica.Id);
            if (_entity == null)
                ModelState.AddModelError("", "No se ha encontrado la practica o ya ha sido eliminada.");
            else
            {
                _repo.Eliminar(_entity);
                return RedirectToAction("Inactivas");
            }
            return View(_practica);
        }
        public IActionResult Ver(int _id)
        {
            PracticasRepository _repo = new(Context);

            var _entity = _repo.GetPracticaByidWithRelacion(_id);
            if (_entity==null)
                return RedirectToAction("Index");
            return new ViewAsPdf(_entity);
        }
    }
}
