using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using NPCConsoleTesting.Models;

namespace NPCConsoleTesting.DB_Connection
{
    public class Helper
    {
        private readonly IConfiguration _config;
        public Helper(IConfiguration config)
        {
            _config = config;
        }

        //DBConnection();

        public static void DBConnection()
        {
            
            //var connectionString = _config.GetValue;

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    string sql = @"SELECT * FROM npcs WHERE id = 58";

            //    var query = connection.Query<CharacterModel>(sql);
            //    Console.WriteLine("test");
            //    Console.ReadLine();
            //}
        }
    }
}
