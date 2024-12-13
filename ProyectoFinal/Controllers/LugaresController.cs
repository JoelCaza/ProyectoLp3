// Importación de los namespaces necesarios para trabajar con ASP.NET Core MVC, Entity Framework y manejo de archivos.
using Microsoft.AspNetCore.Mvc; // Contiene las clases necesarias para trabajar con el controlador y las vistas en ASP.NET Core MVC.
using ProyectoFinal.Data; // Accede a la base de datos de la aplicación.
using ProyectoFinal.Models; // Contiene los modelos de datos utilizados en la aplicación, como el modelo 'Lugar'.
using Microsoft.EntityFrameworkCore; // Herramientas para trabajar con Entity Framework Core y consultas a la base de datos.
using System.IO; // Necesario para trabajar con flujos de memoria, especialmente para manejar imágenes.

namespace ProyectoFinal.Controllers
{
    public class LugaresController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor que inyecta el contexto de la base de datos.
        public LugaresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción GET que muestra la lista de lugares.
        public IActionResult Index()
        {
            // Recupera todos los lugares desde la base de datos.
            var lugares = _context.Lugares.ToList();
            return View(lugares); // Devuelve la vista 'Index' con la lista de lugares.
        }

        // Acción GET para mostrar el formulario de creación de un nuevo lugar.
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Devuelve la vista de creación de lugar.
        }

        // Acción POST que maneja la creación de un nuevo lugar.
        [HttpPost]
        public async Task<IActionResult> Create(Lugar lugar, IFormFile Imagen, double? Latitud, double? Longitud)
        {
            if (ModelState.IsValid)
            {
                // Procesa la imagen si se ha subido una.
                if (Imagen != null && Imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Imagen.CopyToAsync(memoryStream); // Copia la imagen a un flujo de memoria.

                        lugar.ImagenData = memoryStream.ToArray(); // Guarda los datos de la imagen en el modelo.
                        lugar.ImagenNombre = Imagen.FileName; // Guarda el nombre de la imagen.
                        lugar.ImagenTipo = Imagen.ContentType; // Guarda el tipo de la imagen.
                    }
                }

                // Asigna las coordenadas del mapa al modelo.
                lugar.Latitud = Latitud;
                lugar.Longitud = Longitud;

                // Guarda el nuevo lugar en la base de datos.
                _context.Lugares.Add(lugar);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Lugar creado exitosamente."; // Mensaje temporal de éxito.
                return RedirectToAction("Index", "Home"); // Redirige a la lista de lugares.
            }

            return View(lugar); // Si el modelo no es válido, vuelve a la vista de creación con errores.
        }

        // Acción GET que muestra los detalles de un lugar específico.
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Si no se pasa un ID, muestra un error 404.
            }

            var lugar = _context.Lugares
                .FirstOrDefault(m => m.Id == id); // Busca el lugar por ID.

            if (lugar == null)
            {
                return NotFound(); // Si no se encuentra el lugar, muestra un error 404.
            }

            return View(lugar); // Devuelve la vista de detalles con el lugar encontrado.
        }

        // Acción GET que muestra el formulario para editar un lugar.
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Si no se pasa un ID, muestra un error 404.
            }

            var lugar = _context.Lugares.Find(id); // Busca el lugar por ID.

            if (lugar == null)
            {
                return NotFound(); // Si no se encuentra el lugar, muestra un error 404.
            }

            return View(lugar); // Devuelve la vista de edición con el lugar encontrado.
        }

        // Acción POST que maneja la edición de un lugar existente.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Lugar lugar, IFormFile Imagen)
        {
            if (id != lugar.Id)
            {
                return NotFound(); // Si el ID del lugar no coincide, muestra un error 404.
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Procesa la imagen si se ha subido una nueva.
                    if (Imagen != null && Imagen.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Imagen.CopyToAsync(memoryStream); // Copia la imagen a un flujo de memoria.

                            lugar.ImagenData = memoryStream.ToArray(); // Guarda los datos de la imagen en el modelo.
                            lugar.ImagenNombre = Imagen.FileName; // Guarda el nombre de la imagen.
                            lugar.ImagenTipo = Imagen.ContentType; // Guarda el tipo de la imagen.
                        }
                    }
                    else
                    {
                        // Mantiene la imagen existente si no se sube una nueva.
                        var existingLugar = await _context.Lugares.AsNoTracking()
                            .FirstOrDefaultAsync(l => l.Id == id);

                        if (existingLugar != null)
                        {
                            lugar.ImagenData = existingLugar.ImagenData;
                            lugar.ImagenNombre = existingLugar.ImagenNombre;
                            lugar.ImagenTipo = existingLugar.ImagenTipo;
                        }
                    }

                    _context.Update(lugar); // Actualiza el lugar en la base de datos.
                    await _context.SaveChangesAsync(); // Guarda los cambios.

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LugarExists(lugar.Id)) // Verifica si el lugar existe en la base de datos.
                    {
                        return NotFound(); // Si no existe, muestra un error 404.
                    }
                    else
                    {
                        throw; // Si ocurre otro error, vuelve a lanzar la excepción.
                    }
                }

                return RedirectToAction(nameof(Index)); // Redirige a la lista de lugares.
            }
            return View(lugar); // Si el modelo no es válido, vuelve a la vista de edición con errores.
        }

        // Acción GET para mostrar el formulario de eliminación de un lugar.
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Si no se pasa un ID, muestra un error 404.
            }

            var lugar = _context.Lugares
                .FirstOrDefault(m => m.Id == id); // Busca el lugar por ID.

            if (lugar == null)
            {
                return NotFound(); // Si no se encuentra el lugar, muestra un error 404.
            }

            return View(lugar); // Devuelve la vista de eliminación con el lugar encontrado.
        }

        // Acción POST que maneja la eliminación de un lugar.
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lugar = await _context.Lugares.FindAsync(id); // Busca el lugar por ID.

            if (lugar != null)
            {
                _context.Lugares.Remove(lugar); // Elimina el lugar de la base de datos.
                await _context.SaveChangesAsync(); // Guarda los cambios.
            }

            return RedirectToAction(nameof(Index)); // Redirige a la lista de lugares.
        }

        // Método auxiliar para verificar si un lugar existe en la base de datos.
        private bool LugarExists(int id)
        {
            return _context.Lugares.Any(e => e.Id == id); // Verifica si existe un lugar con el ID proporcionado.
        }

        // Acción para mostrar la imagen asociada con un lugar.
        public IActionResult MostrarImagen(int id)
        {
            var lugar = _context.Lugares.Find(id); // Busca el lugar por ID.

            if (lugar == null || lugar.ImagenData == null)
            {
                return NotFound(); // Si el lugar o la imagen no existe, muestra un error 404.
            }

            return File(lugar.ImagenData, lugar.ImagenTipo); // Devuelve la imagen como un archivo.
        }
    }
}
