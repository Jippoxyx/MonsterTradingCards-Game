using MTCG.DAL.Access;
using MTCG.Http;
using MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.Handlers {
    class Users : ControllerBase<string>
    {
        private readonly string content;

        public Users(string content) : base(content)
        {
            this.content = content;
        }

        //Registration
        public override HttpResponse POST()
        {
            UserAccess user = new UserAccess();
            HttpResponse res = new HttpResponse();

            // "{\"Username\"\"":\"kienboec\", \"Password\":\"daniel\"}"          
            UserModel userObj = JsonSerializer.Deserialize<UserModel>(content);

            try
            {
                user.InsertUser(userObj);
            }
            catch (PostgresException)
            {
                Console.WriteLine("Username already exist");

            }

            res.StatusCode = (int)HttpStatusCode.OK;
            res.Content = "New User created";
            return res;
        }       
    }
}

