using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;

namespace SistemaPracticasAcademicas.Areas.Admin.Repository
{
    public class ConfiguracionRepository
    {
        public ConfiguracionRepository(sistema_academicoContext _context)
        {
            Context = _context;
        }
        public List<string> Error;
        public bool Success = false;
        public sistema_academicoContext Context { get; }
        public void ActualizarContraseña(Usuario _docente)
        {
            Update(_docente);
        }
        /// <summary>
        /// Metodo para validar la nueva contraseña a cambiar.
        /// </summary>
        public void ValidarContraseña(string _pass, string _pass2)
        {
            Error = new();
            if (string.IsNullOrWhiteSpace(_pass) || string.IsNullOrWhiteSpace(_pass2))
                Error.Add("Escriba la nueva contraseña.");
            if (_pass != _pass2)
                Error.Add("Las contraseñas deben coincidir.");
            if (Error.Count > 0)
                Success = false;
            else
                Success = true;
        }
        public void Update(Usuario _docente)
        {
            Context.Update(_docente);
            Save();
        }
        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
