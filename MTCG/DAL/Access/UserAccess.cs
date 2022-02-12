using MTCG.DAL.Database;
using MTCG.Model;
using Npgsql;
using System;

namespace MTCG.DAL.Access
{
    class UserAccess
    {
        Postgres db = new Postgres();
        

        //Registration
        public void CreateUser(UserModel user)
        {
            using (NpgsqlCommand command = db.GetConnection().CreateCommand())
            {
                command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
                command.Parameters.AddWithValue("username", user.Username);
                command.Parameters.AddWithValue("password", BCrypt.Net.BCrypt.HashPassword(user.Password));
           
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public UserModel GetUserByName(string username)
        {
            UserModel user = new UserModel();
            try
            {
                
                using (NpgsqlCommand command = db.GetConnection().CreateCommand())
                {
                    command.CommandText = "SELECT userid, username,password FROM users WHERE username = @username";
                    command.Parameters.AddWithValue($"@username", username);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    user.UserID = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(2);
                }
            }
            catch(NullReferenceException)
            {
               return null;
            }                      
            catch(InvalidOperationException)
            {
                return null;
            }
            return user;
        }
    }
}
