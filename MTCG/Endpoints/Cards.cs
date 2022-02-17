using MTCG.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Endpoint
{
    class Cards : EndpointBase<Request>
    {
        public Cards(Request req) : base(req)
        {
            this.req = req;
        }

        //show all acquired cards
        public override Response GET()
        {
            try
            {
                userObj = userAcc.Authorizationen(req.Headers["Authorization"]);
                if (userObj == null)
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
                    res.Content = "Invalid token";
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
