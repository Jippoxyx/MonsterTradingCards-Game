using MTCG.DAL.Database;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Access
{
    class CardAccess
    {
        Postgres db = new Postgres();
        public void CreateCardsByAdmin(List<CardModel> cards)
        {
            using (NpgsqlCommand command = db.CreateConnection().CreateCommand())
            {
                command.CommandText = "INSERT INTO cards (cardid, cardname, damage, description, type, element) " +
                    "VALUES (@cardid, @cardname, @damage, @description, @type, @element)";

                //command.Parameters.AddWithValue("", );
                //command.Parameters.AddWithValue("", );

                command.Prepare();

                command.ExecuteNonQuery();
            }
        }
    }
}
