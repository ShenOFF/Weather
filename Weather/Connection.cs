using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Weather
{
    public class Connection
    {
        public static DataTable Select(string selectSQL)
        {
            DataTable dataTable = new DataTable("DataBase");

            MySqlConnection sqlConnection = new MySqlConnection("server=localhost;port=3307;database=Weather;uid=root;pwd=;");
            sqlConnection.Open();
            MySqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;

        }
    }
}
