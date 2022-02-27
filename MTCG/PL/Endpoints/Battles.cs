using MTCG.BL.Battle;
using MTCG.Http;
using MTCG.Model;
using System.Collections.Generic;

namespace MTCG.Endpoint
{
    
    class Battles : EndpointBase<Request>
    {
        private static List<UserModel> instance = null;
        private static readonly object singletonLock = new object();

        public static List<UserModel> Instance
        {
            get
            {
                lock (singletonLock)
                {
                    if (instance == null)
                    {
                        instance = new List<UserModel>();
                    }
                    return instance;
                }
            }
        }

        public Battles(Request req) : base(req)
        {
            this.req = req;
        }

        public override Response POST()
        {
            //try
            //{

            if(Instance.Count < 2)
            {
                Instance.Add(userAcc.Authorizationen(req.Headers["Authorization"]));

                if (Instance.Count == 2)
                {
                    List<string> battleLog = new List<string>();

                    GameLogic gameLogic = new GameLogic(Instance[0], Instance[1]);
                    battleLog = gameLogic.StartGame();

                    res.StatusCode = (int)HttpStatusCode.OK; ;
                    for (int i = 0; i < battleLog.Count; i++)
                    {
                        res.Content += battleLog[i] + "\n";
                    }
                }
            }
   
            /*}

            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Something went wrong";
                return res;
            }*/
            return res;
        }

    }
}
