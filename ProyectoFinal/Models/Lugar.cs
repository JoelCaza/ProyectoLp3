using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class Lugar
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CantidadSitios { get; set; }
        public string? ImagenNombre { get; set; }
        public byte[]? ImagenData { get; set; }
        public string? ImagenTipo { get; set; }

        // Nuevas propiedades para almacenar latitud y longitud
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
    }

}