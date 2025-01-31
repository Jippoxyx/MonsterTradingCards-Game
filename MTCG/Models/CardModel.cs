﻿namespace MTCG.Models
{
    public enum Elements
    { 
      Normal = 0, 
      Water = 1, 
      Fire = 2,
    }
    public enum CardType
    { 
        Spell = 0, 
      Monster = 1,
    }
    public class CardModel
    {
        public string Id { get; set; }
        public string Name{ get; set; }
        public int Damage { get; set; }       
        public string Description { get; set; }
        public CardType Type { get; set; }
        public Elements Element { get; set; }

        public CardModel(string cardId, string Name, int Damage, CardType Type, Elements Element)
        {
            this.Id = cardId;
            this.Name = Name;
            this.Damage = Damage;
            this.Type = Type;
            this.Element = Element;
        } 
    }
}
