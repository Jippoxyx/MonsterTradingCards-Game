using MTCG.DAL.Database;
using MTCG.DAL.Encryption;
using MTCG.Model;
using Npgsql;
using System;

namespace MTCG.DAL.Access
{
    class UserAccess
    {
        Postgres db = new Postgres();
        EncryptDecrypt crypt = new EncryptDecrypt();
       
        //Registration
        public void CreateUser(UserModel user)
        {
           using (NpgsqlCommand command = db.GetConnection().CreateCommand())
           {
              command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
             
              string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                //bool verified = BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);

                Console.WriteLine(passwordHash);

              command.Parameters.AddWithValue("username", user.Username);
              command.Parameters.AddWithValue("password", passwordHash); 

              command.Prepare();

              command.ExecuteNonQuery();
           }             
        }

        
        
    }
}
