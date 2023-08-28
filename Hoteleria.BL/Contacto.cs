using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoteleria.BL
{
    public class Contacto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? Mensaje { get; set; }
    }
}
