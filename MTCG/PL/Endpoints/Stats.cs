using MTCG.Http;
using System;
using System.Collections.Generic;

namespace MTCG.Endpoint
{
    class Stats : EndpointBase<Request>
    {
        public Stats(Request req) : base(req)
        {
            this.req = req;
        }

        //get stats
        public override Response GET()
        {
            try
            {
                userObj = userAcc.Authorization(req.Headers["Authorization"]);
                if (userObj == null)
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
                    res.Content = "Invalid token";
                    return res;
                }

                List<Object> stats = new List<object>();
                stats = userServ.GetStats(userObj);
                res.StatusCode = (int)HttpStatusCode.OK;

                for (int i = 0; i < stats.Count + 1; i++)
                {
                    if(i == 0)
                    {
                        res.Content += "wins: " + stats[i] + "\n";
                    }
                    else if(i == 1)
                    {
                        res.Content += "loses: " + stats[i] + "\n";
                    }                  
                    else if(i == 2)
                    {
                        res.Content += "elo: " + stats[i] + "\n";
                    }   
                    else
                    {
                        res.Content += "Win Lose Ration: " + userServ.GetWinLoseRatio(userObj) + "\n";
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Something went wrong";
            }
            return res;
        }
    }
}
