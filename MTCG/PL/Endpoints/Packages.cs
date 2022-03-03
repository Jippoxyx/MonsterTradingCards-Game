
using MTCG.Http;
using MTCG.Models;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.Endpoint
{
    class Packages : EndpointBase<Request>
    {
        public Packages(Request req) : base(req)
        {
            this.req = req;
        }

        //create Packages by admin
        public override Response POST()
        {
            try
            {         
                if(req.Headers["Authorization"] == "Basic admin-mtcgToken")
                {
                    req.Content = req.Content.Replace(".0", "");
                   
                    List<CardModel> package;                   
                    package = JsonConvert.DeserializeObject < List<CardModel> > (req.Content);
                    
                    if(cardServ.CreateCards(package))
                    {
                        res.StatusCode = (int)HttpStatusCode.Created;
                        res.Content = "New Cards created by admin";
                        return res;
                    }                   
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.Forbidden;
                    res.Content = "Only admin can create new packages";
                    return res;
                }             
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Incorrect JSON format");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Incorrect JSON format";
            }
            catch (PostgresException)
            {
                Console.WriteLine("One or more Cards already exist");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "One or more Cards already exist";            
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No content received");
                res.StatusCode = (int)HttpStatusCode.NoContent;
                res.Content = "No content received";
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