using MTCG.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Handlers
{
    class Sessions : EndpointBase<Request>
    {                 
        public Sessions(Request req) : base(req)
        {
            this.req = req;
        }

        //Login
        public override Response POST()
        {
            return res;
        }
    }  
}
