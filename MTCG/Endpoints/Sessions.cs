using MTCG.Http;
using MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.Handlers
{
    class Sessions : EndpointBase<Request>
    {
        public UserModel users = new UserModel();
        public Sessions(Request req) : base(req)
        {
            this.req = req;
        }

        //Login
        public override Response POST()
        {
            /*try
            {
                userObj = JsonSerializer.Deserialize<UserModel>(req.Content);
                string token = "";

                users = userAcc.GetUserByName(userObj.Username);
                token = userServ.CreateToken(userObj, users);

                Console.WriteLine("{0} logged in", users.Username);
                res.StatusCode = (int)HttpStatusCode.OK;
                res.Content = token;
            }
             catch(NullReferenceException)
            {
                Console.WriteLine("Username doesn´t exist");
                res.StatusCode = (int)HttpStatusCode.NotFound;
                res.Content = "Username doesn´t exist";
            }
        

            Console.WriteLine("Password doesn´t match");
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            res.Content = "Password doesn´t match";
            */

            try
            {
                userObj = JsonSerializer.Deserialize<UserModel>(req.Content);
                string token = "";

                users = userAcc.GetUserByName(userObj.Username);               

                if (users != null)
                {
                    token = userServ.CreateToken(userObj, users);
                    if (String.IsNullOrEmpty(token))
                    {
                        res.StatusCode = (int)HttpStatusCode.BadRequest;
                        res.Content = "Password doesnt match";
                    }
                    else
                    {
                        res.StatusCode = (int)HttpStatusCode.OK;
                        res.Content = token;
                    }
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.NoContent;
                    res.Content = "Username doesnt exist";
                }

                
                
            }
            catch(Exception)
            {
                Console.WriteLine("Password h");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Password doesnt match";
            }
          

            return res;
        }
    }  
}
