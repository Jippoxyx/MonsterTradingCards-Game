using MTCG.DAL.Access;
using MTCG.Model;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.BL.Service
{
    class UserService
    {
        private UserAccess userAcc = new UserAccess();
        public string CreateToken(UserModel user)
        {
            string token = null;
            token = $"Basic {user.Username}-mtcgToken";
            userAcc.InsertToken(user.Username, token);

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

        public string GetUserProfile(UserModel user)
        {
            List<Object> profile = new List<object>();
            profile.Add(user.UserID);
            profile.Add(user.Username);
            profile.Add(user.Coins);
            profile.Add(user.Bio);
            profile.Add(user.Image);
            return JsonSerializer.Serialize(profile);
        }
    }  
}