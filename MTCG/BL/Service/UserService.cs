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
        public string CreateToken(UserModel plain, UserModel hashed)
        {
            string token = null;
            try
            {               
                if(plain != null && hashed != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(plain.Password, hashed.Password))
                    {
                     
                        token = $"Basic {hashed.Username}-mtcgToken";
                    }
                }                
            }
            catch(NullReferenceException)
            {
                return null;
            }
            return token;
        }
    }
}
