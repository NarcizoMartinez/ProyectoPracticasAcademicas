using SistemaPracticasAcademicas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SistemaPracticasAcademicas.Areas.Repository
{
    public class AsignaturasRepository
    {
        public AsignaturasRepository(sistema_academicoContext context)
        {
            Context = context;
        }
        public bool Success = false;
        public List<string> Error;
        public sistema_academicoContext Context { get; }
        #region CRUD
        public void Agregar(Asignatura _asignatura)
        {
            Add(_asignatura);
            Save();
        }
        public void Editar(Asignatura _asignatura)
        {
            Update(_asignatura);
            Save();
        }
        public void Eliminar(Asignatura _asignatura)
        {
            Remove(_asignatura);
            Save();
        }
        #endregion
        #region METHODS
        public void Validar(Asignatura _asignatura)
        {
            Error = new List<string>();
            if (string.IsNullOrWhiteSpace(_asignatura.Clave))
                Error.Add("Escriba la clave de la asignatura.");
            else
            {
                if (!Regex.Match(_asignatura.Clave, @"^[A-Z]{3}[0-9]{4}$").Success ||
                    !Regex.Match(_asignatura.Clave, @"^[0-9]{2}[A-Z]{3}[0-9]{1}$").Success)
                    Error.Add("El formato de clave no es valido.");
                if (Context.Asignaturas.Any(a => a.Clave == _asignatura.Clave && a.Id != _asignatura.Id))
                    Error.Add($"Ya existe una asignatura con la clave: {_asignatura.Clave}");
            }
            if (string.IsNullOrWhiteSpace(_asignatura.Nombre))
                Error.Add("Escriba el nombre de la asignatura.");
            if (Error.Count == 0)
                Success = true;
            else
                Success = false;
        }
        public List<Asignatura> GetAsignaturas()
        {
            return Context.Asignaturas.OrderBy(a => a.Nombre).ToList();
        }
        public Asignatura GetAsignaturaById(int _id)
        {
            return Context.Asignaturas.FirstOrDefault(a => a.Id == _id);
        }
        #endregion
        #region ENTITYFRAMEWORKCORE
        public void Add(Asignatura _asignatura)
        {
            Context.Add(_asignatura);
        }
        public void Update(Asignatura _asignatura)
        {
            Context.Update(_asignatura);
        }
        public void Remove(Asignatura _asignatura)
        {
            Context.Remove(_asignatura);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        #endregion
    }
}
