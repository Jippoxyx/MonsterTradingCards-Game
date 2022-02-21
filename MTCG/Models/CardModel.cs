namespace MTCG.Models
{
    public enum Elements
    { 
      Normal = 0, 
      Water = 1, 
      Fire = 2,
      Light = 3,
      Dark = 4
    }
    public enum CardType
    { 
        Spell = 0, 
      Monster = 1,
         Trap = 2,
        artifacts = 3,
        sorceries = 4
    }
    class CardModel
    {
        public string Id { get; set; }
        public string Name{ get; set; }
        public int Damage { get; set; }       
        public string Description { get; set; }
        public CardType Type { get; set; }
        public Elements Element { get; set; }

        public CardModel(string cardId, string Name, int Damage, string Descrition, CardType Type, Elements Element)
        {
            this.Id = cardId;
            this.Name = Name;
            this.Damage = Damage;
            this.Description = Description;
            this.Type = Type;
            this.Element = Element;
        } 
    }
}
