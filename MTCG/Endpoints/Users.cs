using MTCG.DAL.Access;
using MTCG.Http;
using MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.Json;


namespace MTCG.Endpoint {
    class Users : EndpointBase<Request>
    {    
        public Users(Request req) : base(req)
        {
            this.req = req;
        }

        //registration
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
        
        //get profile
        public override Response GET()
        {
            try
            {
                userObj = userAcc.Authorizationen(req.Headers["Authorization"]);
                if (userObj == null)
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
                    res.Content = "Invalid token";
                    return res;                  
                }
                else if(userObj.Username != req.SubPath)
                {
                    res.StatusCode = (int)HttpStatusCode.Forbidden;
                    res.Content = "Access denied";
                    return res;
                }

                res.StatusCode = (int)HttpStatusCode.OK;
                res.Content = userServ.GetUserProfile(userObj);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Something went wrong";
            }
            return res;
        }

        //change user profile
        public override Response PUT()
        {
           
                userObj = userAcc.Authorizationen(req.Headers["Authorization"]);
                if (userObj == null)
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
                    res.Content = "Invalid token";
                    return res;
                }
                else if (userObj.Username != req.SubPath)
                {
                    res.StatusCode = (int)HttpStatusCode.Forbidden;
                    res.Content = "Access denied";
                    return res;
                }

                Dictionary<string, string> userUpdate = JsonSerializer.Deserialize<Dictionary<string, string>>(req.Content);
                userObj.Username = userUpdate["Name"];
                userObj.Bio = userUpdate["Bio"];
                userObj.Image = userUpdate["Image"];
                if (userAcc.EditUserProfile(userObj))
                {
                    res.StatusCode = (int)HttpStatusCode.OK;
                    res.Content = "Success! User profile updated";
                }     
            
            /*catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                res.Content = "Something went wrong";
            }*/
            return res;
        }
    }
}

