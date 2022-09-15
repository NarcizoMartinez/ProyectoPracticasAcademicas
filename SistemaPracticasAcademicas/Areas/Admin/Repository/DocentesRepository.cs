using Microsoft.EntityFrameworkCore;
using SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels;
using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SistemaPracticasAcademicas.Areas.Repository
{
    public class DocentesRepository
    {
        public DocentesRepository(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public sistema_academicoContext Context { get; }
        public bool Success = false;
        public List<string> Error;
        #region CRUD
        public void Agregar(DocenteViewModel _docenteVM)
        {
            Add(_docenteVM);
            Save();
        }
        public void Editar(Usuario _docente)
        {
            Update(_docente);
            Save();
        }
        public void Eliminar(Usuario _docente)
        {
            Delete(_docente);
            Save();
        }
        #endregion
        #region METHODS
        /// <summary>
        /// Método para validar los datos recibidos.
        /// </summary>
        public void Validar(DocenteViewModel _docenteVM)
        {
            Error = new List<string>();
            if (string.IsNullOrWhiteSpace(_docenteVM.Usuario.NumeroControl))
                Error.Add("Escriba el número de control.");
            else
            {
                if (!Regex.Match(_docenteVM.Usuario.NumeroControl, @"[\d]{4,5}").Success)
                    Error.Add("El formato del número de control no es válido.");
                if (Context.Usuarios.Any(u => u.NumeroControl == _docenteVM.Usuario.NumeroControl && u.Id != _docenteVM.Usuario.Id))
                    Error.Add("Ya existe un usuario con ese número de control.");
            }
            if (_docenteVM.Usuario.IdDivision == 0)
                Error.Add("Seleccione una división.");
            if (_docenteVM.Usuario.IdRol == 0)
                Error.Add("Seleccione un rol.");
            if(_docenteVM.Usuario.NumeroControl != null)
                if (_docenteVM.Usuario.IdRol == 2 && Context.Divisions.Any(d => d.IdJefe != int.Parse(_docenteVM.Usuario.NumeroControl)))
                    Error.Add("Ya existe un Jefe asignado a esa división");
            if (string.IsNullOrWhiteSpace(_docenteVM.Usuario.Nombre))
                Error.Add("Escriba el nombre del usuario.");
            if (Error.Count == 0)
                Success = true;
            else
                Success = false;
        }
        /// <summary>
        /// Devuelve una colección que incluye la relacion entre Docente y Rol.
        /// </summary>
        public List<Usuario> GetAllPersonal()
        {
            return Context.Usuarios.Include(r => r.IdRolNavigation).Include(d => d.IdDivisionNavigation).Where(r=>r.IdRol != 1).OrderBy(u => u.Nombre).ToList();
        }
        /// <summary>
        /// Devuelve una colección de tipo Rol ordenada por id.
        /// </summary>
        public List<Role> GetAllRoles()
        {
            return Context.Roles.OrderBy(r=>r.Id).ToList();
        }
        /// <summary>
        /// Devuelve una colección de tipo Rol donde el rol == docente.
        /// </summary>
        public List<Role> GetRolDocente()
        {
            return Context.Roles.Where(r => r.Rol == "Docente").ToList();
        }
        /// <summary>
        /// Devuelve el primer valor dado el parametro
        /// </summary>
        public Usuario GetDocenteById(int _id)
        {
            return Context.Usuarios.FirstOrDefault(x => x.Id == _id);
        }
        /// <summary>
        /// Devuelve el prmer valor dado el parametro incluyendo la relacion entre Docente y Rol.
        /// </summary>
        public Usuario GetDocenteByIdWithIdRolNaviagation(int _id)
        {
            return Context.Usuarios.Include(x => x.IdRolNavigation).FirstOrDefault(x => x.Id == _id);
        }
        /// <summary>
        /// Método para filtrar el personal por Nombre o Departamento.
        /// </summary>
        public IEnumerable<Usuario> SearchPersonalByString(IEnumerable<Usuario> _docente, string _buscar)
        {
            string _palabra = _buscar.ToUpper();
            var a = _docente.Where(n => n.Nombre.ToUpper().Contains(_palabra)
                 || n.IdDivisionNavigation.Nombre.ToUpper().Contains(_palabra));
            return a;
        }
        #endregion
        #region ENTITYFRAMEWORKCORE
        public void Add(DocenteViewModel _docenteVM)
        {
            Context.Add(_docenteVM.Usuario);
        }
        public void Update(Usuario _docente)
        {
            Context.Update(_docente);
        }
        public void Delete(Usuario _docente)
        {
            Context.Remove(_docente);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        #endregion
    }
}
