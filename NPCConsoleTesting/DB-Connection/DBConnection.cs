using Dapper;
using Microsoft.Data.SqlClient;
using NPCConsoleTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting.DB_Connection
{
    public class DBConnection
    {
        public static IEnumerable<CharacterModel> QueryDB(string connectionString, string query)
        {
            IEnumerable<CharacterModel> result;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                result = connection.Query<CharacterModel>(query);
            }

            return result;
        }
    }
}
