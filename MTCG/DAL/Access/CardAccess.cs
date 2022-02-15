using MTCG.DAL.Database;
using MTCG.Models;
using Npgsql;
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
    }
}
