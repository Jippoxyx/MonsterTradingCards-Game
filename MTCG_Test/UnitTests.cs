using NUnit.Framework;
using MTCG.Models;
using MTCG.BL.Battle;
using System;
using MTCG.BL.Service;
using System.Collections.Generic;
using MTCG.DAL.Access;

namespace MTCG_Test
{
    public class Tests
    {
        public enum Winner
        {
            Player_1,
            Player_2,
            draw

        }
        UserService userServ = new UserService();
        UserAccess userAcc = new UserAccess();
        CardService cardServ = new CardService();
        UserModel player_1 = new UserModel();
        UserModel player_2 = new UserModel();

        [Test]
        public void RegisterUser()
        {
            //ARRANGE
            player_1.Username = "Jippoxyx";
            player_1.Password = "TestTest";
            player_2.Username = "Jippoxyx";
            player_2.Password = "TestTe";

            //ACT
            bool created = userAcc.CreateUser(player_1);
            bool created_2 = userAcc.CreateUser(player_2);

            //ASSERT
            Assert.IsTrue(created);
            Assert.IsFalse(created_2);
        }

        [Test]
        public void UserDoesNotExist()
        {
            //ARRANGE
            UserModel userObj = new UserModel();
            player_1.Username = "IamNotExisting";

            //ACT
            userObj = userAcc.GetUserByName(player_1.Username);

            //ASSERT
            Assert.AreEqual(userObj, null);
        }

        [Test]
        public void UserExist()
        {
            //ARRANGE
            UserModel userObj = new UserModel();
            player_1.Username = "Jippoxyx";

            //ACT
            userObj = userAcc.GetUserByName(player_1.Username);

            //ASSERT
            Assert.AreEqual(userObj.Username, "Jippoxyx");
        }

        [Test]
        public void CreateToken()
        {
            //ARRANGE           
            player_1.Username = "Jippoxyx";
            //ACT
            string createdToken = userServ.CreateToken(player_1);
            string expectedToken = "Basic Jippoxyx-mtcgToken";

            //ASSERT
            Assert.AreEqual(createdToken, expectedToken);
        }

        [Test]
        public void CheckPassword_true()
        {
            //ARRANGE           
            player_1.Username = "Jippoxyx";
            player_1.Password = "TestTest";


            string realPw = BCrypt.Net.BCrypt.HashPassword(player_1.Password);
            player_2.Username = "Jippoxyx";
            player_2.Password = realPw;

            //ACT
            bool loggedIn = userServ.loogedIn(player_1, player_2);

            //ASSERT
            Assert.IsTrue(loggedIn);
        }

        [Test]
        public void CheckPassword_false()
        {
            //ARRANGE           
            player_1.Username = "Jippoxyx";
            player_1.Password = "falsePw";


            string realPw = BCrypt.Net.BCrypt.HashPassword("TestTest");
            player_2.Username = "Jippoxyx";
            player_2.Password = realPw;

            //ACT
            bool loggedIn = userServ.loogedIn(player_1, player_2);

            //ASSERT
            Assert.IsFalse(loggedIn);
        }

        [Test]
        public void CreateCards_false()
        {
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "FireElf", 40, CardType.Monster, Elements.Fire);
            CardModel card_2 = new CardModel("RandomI", "WaterGoblin", 45, CardType.Monster, Elements.Water);
            CardModel card_3 = new CardModel("Random", "FireElf", 40, CardType.Monster, Elements.Fire);          

            List<CardModel> package = new List<CardModel>();
            package.Add(card_1);
            package.Add(card_2);
            package.Add(card_3);

            //ASSERT
            Assert.IsFalse(cardServ.CreateCards(package));
        }

        [Test]
        public void WinLoseRatio()
        {
            //ARRANGE
            player_1.Wins = 10;
            player_1.Loses = 5;

            //ACT
            int winLoseRatio = userServ.GetWinLoseRatio(player_1);
            int expectedwinLoseRatio = 2;

            //ASSERT
            Assert.AreEqual(winLoseRatio, expectedwinLoseRatio);
        }

