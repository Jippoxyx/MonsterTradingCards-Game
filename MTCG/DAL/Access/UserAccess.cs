using MTCG.DAL.Database;
using MTCG.Handlers;
using MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Access
{
    class UserAccess
    {
        Postgres db = new Postgres();
       
        public void InsertUser(UserModel user)
        {
           using (NpgsqlCommand command = db.GetConnection().CreateCommand())
           {
                    command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";

                    command.Parameters.AddWithValue("username", user.Username);
                    command.Parameters.AddWithValue("password", user.Password); //Pw verschlüsseln

                    command.Prepare();

                    command.ExecuteNonQuery();
           }             
        }
    }
}
