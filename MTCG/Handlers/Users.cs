using MTCG.Http;
using MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.Handlers {
    public class Users : HandlerBase<string>
    {
        private readonly string content;

        public Users(string content) : base(content)
        {
            this.content = content;
        }

        public override HttpResponse POST()
        {
            // TODO db stuff
            // "{\"Username\"\"":\"kienboec\", \"Password\":\"daniel\"}"          
            UserModel userObj = JsonSerializer.Deserialize<UserModel>(content);
            //databasemanager(userObj);

            HttpResponse res = new HttpResponse();
            res.StatusCode = (int)HttpStatusCode.bad;
            return res;
        }
        
    }
}

