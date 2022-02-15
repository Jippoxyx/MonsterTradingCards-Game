using MTCG.DAL.Access;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL.Service
{
    class CardService
    {
        private Random random = new Random();
        private CardAccess cardAcc = new CardAccess();
        public string GenerateCardDescription(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool CreateCards(List<CardModel> package)
        {
            if(package != null)
            {
                Array values = Enum.GetValues(typeof(Elements));
                foreach (var card in package)
                {
                    //get type
                    if (card.Name.Contains("Spell"))
                    {
                        card.Type = (int)CardType.Spell;                      
                    }
                    else
                    {
                        card.Type = (CardType)(int)CardType.Monster;
                    }
                    card.Description = GenerateCardDescription(20);                
                    //get random Element                    
                    card.Element = (Elements)values.GetValue(random.Next(values.Length));
                }
                cardAcc.CreateCardsByAdmin(package);
                return true;
            }           
            return false;
        }
    }
}
