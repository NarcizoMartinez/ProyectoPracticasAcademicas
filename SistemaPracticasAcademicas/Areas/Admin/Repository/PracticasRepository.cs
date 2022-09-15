using Microsoft.EntityFrameworkCore;
using SistemaPracticasAcademicas.Areas.Admin.Helpers;
using SistemaPracticasAcademicas.Areas.Admin.Models.ViewModels;
using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;
using System.Linq;

namespace SistemaPracticasAcademicas.Areas.Repository
{
    public class PracticasRepository
    {
        public PracticasRepository(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public sistema_academicoContext Context { get; }
        public bool Success = false;
        public List<string> Error;
        //RemoveDiacritics _remove = new();
        #region CRUD
        public void Agregar(PracticasViewModel _practicaVM)
        {
            Add(_practicaVM);
            Save();
        }
        public void Editar(Practica _practica)
        {
            Update(_practica);
            Save();
        }
        public void Eliminar(Practica _practica)
        {
            Delete(_practica);
            Save();
        }
        #endregion
        #region METHODS
        /// <summary>
        /// Método para validar los datos recibidos.
        /// </summary>
        public void Validar(PracticasViewModel _practicaVM)
        {
            Error = new List<string>();
            if (_practicaVM.Practica.IdAsignatura == 0)
                Error.Add("Seleccione una asignatura.");
            if (_practicaVM.Practica.NumUnidad == 0)
                Error.Add("Escriba el número de la unidad.");
            if (_practicaVM.Practica.NumUnidad <= 0)
                Error.Add("El número de la unidad debe ser mayor a 0.");
            if (string.IsNullOrWhiteSpace(_practicaVM.Practica.NombreUnidad))
                Error.Add("Escriba el nombre de la unidad.");
            if (string.IsNullOrWhiteSpace(_practicaVM.Practica.Tema))
                Error.Add("Escriba el tema de la práctica.");
            if (string.IsNullOrWhiteSpace(_practicaVM.Practica.Nombre))
                Error.Add("Escriba el nombre de la práctica.");
            if (Context.Practicas.Any(p => p.Nombre == _practicaVM.Practica.Nombre && p.Activo == 1 && p.Id != _practicaVM.Practica.Id))
                Error.Add("Ya existe una práctica con el mismo nombre");
            if (string.IsNullOrWhiteSpace(_practicaVM.Practica.Planteamiento))
                Error.Add("Escriba el planteamiento de la práctica.");
            if (string.IsNullOrWhiteSpace(_practicaVM.Practica.Objetivo))
                Error.Add("Escriba el objetivo de la práctica.");
            if (string.IsNullOrWhiteSpace(_practicaVM.Practica.Resultado))
                Error.Add("Escriba el resultado de la práctica.");
            if (Error.Count > 0)
                Success = false;
            else
                Success = true;
        }
        /// <summary>
        /// Devuelve una colección que incluye la relacion entre Practicas, Asignaturas y Docentes.
        /// </summary>
        public List<Practica> GetAllPracticasWithRelacion()
        {
            return Context.Practicas.Include(a => a.IdAsignaturaNavigation).Include(d => d.IdUsuarioNavigation).Where(p => p.Activo == 1).OrderBy(n => n.Nombre).ToList();
        }
        /// <summary>
        /// Devuelve una colección de las practicas inactivas.
        /// </summary>
        public List<Practica> GetAllPracticasInactivas()
        {
            return Context.Practicas.Include(a => a.IdAsignaturaNavigation).Include(d => d.IdUsuarioNavigation).OrderBy(n => n.Nombre).ToList();
        }
        /// <summary>
        /// Devuelve el primer valor dado el parametro.
        /// </summary>
        public Practica GetPracticaById(int _id)
        {
            return Context.Practicas.FirstOrDefault(p => p.Id == _id);
        }
        public Practica GetPracticaByidWithRelacion(int _id)
        {
            return Context.Practicas.Include(x => x.IdAsignaturaNavigation).Include(x=>x.IdUsuarioNavigation).FirstOrDefault(x=>x.Id == _id);
        }
        /// <summary>
        /// Método para filtrar las practicas por Título, Docente, Asignatura o Periodo.
        /// </summary>
        public IEnumerable<Practica> SearchPracticaByString(IEnumerable<Practica> _practica, string _buscar)
        {
            //string _palabra = _remove.QuitarAcentos(_buscar);
            string _palabra = _buscar.ToUpper();
            return _practica.Where(n => n.Tema.ToUpper().Contains(_palabra)
                || n.IdUsuarioNavigation.Nombre.ToUpper().Contains(_palabra)
                || n.IdAsignaturaNavigation.Nombre.ToUpper().Contains(_palabra)
                || n.Periodo.ToUpper().Contains(_palabra));
        }
        #endregion
        #region ENTITYFRAMEWORKCORE
        public void Add(PracticasViewModel _practicaVM)
        {
            Context.Add(_practicaVM.Practica);
        }
        public void Update(Practica _practica)
        {
            Context.Update(_practica);
        }
        public void Delete(Practica _practica)
        {
            Context.Remove(_practica);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        #endregion
    }
}
