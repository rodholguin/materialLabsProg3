using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSoftDBManager
{
    public class DBManager
    {
        private static DBManager dBManager = new DBManager();
        private string URL = "server=" +
            "labs-1inf30-prog3-20242.cnjqylm6u7zo.us-east-1.rds.amazonaws.com;";
        private string user = "user=" +
            "admin;";
        private string password = "password=" +
            "prog320242labs;";
        private string puerto = "port=" +
            "3306;";
        private string database = "database=" +
            "labPractica;";
        private MySqlConnection con;

        private DBManager()
        {
            String cadena = URL + user + password + puerto + database;
            con = new MySqlConnection(cadena);
        }
        public static DBManager Instance 
        {
            get{
                return dBManager;
            }
        }
        

        public MySqlConnection Connection
        {
            get { 
                return con;
            }
        }

    }
}
