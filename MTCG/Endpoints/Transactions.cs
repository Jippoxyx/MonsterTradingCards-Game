using MTCG.Endpoint;
using MTCG.Http;
using MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Endpoint
{
    class Transactions : EndpointBase<Request>
    {
        public Transactions(Request req) : base(req)
        {
            this.req = req;
        }

        //acquire packages 
        public override Response POST()
        {
            try
            {
                userObj = userAcc.Authorizationen(req.Headers["Authorization"]);
                if(userObj == null)
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
                    res.Content = "Invalid token";
                    return res;
                }
                else if(userObj.Coins < 5)
                {
                    res.StatusCode = (int)HttpStatusCode.Forbidden;
                    res.Content = "Error, User has not enough money";
                    return res;
                }
                else if(cardServ.AcquirePackages(userObj))
                {
                    res.StatusCode = (int)HttpStatusCode.OK;
                    res.Content = "Acquireing packages was successful!";
                    return res;
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.BadRequest;
                    res.Content = "Error, no packages available";
                    return res;
                }                
            }
            catch (Exception)
            {
                Console.WriteLine("Uppsii something went wrong");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Error";
            }          
            return res;
        }
    }
}

