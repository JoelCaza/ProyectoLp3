using ProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Data;
using Microsoft.AspNetCore.Identity;


namespace ProyectoFinal.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<Usuario>();
                usuario.Contrasena = passwordHasher.HashPassword(usuario, usuario.Contrasena);

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                TempData["Mensaje"] = "Usuario registrado exitosamente.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Todos los campos son obligatorios.";
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Nombre, string Contrasena)
        {
            // Validar las credenciales
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Nombre == Nombre && u.Contrasena == Contrasena);

            if (usuario != null)
            {
                // Guardar el nombre de usuario en TempData para comprobar si el usuario está autenticado
                TempData["Usuario"] = usuario.Nombre; // Almacenar el nombre del usuario
                return RedirectToAction("Index", "Home"); // Redirigir al controlador Home (que es el inicio)
            }

            ViewBag.Error = "Credenciales incorrectas.";
            return View();
        }

    }
}