        [Test]
        public void GetStats()
        {
            //ARRANGE         
            List<Object> stats = new List<object>();

            //ACT
            stats = userServ.GetStats(player_1);

            //ASSERT
            Assert.AreEqual(player_1.Wins, 0);
            Assert.AreEqual(player_1.Loses, 0);
        }

        [Test]
        public void BuyPackage_true()
        {
            //ARRANGE
            UserModel userObj = new UserModel();
            player_1.Username = "Jippoxyx";

            //ACT
            userObj = userAcc.GetUserByName(player_1.Username);

            //ASSERT
            Assert.IsTrue(cardServ.AcquirePackages(userObj));
        }

        [Test]
        public void BuyPackage_fail()
        {
            //ARRANGE
            UserModel userObj = new UserModel();
            player_1.Username = "Jippoxyx";

            //ACT
            userObj = userAcc.GetUserByName(player_1.Username);
            userObj.Coins -= 18;

            //ASSERT
            Assert.IsFalse(cardServ.AcquirePackages(userObj));
        }

        [Test]
        public void MonsterFight_1()
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "FireElf", 40, CardType.Monster, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "WaterGoblin", 45, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.MonsterFight(card_1, card_2);
            Winner realWinner = Winner.Player_2;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }

        [Test]
        public void MonsterFight_2()
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "FireElf", 50, CardType.Monster, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "WaterGoblin", 45, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.MonsterFight(card_1, card_2);
            Winner realWinner = Winner.Player_1;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }

        [Test]
        public void MonsterFight_3() //Goblins are too afraid of Dragons to attack
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "WaterGoblin", 50, CardType.Monster, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "Dragon", 45, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.MonsterFight(card_1, card_2);
            Winner realWinner = Winner.Player_2;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }

        [Test]
        public void MonsterFight_4() //Wizzard can control Orks so they are not able to damage them
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "Wizzard", 50, CardType.Monster, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "Orks", 45, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.MonsterFight(card_1, card_2);
            Winner realWinner = Winner.Player_1;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }


        [Test]
        public void MonsterFight_5() //Fireelfs know Dragons since they were little and can evade their attacks
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "Fireelf", 50, CardType.Monster, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "Orks", 45, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.MonsterFight(card_1, card_2);
            Winner realWinner = Winner.Player_1;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }

        [Test]
        public void SellFight_Water_VS_Fire()
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "Knight", 100, CardType.Spell, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "WaterSpell", 55, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.SpellFight(card_1, card_2);
            Winner realWinner = Winner.Player_2;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }

        [Test]
        public void SellFight_Water_VS_Normal()
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "Knight", 100, CardType.Spell, Elements.Normal);
            CardModel card_2 = new CardModel("RandomID", "WaterSpell", 55, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.SpellFight(card_1, card_2);
            Winner realWinner = Winner.Player_1;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }

        [Test]
        public void SellFight_Knight_VS_WaterSpell()
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "Knight", 50, CardType.Spell, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "WaterSpell", 45, CardType.Monster, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.SpellFight(card_1, card_2);
            Winner realWinner = Winner.Player_2;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }

        [Test]
        public void SellFight_Kraken_VS_Type_Spell()
        {
            GameLogic battle = new GameLogic(player_1, player_2);
            //ARRANGE
            CardModel card_1 = new CardModel("RandomID", "Kraken",5, CardType.Monster, Elements.Fire);
            CardModel card_2 = new CardModel("RandomID", "WaterSpell", 45, CardType.Spell, Elements.Water);

            //ACT
            Winner currentWinner = (Winner)battle.SpellFight(card_1, card_2);
            Winner realWinner = Winner.Player_2;

            //ASSERT
            Assert.AreEqual(currentWinner, realWinner);
        }
    }
}
    