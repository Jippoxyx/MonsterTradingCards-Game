using MTCG.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;

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
            try
            {
                userObj = userAcc.Authorizationen(req.Headers["Authorization"]);
                if (userObj == null)
                {
                    res.StatusCode = (int)HttpStatusCode.Forbidden;
                    res.Content = "Access denied";
                    return res;
                }

                List<string> deck = new List<string>();
                deck = cardAcc.GetDeck(userObj);
                if(deck == null)
                {
                    res.StatusCode = (int)HttpStatusCode.BadRequest;
                    res.Content = "User has 0 cards selected";
                    return res;
                }
                else if (req.SubPath != null && req.SubPath.Contains("format=plain"))
                {
                    res.StatusCode = (int)HttpStatusCode.OK;
                 
                    for (int i = 0; i < deck.Count ; i++)
                    {
                        res.Content += deck[i] + "\n";
                    }                    
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.OK;
                    res.Content = JsonSerializer.Serialize(deck);
                }
            }
            catch (Exception)
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Error";
            }
            return res;
        }

        //configure deck 
        public override Response PUT()
        {
            try
            {
                userObj = userAcc.Authorizationen(req.Headers["Authorization"]);
                if (userObj == null)
                {
                    res.StatusCode = (int)HttpStatusCode.Forbidden;
                    res.Content = "Access denied";
                    return res;
                }
                else if (cardServ.ConfigureDeck(userObj))
                {
                    res.StatusCode = (int)HttpStatusCode.OK;
                    res.Content = "Success! 4 cards selected for users deck";
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.BadRequest;
                    res.Content = "User has already 4 cards in deck";
                }
            }
            catch (Exception)
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Error";
            }
            return res;
        }
    }
}
