using MTCG.DAL.Database;
using MTCG.Model;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;

namespace MTCG.DAL.Access
{
    class CardAccess
    {
        Postgres db = new Postgres();
        public void CreateCardsByAdmin(List<CardModel> cards)
        {
            foreach (var card in cards)
            {
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {
                    command.CommandText = "INSERT INTO cards (cardid, cardname, damage, description, type, elements) " +
                        "VALUES (@cardid, @cardname, @damage, @description, @type, @elements)";
                    command.Parameters.AddWithValue("@cardid", card.Id);
                    command.Parameters.AddWithValue("@cardname", card.Name);
                    command.Parameters.AddWithValue("@damage", card.Damage);
                    command.Parameters.AddWithValue("@description", card.Description);
                    command.Parameters.AddWithValue("@type", (int)card.Type);
                    command.Parameters.AddWithValue("@elements", (int)card.Element);

                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void GetPackage(UserModel user)
        {
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "UPDATE cards SET player = @userid WHERE cardid " +
                "IN(SELECT cardid FROM cards WHERE player IS NULL ORDER BY random() LIMIT 5)";
                command.Parameters.AddWithValue("userid", user.UserID);

                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public bool CheckPackagesAvailable()
        {
            Int64 count = 0;
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM cards WHERE player is null";
                count = (Int64)command.ExecuteScalar();
                command.Prepare();
                command.ExecuteNonQuery();
            }
            return (count > 5);
        }

        public void PayCoins(string username, int coins)
        {
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "UPDATE users SET coins = @coins  WHERE username = @username";
                command.Parameters.AddWithValue("coins", coins);
                command.Parameters.AddWithValue("username", username);

                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public List<string> GetAcquiredCards(UserModel user)
        {
            List<string> cards = new List<string>();
            try
            {
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {
                    command.CommandText = "SELECT cardname FROM cards WHERE player = @userid";
                    command.Parameters.AddWithValue("userid", user.UserID);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        cards.Add(reader.GetString(0));
                    }
                }
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return cards;
        }

        public List<string> GetDeck(UserModel user)
        {
            List<string> deck = new List<string>();
            try
            {
                using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
                {                 
                    command.CommandText = "SELECT cardname FROM cards WHERE deck is true AND player = @userid";
                    command.Parameters.AddWithValue("userid", user.UserID);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        deck.Add(reader.GetString(0));
                    }
                }
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return deck;
        }    

        //check if user has already 4 cards in deck 
        public bool CheckDeck(UserModel user)
        {
            Int64 count = 0;
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM cards WHERE player = @userid AND deck is true";
                command.Parameters.AddWithValue("userid", user.UserID);
                count = (Int64)command.ExecuteScalar();
                command.Prepare();
                command.ExecuteNonQuery();
            }
            return (count == 4);
        }

        public void select4CardsForDeck(UserModel user)
        {
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "UPDATE cards SET deck = true WHERE cardid " +
                "IN(SELECT cardid FROM cards WHERE player = @userid ORDER BY random() LIMIT 4)";
                command.Parameters.AddWithValue("userid", user.UserID);

                command.Prepare();
                command.ExecuteNonQuery();
            }
        }
    }
}
