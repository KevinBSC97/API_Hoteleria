using HoteleriaAPI.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;
using Hoteleria.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Configuration;
using NuGet.Common;
using System.Net;

namespace HoteleriaAPI.Controllers
{
    [Route("api/[action]")]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration Configuration;
        public UsuarioController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> Login([FromBody]Usuario usuarios)
        {
            try
            {
                var cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_db"];
                XDocument? xmlParam = DBXmlMethods.GetXml(usuarios);
                DataSet dataSet = await DBXmlMethods.EjecutaBase(NameStoredProcedure.sp_HoteleriaGet, cadenaConexion, "sp_Login", xmlParam.ToString());
                List<Usuario> listData = new List<Usuario>();
                if(dataSet.Tables.Count > 0)
                {
                    try
                    {
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            Usuario usertmp = new Usuario();
                            usertmp.Id = Convert.ToInt32(dataSet.Tables[0].Rows[0]["id"]);
                            usertmp.NombreUsuario = dataSet.Tables[0].Rows[0]["nombre_usuario"].ToString();
                            return Ok(
                                 new HoteleriaRespuesta<dynamic>
                                 {
                                     StatusCode = HttpStatusCode.OK,
                                     Message = "Inicio de sesion exitoso",
                                     Data = usertmp.NombreUsuario,
                                     Token = JsonConvert.SerializeObject(CrearToken(usertmp))
                                 }
                            );
                        }
                        else
                        {
                            BadRequest(
                                 new HoteleriaRespuesta<dynamic>
                                 {
                                     StatusCode = HttpStatusCode.OK,
                                     Message = "Error en las credenciales de acceso",
                                 });
                           
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.Write("error" + ex.Message);
                    }
                }
                return Ok();
            }
            catch(Exception ex)
            {
                Console.Write("error" + ex.Message);
                return StatusCode(500);
            }
        }

        private string CrearToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
       
    }
}
