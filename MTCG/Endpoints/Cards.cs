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
            return res;
        }
    }
}
