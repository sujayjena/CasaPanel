#nullable disable
using Dapper;
using CasaAPI.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Xml;
using System.Data.Common;

namespace CasaAPI.Repositories
{
    public class BaseRepository : IBaseRepository, IDisposable
    {
        private readonly IConfiguration _configuration;
        private static string _connectionString;

        #region Configuration Methods
        public IConfigurationSection GetConfigurationSection(string key)
        {
            return _configuration.GetSection(key);
        }

        private string GetConnectionString(string connectionName)
        {
            return _configuration.GetConnectionString(connectionName);
        }

        private static SqlConnection OpenConnection()
        {
            SqlConnection connection;
            connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        #endregion

        protected BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = GetConnectionString("DefaultConnStr");
        }

        #region Query Methods
        public static async Task<T> SaveByStoredProcedure<T>(string storedProcedureName)
        {
            try
            {
                using (SqlConnection con = OpenConnection())
                {
                    return await (Task<T>)Convert.ChangeType(con.ExecuteScalarAsync(storedProcedureName, null, null, null, CommandType.StoredProcedure), typeof(T));
                }
            }
            catch
            {
                throw;
            }
        }

        public static async Task<T> SaveByStoredProcedure<T>(string storedProcedureName, object parameters)
        {
            try
            {
                using (SqlConnection con = OpenConnection())
                {
                    return await con.ExecuteScalarAsync<T>(storedProcedureName, parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch// (Exception ex)
            {
                throw;// ex;
            }
        }

        public static async Task<IEnumerable<T>> ListByStoredProcedure<T>(string storedProcedureName)
        {
            try
            {
                using (SqlConnection conn = OpenConnection())
                {
                    //return await (Task<IEnumerable<T>>)Convert.ChangeType(conn.QueryAsync<T>(storedProcedureName, null, null, null, CommandType.StoredProcedure), typeof(IEnumerable<T>));
                    return await conn.QueryAsync<T>(storedProcedureName, null, null, null, CommandType.StoredProcedure);
                }
            }
            catch// (Exception ex)
            {
                throw;// ex;
            }
        }

        public static async Task<IEnumerable<T>> ListByStoredProcedure<T>(string storedProcedureName, object parameters)
        {
            try
            {
                using (SqlConnection conn = OpenConnection())
                {
                    return await conn.QueryAsync<T>(storedProcedureName, parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }
        }

       

        public static async Task<IEnumerable<T>> ListByQuery<T>(string query)
        {
            try
            {
                using (SqlConnection con = OpenConnection())
                {
                    return await con.QueryAsync<T>(query, null, null, null, CommandType.Text);
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        public static async Task<IEnumerable<T>> ListByQuery<T>(string query, object parameters)
        {
            try
            {
                using (SqlConnection con = OpenConnection())
                {
                    return await (Task<IEnumerable<T>>)Convert.ChangeType(con.QueryAsync<T>(query, parameters, null, null, CommandType.Text), typeof(IEnumerable<T>));
                }
            }
            catch// (Exception ex)
            {
                throw;
            }
        }

        public static async Task<int> ExecuteNonQuery(string procedureName)
        {
            try
            {
                using (SqlConnection conn = OpenConnection())
                {
                    return await conn.ExecuteAsync(procedureName, null, null, null, CommandType.StoredProcedure);
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        public static async Task<int> ExecuteNonQuery(string procedureName, object parameters)
        {
            try
            {
                using (SqlConnection conn = OpenConnection())
                {
                    return await conn.ExecuteAsync(procedureName, parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch// (Exception ex)
            {
                throw;
            }
        }
        #endregion

        public static string ConvertListToXml<T>(List<T> dataList)
        {
            XmlSerializer serializer;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            //using (StringWriter writer = new StringWriter())
            //{
            //    serializer = new XmlSerializer(typeof(List<T>));
            //    serializer.Serialize(writer, dataList, ns);
            //    return writer.ToString();
            //}

            using (var stream = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer = new XmlSerializer(typeof(List<T>));
                    serializer.Serialize(writer, dataList, ns);
                    return stream.ToString();
                }
            }
        }

        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public LinkedList<Dictionary<string, string>> GETDETAILS(string procedurename, SqlParameter[] _param)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = OpenConnection())
            {
                  SqlCommand comm = conn.CreateCommand();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = procedurename;
                    comm.Parameters.AddRange(_param);
                    try
                    {
                        using (SqlDataAdapter sqldataAdpt = new SqlDataAdapter(comm))
                        {
                            sqldataAdpt.Fill(dt);
                        }

                        comm.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                    }
               }

            LinkedList<Dictionary<string, string>> llist = new LinkedList<Dictionary<string, string>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                    dict.Add(dt.Columns[i].ColumnName, row[i].ToString());
                llist.AddLast(dict);
            }
            return llist;
        }
       
        public void Dispose()
        {

        }
    }
}
