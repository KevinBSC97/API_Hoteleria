using Hoteleria.BL;
using HoteleriaAPI.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HoteleriaAPI.Controllers
{
    [Route("api/[action]")]
    //[Authorize]
    public class HabitacionHotelController : Controller
    {
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<List<HabitacionHotel>>> MostrarHabitacionesHotel()
        {
            string cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_db"];
            DataSet dataSet = await DBXmlMethods.EjecutaBase(NameStoredProcedure.sp_HoteleriaGet, cadenaConexion, "sp_HabitacionHotel", "");

            List<HabitacionHotel> listaHabitacionHotel = new List<HabitacionHotel>();
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    HabitacionHotel habitacionhotel = new HabitacionHotel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Calificacion = Convert.ToSingle(row["calificacion"]),
                        NombreHabitacion = row["nombre_habitacion"].ToString(),
                        Descripcion = row["descripcion"].ToString(),
                        Precio = Convert.ToDouble(row["precio"]),
                        Imagen = row["imagen"].ToString()
                    };

                    listaHabitacionHotel.Add(habitacionhotel);
                }
            }
            return Ok(listaHabitacionHotel);
        }
    }
}
