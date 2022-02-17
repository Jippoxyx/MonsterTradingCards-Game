using MTCG.DAL.Access;
using MTCG.Model;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                        card.Type = CardType.Monster;
                    }
                    //get random description
                    card.Description = GenerateCardDescription(29);                
                    //get random Element                    
                    card.Element = (Elements)values.GetValue(random.Next(values.Length));
                }
                cardAcc.CreateCardsByAdmin(package);
                return true;
            }           
            return false;
        }

        public bool AcquirePackages(UserModel user)
        {
            //check if at least 5 packages available
            if(cardAcc.CheckPackagesAvailable())
            {
                //assign cards to the user
                cardAcc.GetPackage(user);
                //user pays 5 coins
                int currentCoins = user.Coins;
                currentCoins -= 5;
                cardAcc.PayCoins(user.Username, currentCoins);
                return true;
            }
            return false;
        }
       // public List<CardModel> ShowAcquiredCards()
        //{

  //      }
    }
}
