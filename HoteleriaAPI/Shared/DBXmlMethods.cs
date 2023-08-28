using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HoteleriaAPI.Shared
{
    public class DBXmlMethods
    {
        /// <summary>
        /// Convertir un modelo a XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterio"></param>
        /// <returns></returns>
        public static XDocument? GetXml<T>(T criterio)
        {
            XDocument resultado = new XDocument(new XDeclaration("1.0", "utf-8", "true"));
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using XmlWriter xw = resultado.CreateWriter();
                xs.Serialize(xw, criterio);
                xw.Close();
                return resultado;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static async Task<DataSet> EjecutaBase(string sp, string conexion, string transaccion, string dataXML)
        {
            DataSet dsResultado = new DataSet();
            SqlConnection cnn = new SqlConnection(conexion);

            try
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter adt = new SqlDataAdapter();

                cmd.CommandText = sp;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = cnn;
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add("@iTransaction", SqlDbType.VarChar).Value = transaccion;
                cmd.Parameters.Add("@iXML", SqlDbType.Xml).Value = dataXML.ToString();

                await cnn.OpenAsync().ConfigureAwait(false);
                adt = new SqlDataAdapter(cmd);
                adt.Fill(dsResultado);
                cmd.Dispose();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                Console.Write("Logs", "EjecutaBase", ex.ToString());
                cnn.Close();
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }

            }

            return dsResultado;
        }
    }
}