using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoteleria.BL
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contraseña { get; set; }

        public string Transaccion { get; set; }
    }
}
