using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models; // Asegúrate de incluir tu modelo Lugar
using ProyectoFinal.Data;  // Asegúrate de incluir tu DbContext

namespace ProyectoFinal.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Servicios()
        {
            var lugares = _context.Lugares.ToList(); // Obtén los datos de la base de datos

            // Asegúrate de que la autenticación esté configurada correctamente
            var isAuthenticated = User.Identity.IsAuthenticated;
            ViewBag.IsAuthenticated = isAuthenticated;

            return View(lugares); // Pasa los datos a la vista
        }
    }
}
