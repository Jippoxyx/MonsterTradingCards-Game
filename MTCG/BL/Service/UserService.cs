using MTCG.Model;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL.Service
{
    class UserService
    {
        public string CreateToken(UserModel user)
        {
            string token = null;
            token = $"Basic {user.Username}-mtcgToken";                                        
            return token;
        }

        public bool loogedIn(UserModel plain, UserModel hashed)
        {
            if (plain != null && hashed != null)
            {
                if (BCrypt.Net.BCrypt.Verify(plain.Password, hashed.Password))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
