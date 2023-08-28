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
    public class ContactoController : Controller
    {
        [Route("[action]")]
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<List<Object>>> IngresarContacto(Contacto parametros)
        {
            string cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_db"];
            XDocument? xmlParam = DBXmlMethods.GetXml(parametros);
            DataSet dataSet = await DBXmlMethods.EjecutaBase(NameStoredProcedure.sp_HoteleriaSet, cadenaConexion, "sp_Contacto", xmlParam.ToString());

            List<Object> Objeto = new List<Object>();
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    Object respuesta = new
                    {
                        Respuesta = row["Respuesta"].ToString(),
                        Legenda = row["Legenda"].ToString(),
                    };

                    Objeto.Add(respuesta);
                }
            }
            return Ok(Objeto);
        }
    }
}
