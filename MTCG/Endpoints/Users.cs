using MTCG.DAL.Access;
using MTCG.Http;
using MTCG.Model;
using Npgsql;
using System;
using System.Text.Json;


namespace MTCG.Handlers {
    class Users : EndpointBase<Request>
    {    
        public Users(Request req) : base(req)
        {
            this.req = req;
        }

        //Registration
        public override Response POST()
        {    
            try
            {
                userObj = JsonSerializer.Deserialize<UserModel>(req.Content);
                Console.WriteLine(req.Content);
                userAcc.CreateUser(userObj);
                Console.WriteLine("New User created");
                res.StatusCode = (int)HttpStatusCode.Created;
                res.Content = "New User created";
            }
            catch (PostgresException)
            {
                Console.WriteLine("Username already exist");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Username already exist";
                return res;
            }
            catch(JsonException)
            {
                Console.WriteLine("Incorrect JSON format");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Incorrect JSON format";
                return res;
            }
            catch(Exception)
            {
                Console.WriteLine("Something went wrong");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Something went wrong";
                return res;
            }
            
            return res;
        }
        
        public override Response DELETE()
        {
            Console.WriteLine("Server refuses to fulfill the request");
            res.Content = "Server refuses to fulfill the request";
            res.StatusCode = (int)HttpStatusCode.Forbidden;
            return res;
        }
    }
}

