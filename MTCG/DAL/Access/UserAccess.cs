using MTCG.DAL.Database;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;

namespace MTCG.DAL.Access
{
    public class UserAccess
    {
        Postgres db = new Postgres();

        //Registration
        public bool CreateUser(UserModel user)
        {
            try
            {
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {
                    command.CommandText = "INSERT INTO users (username, password, coins,wins,loses, elo, games_played) VALUES (@username, @password, @coins, @wins, @loses, @elo, @games_played)";
                    command.Parameters.AddWithValue("username", user.Username);
                    command.Parameters.AddWithValue("password", BCrypt.Net.BCrypt.HashPassword(user.Password));
                    command.Parameters.AddWithValue("coins", 20);
                    command.Parameters.AddWithValue("wins", 0);
                    command.Parameters.AddWithValue("loses", 0);
                    command.Parameters.AddWithValue("elo", 100);
                    command.Parameters.AddWithValue("games_played", 0);

                    command.Prepare();
                    command.ExecuteNonQuery();
                }
                db.Dispose();
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (PostgresException)
            {
                return false;
            }
            return true;
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
                db.Dispose();
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
            db.Dispose();
        }

        public UserModel Authorization(string token)
        {
            UserModel user = new UserModel();
            try
            {
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {

                    command.CommandText = "SELECT userid, username, coins, wins, loses, elo, games_played FROM users WHERE token = @token";
                    command.Parameters.AddWithValue("@token", token);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    user.UserID = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                    user.Coins = reader.GetInt32(2);
                    user.Wins = reader.GetInt32(3); ;
                    user.Loses = reader.GetInt32(4);
                    user.Elo = reader.GetInt32(5);
                    user.GamesPlayed = reader.GetInt32(6);
                }
                db.Dispose();
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
                db.Dispose();
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

        public Dictionary<string, int> GetScoreboard()
        {
            Dictionary<string, int> scores = new Dictionary<string, int>();
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "SELECT username, elo FROM users ORDER BY elo DESC";               

                NpgsqlDataReader reader = command.ExecuteReader();           

                while (reader.Read())
                {
                    scores.Add(reader.GetString(0), reader.GetInt32(1));
                }
            }
            db.Dispose();
            return scores;
        }

        public void UpdateUser(UserModel user)
        {
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "UPDATE users SET " +
                    "username = @username, wins = @wins, loses = @loses, elo = @elo, games_played = @games_played, winloseratio = @winloseratio" +
                    " WHERE userid = @userid";
                command.Parameters.AddWithValue("@userid", user.UserID);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@wins", user.Wins);
                command.Parameters.AddWithValue("@loses", user.Loses);
                command.Parameters.AddWithValue("@elo", user.Elo);
                command.Parameters.AddWithValue("@games_played", user.GamesPlayed);
                command.Parameters.AddWithValue("@winloseratio", user.WinLoseRatio);

                command.Prepare();
                command.ExecuteNonQuery();
            }
            db.Dispose();
        }
    }
}
