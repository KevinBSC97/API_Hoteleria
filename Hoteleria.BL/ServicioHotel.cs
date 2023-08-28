using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoteleria.BL
{
    public class ServicioHotel
    {
        public int Id { get; set; }
        public float? Calificacion { get; set; }
        public string? Descripcion { get; set; }
        public double? Precio { get; set; }
        public string? Imagen { get; set; }
    }
}
