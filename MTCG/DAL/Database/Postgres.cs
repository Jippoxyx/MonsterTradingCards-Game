using Npgsql;
using System;

namespace MTCG.DAL.Database
{
    class Postgres
    {
        public NpgsqlConnection conn;
        private string connString = "Host = localhost; Username = postgres; Password = 0000; Database = mtcg; Port = 5432"; 
        
        public NpgsqlConnection CreateConnection()
        {           
            conn = new NpgsqlConnection(connString);
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {
               Console.WriteLine("Success open postgreSQL connection.");
            }
            else
            {
               Console.WriteLine("Connection failed");
               return null;
            }
        
            return conn;
        } 
        
        public NpgsqlConnection GetConnection()
        {
            return CreateConnection();
        }
    }
}

