using MTCG.Endpoint;
using MTCG.Http;
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
            string token = req.Headers["Authorization"];
            Console.WriteLine(token);
            return res;
        }

        //show all acquired cards
        public override Response GET()
        {
            return res;
        }
    }
}

