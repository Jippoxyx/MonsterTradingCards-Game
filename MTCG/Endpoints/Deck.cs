using MTCG.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Endpoint
{
    class Deck : EndpointBase<Request>
    {
        public Deck(Request req) : base(req)
        {
            this.req = req;
        }

        //show all acquired cards
        public override Response GET()
        {
            userObj = userAcc.Authorizationen(req.Headers["Authorization"]);
            if (userObj == null)
            {
                res.StatusCode = (int)HttpStatusCode.Forbidden;
                res.Content = "Access denied";
                return res;
            }

            /*if (req.Path.Contains("format=plain"))
            {
                res.StatusCode = (int)HttpStatusCode.OK;
                res.Content = "l";
            }
            else
            {
                res.StatusCode = (int)HttpStatusCode.OK;
                res.Content = "l";               
            }*/
            return res;
        }    
    }
}
