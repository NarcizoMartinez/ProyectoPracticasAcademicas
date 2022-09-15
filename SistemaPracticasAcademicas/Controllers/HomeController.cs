using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPracticasAcademicas.Helpers;
using SistemaPracticasAcademicas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SistemaPracticasAcademicas.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(sistema_academicoContext _context, IWebHostEnvironment _environment)
        {
            Context = _context;
            Environment = _environment;
        }

        public sistema_academicoContext Context { get; }
        public IWebHostEnvironment Environment { get; }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string _user, string _password)
        {
            var _entity = Context.Usuarios.Include(r => r.IdRolNavigation).SingleOrDefault(u => u.NumeroControl == _user && u.Contrasena == _password);
            if (string.IsNullOrWhiteSpace(_user))
                ModelState.AddModelError("", "Escriba el número de control.");
            if (string.IsNullOrWhiteSpace(_password))
                ModelState.AddModelError("", "Escriba la contraseña.");
            if (!ModelState.IsValid)
                return View();
            else
            {
                if (_entity != null)
                {
                    List<Claim> _claims = new();
                    _claims.Add(new Claim(ClaimTypes.NameIdentifier, _entity.Id.ToString()));
                    _claims.Add(new Claim(ClaimTypes.Role, _entity.IdRolNavigation.Rol));
                    _claims.Add(new Claim(ClaimTypes.Name, _entity.Nombre));
                    var identidad = new ClaimsIdentity(_claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(new ClaimsPrincipal(identidad));
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                    ModelState.AddModelError("", "El usuario ó contraseña son incorrectos.");
            }
            return View();
        }
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync();
            return View("Index", "Home");
        }
        public IActionResult RecuperarContrasena()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecuperarContrasena(string _email)
        {
            var _entity = Context.Usuarios.SingleOrDefault(e => e.CorreoElectronico == _email);
            if (_entity != null)
            {
                RandomPasswordGenerator _random = new();
                MailMessage _mensaje = new();
                _mensaje.From = new MailAddress("","ITESRC"); //email
                _mensaje.To.Add(_email);
                _mensaje.Subject = "Restablecer contraseña Sistema Prácticas Academicas";
                string _texto = System.IO.File.ReadAllText(Environment.WebRootPath + "/PasswordRecovery.html");
                string _nuevaContrasena = _random.GetRandomPassword(7);
                _mensaje.Body = _texto.Replace("{##defaultPass##}", _nuevaContrasena);
                _mensaje.IsBodyHtml = true;
                SmtpClient _cliente = new("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("", "") //email 'n' password
                };
                _cliente.Send(_mensaje);
                _entity.Contrasena = _nuevaContrasena;
                Context.Update(_entity);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                if (_email == null)
                    ModelState.AddModelError("", "Escriba su correo institucional.");
                else
                    ModelState.AddModelError("", "El correo institucional no es válido.");
                return View();
            }    
        }
        [Route("Acceso-Denegado")]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
