using ProyectoFinal.Models; // Importa los modelos de datos, como 'Reservacion' y 'Lugar'.
using ProyectoFinal.Data; // Accede a la base de datos de la aplicación.
using Microsoft.AspNetCore.Mvc; // Contiene las clases necesarias para trabajar con el controlador y las vistas en ASP.NET Core MVC.
using Microsoft.EntityFrameworkCore; // Herramientas para trabajar con Entity Framework Core y consultas a la base de datos.

namespace ProyectoFinal.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor que inyecta el contexto de la base de datos.
        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción GET que muestra el formulario de reserva para un lugar específico.
        // El parámetro lugarId se pasa en la URL y se usa para buscar el lugar en la base de datos.
        [HttpGet]
        public IActionResult Reservar(int lugarId)
        {
            // Busca el lugar en la base de datos usando el lugarId.
            var lugar = _context.Lugares.FirstOrDefault(l => l.Id == lugarId);
            if (lugar == null)
            {
                // Si no se encuentra el lugar, devuelve un error 404.
                return NotFound();
            }

            // Pasa el lugar a la vista para mostrarlo en el formulario de reserva.
            ViewBag.Lugar = lugar;
            return View(); // Muestra la vista de reserva.
        }

        // Acción POST que maneja la creación de una reserva.
        // El parámetro lugarId se pasa desde el formulario y horaItinerario es el tiempo de la reserva.
        [HttpPost]
        public IActionResult Reservar(int lugarId, DateTime horaItinerario)
        {
            if (ModelState.IsValid)
            {
                // Simula la obtención del ID del usuario (en un escenario real, esto se obtendría del contexto del usuario autenticado).
                var usuarioId = 1;

                // Crea una nueva reserva con los datos proporcionados.
                var reservacion = new Reservacion
                {
                    UsuarioId = usuarioId,
                    LugarId = lugarId,
                    HoraItinerario = horaItinerario // Asigna el tiempo de la reserva.
                };

                // Agrega la nueva reserva a la base de datos.
                _context.Reservaciones.Add(reservacion);
                _context.SaveChanges(); // Guarda los cambios en la base de datos.

                // Muestra un mensaje temporal de éxito en la interfaz de usuario.
                TempData["Mensaje"] = "Reservación realizada con éxito.";
                // Redirige a la página principal (Home).
                return RedirectToAction("Index", "Home");
            }

            return View(); // Si el modelo no es válido, vuelve a la vista de reserva.
        }

        // Acción GET que muestra la lista de todas las reservas.
        public IActionResult Index()
        {
            // Obtiene todas las reservas de la base de datos e incluye los lugares relacionados.
            var reservas = _context.Reservaciones.Include(r => r.Lugar).ToList();
            // Devuelve la vista con la lista de reservas.
            return View(reservas);
        }
    }
}
