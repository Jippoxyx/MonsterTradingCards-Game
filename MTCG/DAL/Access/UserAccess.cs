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
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "INSERT INTO users (username, password, coins) VALUES (@username, @password, @coins)";
                command.Parameters.AddWithValue("username", user.Username);
                command.Parameters.AddWithValue("password", BCrypt.Net.BCrypt.HashPassword(user.Password));
                command.Parameters.AddWithValue("coins", 20);

                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public UserModel GetUserByName(string username)
        {
            UserModel user = new UserModel();
            try
            {              
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {
                    command.CommandText = "SELECT userid, username, password, coins FROM users WHERE username = @username";
                    command.Parameters.AddWithValue($"@username", username);

                    NpgsqlDataReader reader = command.ExecuteReader();
                    
                    reader.Read();

                    user.UserID = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.Coins = reader.GetInt32(3);

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

        public void InsertToken(string username, string token)
        {
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "UPDATE users SET token = @token WHERE username = @username";
                command.Parameters.AddWithValue("token", token);
                command.Parameters.AddWithValue("username", username);

                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public UserModel Authorizationen(string token)
        {
            UserModel user = new UserModel();
            try
            {
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {

                    command.CommandText = "SELECT userid, username FROM users WHERE token = @token";
                    command.Parameters.AddWithValue("@token", token);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    user.UserID = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                }
            }
            //returns null if token doesnt exist
            catch (NullReferenceException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return user;           
        }

        public bool EditUserProfile(UserModel user)
        {
            try
            {
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {
                    command.CommandText = "UPDATE users SET username = @username, bio = @bio, image = @image WHERE userid = @userid";

                    command.Parameters.AddWithValue("@userid", user.UserID);
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@bio", user.Bio);
                    command.Parameters.AddWithValue("@image", user.Image);

                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }
    }
}
