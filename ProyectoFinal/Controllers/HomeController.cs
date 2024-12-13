// Importación de los namespaces necesarios para trabajar con ASP.NET Core MVC y Entity Framework.
using Microsoft.AspNetCore.Mvc; // Proporciona las clases necesarias para el controlador y la vista.
using Microsoft.EntityFrameworkCore; // Contiene las clases y métodos para trabajar con Entity Framework Core.
using ProyectoFinal.Data; // Acceso al contexto de la base de datos de la aplicación.
using ProyectoFinal.Models; // Contiene los modelos de datos utilizados en la aplicación.
using System.Diagnostics; // Espacio de nombres para herramientas de depuración (no se utiliza en este caso, pero es importado por defecto).

namespace ProyectoFinal.Controllers
{
    // Define el controlador 'HomeController' que maneja las solicitudes de la página de inicio.
    public class HomeController : Controller
    {
        // Declaración del contexto de la base de datos que se inyecta en el controlador.
        private readonly ApplicationDbContext _context;

        // Constructor del controlador que permite la inyección de dependencias para el contexto de la base de datos.
        // Esto permite acceder a la base de datos a través de Entity Framework.
        public HomeController(ApplicationDbContext context)
        {
            // Si el contexto es nulo, lanza una excepción de argumento nulo, para asegurarse de que se proporciona el contexto correctamente.
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Acción que maneja las solicitudes a la página principal (Index).
        public IActionResult Index()
        {
            // Consulta a la base de datos para obtener una lista de lugares.
            var lugares = _context.Lugares.ToList();

            // Devuelve la vista 'Index' y pasa la lista de lugares como modelo a la vista.
            return View(lugares);
        }
    }
}
