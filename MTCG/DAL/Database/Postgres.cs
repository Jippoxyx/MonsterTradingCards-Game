using Npgsql;
using System;

namespace MTCG.DAL.Database
{
    class Postgres : IDisposable
    {
        public NpgsqlConnection conn;       
        
        public NpgsqlConnection CreateConnection()
        {           
            conn = new NpgsqlConnection("Host = localhost; Username = postgres; " +
                "Password = 0000; Database = mtcg; Port = 5432");
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
        
        public void Dispose()
        {
            conn.Close();
        }
    }
}

