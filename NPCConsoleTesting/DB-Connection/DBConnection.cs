using Dapper;
using Microsoft.Data.SqlClient;
using NPCConsoleTesting.Models;
using System.Collections.Generic;

namespace NPCConsoleTesting.DB_Connection
{
    public class DBConnection
    {
        public static List<CharacterModel> QueryDB(string connectionString, string query)
        {
            List<CharacterModel> listResult = new();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Query<CharacterModel>(query);

                foreach (var x in result)
                {
                    listResult.Add(x);
                }
            }

            return listResult;
        }
    }
}
