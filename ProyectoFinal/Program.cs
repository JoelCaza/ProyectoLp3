using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using ProyectoFinal.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de servicios de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configuraci�n del contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios para controladores con vistas
builder.Services.AddControllersWithViews();

// Configuraci�n para la carga de archivos
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 209715200; // 200 MB, ajusta seg�n tus necesidades
});

// Agregar Razor Pages si las usas en tu proyecto
builder.Services.AddRazorPages(); // Si no usas Razor Pages, puedes eliminar esta l�nea

// Agregar servicio para manejar el almacenamiento de archivos est�ticos
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configuraci�n del middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Manejo de errores espec�fico para desarrollo
}
else
{
    app.UseExceptionHandler("/Home/Error"); // P�gina de error personalizada en producci�n
    app.UseHsts(); // Seguridad adicional para HTTPS
}

app.UseHttpsRedirection(); // Redirigir autom�ticamente las solicitudes HTTP a HTTPS
app.UseStaticFiles(); // Habilitar archivos est�ticos (como im�genes, CSS, etc.)

// Configuraci�n de rutas
app.UseRouting();

// Middleware de autenticaci�n y autorizaci�n (si los usas)
app.UseAuthentication(); // Si tienes autenticaci�n configurada, aseg�rate de que est� aqu�
app.UseAuthorization(); // Si tienes autorizaci�n configurada, aseg�rate de que est� aqu�

// Configuraci�n de rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configuraci�n de rutas de Razor Pages (si las usas)
app.MapRazorPages(); // Si no usas Razor Pages, puedes eliminar esta l�nea

// Asegurarse de que la base de datos est� creada y las migraciones aplicadas
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
        logger.LogError(ex, "Ocurri� un error al aplicar las migraciones a la base de datos.");
    }
}

app.Run(); // Ejecutar la aplicaci�n
