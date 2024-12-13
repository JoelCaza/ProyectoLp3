using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using System.Collections.Generic;

namespace ProyectoFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }
    }
}
