using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public enum Element
    { 
      Normal, 
      Water, 
      Fire 
    }
    public enum CardType
    { 
        Spell, 
      Monster 
    }
    class CardModel
    {
        public string Id { get; set; }
        public string Name{ get; set; }
        public int Damage { get; set; }       
        public string Description { get; set; }
        public CardType Type { get; set; }
        public Element Element { get; set; }

        public CardModel(string cardId, string Name, int Damage, string Descrition, CardType Type, Element Element)
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
