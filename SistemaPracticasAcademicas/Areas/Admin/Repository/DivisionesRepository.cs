using Microsoft.EntityFrameworkCore;
using SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels;
using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SistemaPracticasAcademicas.Areas.Repository
{
    public class DivisionesRepository
    {
        public DivisionesRepository(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public bool Success = false;
        public List<string> Error;
        public sistema_academicoContext Context { get; }
        #region CRUD
        public void Agregar(DivisionViewModel _divisionVM)
        {
            Add(_divisionVM);
            Save();
        }
        public void Editar(Division _division)
        {
            Update(_division);
            Save();
        }
        public void Elminar(Division _division)
        {
            Delete(_division);
            Save();
        }
        #endregion
        #region METHODS
        /// <summary>
        /// Método para validar los datos recibidos.
        /// </summary>
        public void Validar(DivisionViewModel _divisionVM)
        {
            Error = new List<string>();
            if (string.IsNullOrWhiteSpace(_divisionVM.Divisiones.Clave))
                Error.Add("Esriba la clave de la división.");
            else
            {
                if (!Regex.Match(_divisionVM.Divisiones.Clave, @"^[A-Z]{1}").Success)
                    Error.Add("El formato de la clave no es válido.");
                if (Context.Divisions.Any(d => d.Clave == _divisionVM.Divisiones.Clave && d.Id != _divisionVM.Divisiones.Id))
                    Error.Add("Ya se encuentra una división con esa clave.");
            }
            if (string.IsNullOrWhiteSpace(_divisionVM.Divisiones.Nombre))
                Error.Add("Escriba el nombre de la división.");
            if (_divisionVM.Divisiones.IdJefe == 0)
                Error.Add("Seleccione un jefe de división.");
            if (Error.Count == 0)
                Success = true;
            else
                Success = false;
        }
        /// <summary>
        /// Devuelve una colección que incluye la relacion entre Division y Docente.
        /// </summary>
        public List<Division> GetDivisionesWithIdJefeNavigation()
        {
            return Context.Divisions.Include(x => x.IdJefeNavigation).OrderBy(n => n.Clave).Where(d=>d.Nombre != "ADMINISTRADOR DEL SISTEMA").ToList();
        }
        /// <summary>
        /// Devuelve una colección de datos de tipo Division ordenados por nombre.
        /// </summary>
        public List<Division> GetDivisiones()
        {
            return Context.Divisions.OrderBy(d => d.Clave).ToList();
        }
        /// <summary>
        /// Devuelve el primer valor dado el parametro.
        /// </summary>
        public Division GetDivisionById(int _id)
        {
            return Context.Divisions.Include(x => x.IdJefeNavigation).FirstOrDefault(d => d.Id == _id);
        }
        public List<Usuario> GetAllJefeDepartamento()
        {
            return Context.Usuarios.Where(x => x.IdRolNavigation.Rol == "Jefe Departamento").OrderBy(u => u.Nombre).ToList();
        }
        #endregion
        #region ENTITYFFRAMEWORKCORE
        public void Add(DivisionViewModel _divisionVM)
        {
            Context.Add(_divisionVM.Divisiones);
        }
        public void Update(Division _division)
        {
            Context.Update(_division);
        }
        public void Delete(Division _division)
        {
            Context.Remove(_division);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        #endregion
    }
}
