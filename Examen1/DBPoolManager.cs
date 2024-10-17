using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResearchSoftPUCPDBManager
{
    public class DBPoolManager
    {
        private static DBPoolManager dbPoolManager = new DBPoolManager();
        private string URL = "server=labs-1inf30-prog3-20242.cnjqylm6u7zo.us-east-1.rds.amazonaws.com;";
        private string usuario = "user=admin;";
        private string password = "password=prog320242labs;";
        private string puerto = "port=3306;";
        private string database = "database=lab04;";

        private MySqlConnection con;
        private MySqlDataReader lector = null;

        private DBPoolManager()
        {
            string cadena = URL + usuario + password + puerto + database;
            con = new MySqlConnection(cadena);
        }

        public static DBPoolManager Instance
        {
            get
            {
                return dbPoolManager;
            }
        }

        public MySqlConnection Connection
        {
            get
            {
                AbrirConexion();
                return con;
            }
        }

        //Método para abrir la conexión
        public void AbrirConexion()
        {
            if (con.State != ConnectionState.Open)
                con.Open();
        }

        //Método para cerrar la conexión
        public void CerrarConexion()
        {
            if (lector!=null) if (!lector.IsClosed) lector.Close();
            if (con.State != ConnectionState.Closed)
                con.Close();
        }

        //Método para ejecutar un procedimiento almacenado insert/update/delete
        public int EjecutarProcedimiento(string nombreProcedimiento, MySqlParameter[] parameters, string nombreParametroSalida)
        {
            int resultado = 0;
            try
            {
                AbrirConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandText = nombreProcedimiento;
                comando.CommandType = CommandType.StoredProcedure;

                // Añade los parámetros de entrada si existen
                if (parameters != null)
                    comando.Parameters.AddRange(parameters);
                if (nombreParametroSalida!=null)
                    comando.Parameters.Add(nombreParametroSalida, MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output; ;
                // Ejecuta el procedimiento
                resultado = comando.ExecuteNonQuery();

                // Si existe parámetro de salida, obtenemos el valor en el resultado
                if (nombreParametroSalida != null)
                    resultado = Convert.ToInt32(comando.Parameters[nombreParametroSalida].Value);
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CerrarConexion();
            }
            return resultado;
        }

        //Método para ejecutar un procedimiento almacenado select
        public MySqlDataReader EjecutarProcedimientoLectura(string nombreProcedimiento, MySqlParameter[] parameters)
        {
            try
            {
                AbrirConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandText = nombreProcedimiento;
                comando.CommandType = CommandType.StoredProcedure;

                // Añade los parámetros de entrada si existen
                if (parameters != null)
                    comando.Parameters.AddRange(parameters);

                lector = comando.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lector;
        }
    }
}
