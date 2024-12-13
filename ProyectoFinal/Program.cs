using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using ProyectoFinal.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configuración del contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios para controladores con vistas
builder.Services.AddControllersWithViews();

// Configuración para la carga de archivos
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 209715200; // 200 MB, ajusta según tus necesidades
});

// Agregar Razor Pages si las usas en tu proyecto
builder.Services.AddRazorPages(); // Si no usas Razor Pages, puedes eliminar esta línea

// Agregar servicio para manejar el almacenamiento de archivos estáticos
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configuración del middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Manejo de errores específico para desarrollo
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Página de error personalizada en producción
    app.UseHsts(); // Seguridad adicional para HTTPS
}

app.UseHttpsRedirection(); // Redirigir automáticamente las solicitudes HTTP a HTTPS
app.UseStaticFiles(); // Habilitar archivos estáticos (como imágenes, CSS, etc.)

// Configuración de rutas
app.UseRouting();

// Middleware de autenticación y autorización (si los usas)
app.UseAuthentication(); // Si tienes autenticación configurada, asegúrate de que está aquí
app.UseAuthorization(); // Si tienes autorización configurada, asegúrate de que está aquí

// Configuración de rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configuración de rutas de Razor Pages (si las usas)
app.MapRazorPages(); // Si no usas Razor Pages, puedes eliminar esta línea

// Asegurarse de que la base de datos esté creada y las migraciones aplicadas
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Aplicar migraciones pendientes
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones a la base de datos.");
    }
}

app.Run(); // Ejecutar la aplicación
