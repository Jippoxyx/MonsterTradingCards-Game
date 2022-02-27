using MTCG.Http;
using MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.Endpoint
{
    class Sessions : EndpointBase<Request>
    {
        private UserModel users = new UserModel();
        public Sessions(Request req) : base(req)
        {
            this.req = req;
        }

        //Login
        public override Response POST()
        {
            try
            {
                userObj = JsonSerializer.Deserialize<UserModel>(req.Content);
                string token = "";

                //returns null if username doesnt exist
                users = userAcc.GetUserByName(userObj.Username);               
                //true if password macthes
                if (users != null && userServ.loogedIn(userObj, users))
                {
                    token = userServ.CreateToken(users);                  
                    res.StatusCode = (int)HttpStatusCode.OK;
                    res.Content = token;               
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
                    res.Content = "Username or Password wrong";
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Uppsii something went wrong");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Error";
            }
            return res;
        }
    }  
}
