using MTCG.DAL.Access;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL.Battle
{
    public class GameLogic
    {
        public enum State
        {
            gameStart,
            draw,
            attack,
            endOfTheGame
        }

        public enum Winner
        {
            Player_1,
            Player_2,
            draw
        }

        private UserModel player_1 { get; set; }
        private List<CardModel> deck_1 { get; set; }
        private CardModel currentCard_1 { get; set; }
        private UserModel player_2 { get; set; }
        private List<CardModel> deck_2 { get; set; }
        private CardModel currentCard_2 { get; set; }

        private CardAccess cardAcc = new CardAccess();
        private UserAccess userAcc = new UserAccess();
        Winner winnerOfMatch;
        static Random rnd = new Random();
        private readonly List<string> battleHistory = new List<string>();
        bool uniqueFeature_player1 = false;
        bool uniqueFeature_player2 = false;
        public GameLogic(UserModel player_1, UserModel player_2)
        {
            this.player_1 = player_1;
            this.player_2 = player_2;
        }
        
        public List<string> StartGame()
        {
            State currentState = State.gameStart;           
            bool gameIsRunning = true;
            int roundCounter = 0;
            
            while(gameIsRunning)
            {
                switch(currentState)
                {
                    case State.gameStart:
                        battleHistory.Add($"---------------------- START GAME ----------------------" + "\n \n");
                        deck_1 = cardAcc.GetFullDeck(player_1);
                        deck_2 = cardAcc.GetFullDeck(player_2);
                        currentState = State.draw;
                        break;
                    case State.draw:                      
                        currentCard_1 = deck_1[rnd.Next(deck_1.Count)]; 
                        currentCard_2 = deck_2[rnd.Next(deck_2.Count)];
                        currentState = State.attack;
                        break;
                    case State.attack:
                        roundCounter++;
                        if (roundCounter <= 100)
                            battleHistory.Add($"Round: \"{roundCounter}\"  \n");
                        battle();
                        if (deck_1.Count == 0 || deck_2.Count == 0)
                            UniqueFeature();                                                     
                        currentState = ((deck_1.Count == 0 || deck_2.Count == 0) || (roundCounter > 100))
                                                      ? State.endOfTheGame : State.draw;
                        break;
                    case State.endOfTheGame:
                        UpdateStats(winnerOfMatch);                                              
                        gameIsRunning = false;
                        break;
                }
            }
            return battleHistory;
        }

        private void TakeOverCard(Winner currentWinner)
        {
            if(currentWinner == Winner.Player_1)
            {
                battleHistory.Add($"--> \"{currentCard_1.Name}\" defeats \"{currentCard_2.Name}\"");
                battleHistory.Add($"Winner  of this round: \"{player_1.Username}\"" + "\n");
                deck_1.Remove(currentCard_1);
                deck_2.Add(currentCard_1);
            }
            else if(currentWinner == Winner.Player_2)
            {
                battleHistory.Add($"-- > \"{currentCard_2.Name}\" defeats \"{currentCard_1.Name}\"");
                battleHistory.Add($"Winner  of this round: \"{player_2.Username}\"" + "\n");
                deck_2.Remove(currentCard_2);
                deck_1.Add(currentCard_1);
            }
            else
            {
                battleHistory.Add("DRAW" + "\n");
            }
        }

        private void battle()
        {
            Winner currentWinner;
            if (currentCard_1.Type == CardType.Monster && currentCard_2.Type == CardType.Monster)
            {
                //monster fights
                battleHistory.Add($"\"{player_1.Username}\": \"{currentCard_1.Name}\" (\"{currentCard_1.Damage}\") VS \"{player_2.Username}\": \"{currentCard_2.Name}\" (\"{currentCard_2.Damage}\")");
                currentWinner = MonsterFight(currentCard_1, currentCard_2);
                TakeOverCard(currentWinner);
            }
            else
            {
                //spell fight/mixed fight 
                battleHistory.Add($"\"{player_1.Username}\": \"{currentCard_1.Name}\" (\"{currentCard_1.Damage}\") VS \"{player_2.Username}\": \"{currentCard_2.Name}\" (\"{currentCard_2.Damage}\")");
                currentWinner = SpellFight(currentCard_1, currentCard_2);            
                TakeOverCard(currentWinner);
            }
            if ((deck_1.Count == 0 || deck_2.Count == 0))
                winnerOfMatch = currentWinner;    
        }

        public Winner MonsterFight(CardModel curr_1, CardModel curr_2)
        {
           Winner currentWinner = Winner.draw;
            if (curr_1.Name == "Dragon" && curr_2.Name == "WaterGoblin")
            {
                battleHistory.Add("Goblins are too afraid of Dragons to attack");
                return currentWinner = Winner.Player_1;
            }
            else if(curr_2.Name == "Dragon" && curr_1.Name == "WaterGoblin")
            {
                battleHistory.Add("Goblins are too afraid of Dragons to attack");
                return currentWinner = Winner.Player_2;
            }
            else if (curr_2.Name == "Wizzard" && curr_1.Name == "Ork")
            {
                battleHistory.Add("Wizzard can control Orks so they are not able to damage them");
                return currentWinner = Winner.Player_2;
            }
            else if (curr_1.Name == "Wizzard" && curr_2.Name == "Ork")
            {
                battleHistory.Add("Wizzard can control Orks so they are not able to damage them");
                return currentWinner = Winner.Player_1;
            }
            else if (curr_1.Name == "FireElf" && curr_2.Name == "Dragon")
            {
                battleHistory.Add("Fireelfs know Dragons since they were little and can evade their attacks");
                return currentWinner = Winner.Player_1;
            }
            else if (curr_2.Name == "FireElf" && curr_1.Name == "Dragon")
            {
                battleHistory.Add("Fireelfs know Dragons since they were little and can evade their attacks");
                return currentWinner = Winner.Player_2;
            }
            else if(curr_1.Damage == curr_2.Damage)
            {
                return currentWinner = Winner.draw;
            }
            else if (curr_1.Damage > curr_2.Damage)
            {               
                return currentWinner = Winner.Player_1;
            }
            else if(curr_1.Damage < curr_2.Damage)
            {            
                return currentWinner = Winner.Player_2;
            }
            return currentWinner;
        }

        public Winner SpellFight(CardModel curr_1, CardModel curr_2)
        {
            Winner currentWinner = Winner.draw;
            int newDmg_1 = curr_1.Damage;
            int newDmg_2 = curr_2.Damage;

            if (curr_1.Element == Elements.Water && curr_2.Element == Elements.Fire)
            {
                newDmg_1 *= 2;
                newDmg_2 /= 2;
                
                currentWinner = (curr_1.Damage * 2 > curr_2.Damage / 2) ? Winner.Player_1 : Winner.Player_2;                              
                if (curr_1.Damage * 2 == curr_2.Damage)
                    currentWinner = Winner.draw;
            }
            else if(curr_2.Element == Elements.Water && curr_1.Element == Elements.Fire)
            {
                newDmg_1 /= 2;
                newDmg_2 *= 2;

                currentWinner = (curr_2.Damage * 2 > curr_1.Damage / 2) ? Winner.Player_2 : Winner.Player_1;
                if (curr_2.Damage * 2 == curr_1.Damage)
                    currentWinner = Winner.draw;
            }
            else if (curr_1.Element == Elements.Normal && curr_2.Element == Elements.Water)
            {
                newDmg_1 *= 2;
                newDmg_2 /= 2;

                currentWinner = (curr_1.Damage * 2 > curr_2.Damage / 2) ? Winner.Player_1 : Winner.Player_2;
                if (curr_1.Damage * 2 == curr_2.Damage)
                    currentWinner = Winner.draw;
            }
            else if (curr_2.Element == Elements.Normal && curr_1.Element == Elements.Water)
            {
                newDmg_1 /= 2;
                newDmg_2 *= 2;

                currentWinner = (curr_2.Damage * 2 > curr_1.Damage / 2) ? Winner.Player_2 : Winner.Player_1;
                if (curr_2.Damage * 2 == curr_1.Damage)
                    currentWinner = Winner.draw;
            }
            else if(curr_1.Element == Elements.Fire && curr_2.Element == Elements.Normal)
            {
                newDmg_1 *= 2;

                currentWinner = (curr_1.Damage * 2 > curr_2.Damage) ? Winner.Player_1 : Winner.Player_2;
                if (curr_1.Damage * 2 == curr_2.Damage)
                    currentWinner = Winner.draw;
            }
            else if (curr_2.Element == Elements.Fire && curr_1.Element == Elements.Normal)
            {
                newDmg_2 *= 2;
                currentWinner = (curr_2.Damage * 2 > curr_1.Damage) ? Winner.Player_2 : Winner.Player_1;
                if (curr_2.Damage * 2 == curr_1.Damage)
                    currentWinner = Winner.draw;
            }
            else if (curr_1.Name == "Knight" && curr_2.Name == "WaterSpell")
            {
                return currentWinner = Winner.Player_2;
            }
            else if (curr_2.Name == "Knight" && curr_1.Name == "WaterSpell")
            {
                return currentWinner = Winner.Player_1;
            }
            else if(curr_1.Name == "Kraken" && curr_2.Type == CardType.Spell)
            {
                return currentWinner = Winner.Player_1;
            }
            else if (curr_2.Name == "Kraken" && curr_1.Type == CardType.Spell)
            {
                return currentWinner = Winner.Player_2;
            }
            else if (curr_1.Damage > curr_2.Damage)
            {
                return currentWinner = Winner.Player_1;
            }
            else if (curr_1.Damage < curr_2.Damage)
            {
                return currentWinner = Winner.Player_2;
            }
            battleHistory.Add($"==> \"{newDmg_1}\" VS \"{newDmg_2}\"");
            return currentWinner;
        }
        
        private void UniqueFeature()
        {           
            int number = rnd.Next(0, 100);
            if(number % 2 == 0)
            {
                List<CardModel> temporaryDeck = new List<CardModel>();
                CardModel temporaryCard;
                if (deck_1.Count == 0 && !(uniqueFeature_player1))
                {                  
                    temporaryDeck = cardAcc.GetFullDeck(player_1);
                    temporaryCard = temporaryDeck[rnd.Next(temporaryDeck.Count)];
                    deck_1.Add(temporaryCard);
                    uniqueFeature_player1 = true;
                    battleHistory.Add($"UNIQUE FEATURE ACTIVATED: \"{player_1.Username}\" got a card back! \n");
                    return;
                }
                else if (deck_2.Count == 0 && !(uniqueFeature_player2))
                {
                    temporaryDeck = cardAcc.GetFullDeck(player_2);
                    temporaryCard = temporaryDeck[rnd.Next(temporaryDeck.Count)];
                    deck_2.Add(temporaryCard);
                    uniqueFeature_player1 = false;
                    battleHistory.Add($"UNIQUE FEATURE ACTIVATED: \"{player_2.Username}\" got a card back! \n");
                    return;
                }
            }           
        }

        private void UpdateStats(Winner currentWinner)
        {
            if(currentWinner == Winner.Player_1)
            {
                player_1.Wins += 1;
                player_2.Loses += 1;
                player_1.Elo += 3;
                player_2.Elo -= 5;
                battleHistory.Add($" \n {player_1.Username}\" is the Winner of this Game!");

            }
            else if(currentWinner == Winner.Player_2)
            {
                player_2.Wins += 1;
                player_1.Loses += 1;
                player_2.Elo += 3;
                player_1.Elo -= 5;
                battleHistory.Add($" \n \"{player_2.Username}\" is the Winner of this Game!");
            }
            player_1.GamesPlayed += 1;
            player_2.GamesPlayed += 1;

            //update in db
            userAcc.UpdateUser(player_1);
            userAcc.UpdateUser(player_2);
        }
    }
}
