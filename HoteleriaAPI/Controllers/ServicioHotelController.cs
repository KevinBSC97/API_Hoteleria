using Hoteleria.BL;
using HoteleriaAPI.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace HoteleriaAPI.Controllers
{
    [Route("api/[action]")]
    //[Authorize]
    public class ServicioHotelController : Controller
    {

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<List<ServicioHotel>>> MostrarServiciosHotel()
        {
            string cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_db"];
            DataSet dataSet = await DBXmlMethods.EjecutaBase(NameStoredProcedure.sp_HoteleriaGet, cadenaConexion, "sp_ServicioHotel", "");

            List<ServicioHotel> listaServiciosHotel = new List<ServicioHotel>();
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    ServicioHotel servicioHotel = new ServicioHotel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Calificacion = Convert.ToSingle(row["calificacion"]),
                        Descripcion = row["descripcion"].ToString(),
                        Precio = Convert.ToDouble(row["precio"]),
                        Imagen = row["imagen"].ToString()
                    };

                    listaServiciosHotel.Add(servicioHotel);
                }
            }
            return Ok(listaServiciosHotel);
        }
    }
}
