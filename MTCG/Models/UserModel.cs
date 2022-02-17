using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Model
{
    class UserModel
    {
        public int UserID { get; set; }
        public string Username{ get; set;}
        public string Password { get; set; }
        public int Coins { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

    }
}
