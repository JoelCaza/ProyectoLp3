using System;

namespace ProyectoFinal.Models
{
    public class Reservacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LugarId { get; set; }
        public DateTime HoraItinerario { get; set; }  // Cambiado a DateTime

        public Usuario Usuario { get; set; }
        public Lugar Lugar { get; set; }
    }
}
