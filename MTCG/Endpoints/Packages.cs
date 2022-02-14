﻿
using MTCG.Http;
using MTCG.Model;
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
        //private CardModel cardObj = new CardModel();
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
                    
                    //cardAcc.CreatePackagesByAdmin(package);
                                  
                    Console.WriteLine("New Cards created by amdin");
                    res.StatusCode = (int)HttpStatusCode.Created;
                    res.Content = "New Cards created by admin";
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
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
/*
     foreach (var value in req.Headers.Values)
     {
         Console.WriteLine("Value of the Dictionary Item is: {0}", value);
     }

      foreach (var value in req.Headers.Keys)
      {
          Console.WriteLine("Value of the Dictionary Item is: {0}", value);
      }

      foreach (CardModel card in package)
      {
         Console.WriteLine(card);
      }
*/