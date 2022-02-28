using MTCG.Http;
using System;
using System.Collections.Generic;

namespace MTCG.Endpoint
{
    class Score : EndpointBase<Request>
    {
        public Score(Request req) : base(req)
        {
            this.req = req;
        }

        //show scoreboard
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

                Dictionary<string, int> scores = new Dictionary<string, int>();
                scores = userAcc.GetScoreboard();

                res.StatusCode = (int)HttpStatusCode.OK;
                
                foreach(KeyValuePair<string, int> score in scores)
                {
                    res.Content += (score.Key, score.Value) + "\n";
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
