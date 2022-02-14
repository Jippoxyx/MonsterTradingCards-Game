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

        public override Response POST()
        {
            Console.WriteLine("This message is from TransactionsPackage");
            return res;
        }
    } 
}

