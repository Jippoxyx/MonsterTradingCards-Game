using MTCG.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.Handlers {
    public class Users : HandlerBase<string>, IHandler {
        private readonly string content;

        public Users(string content) : base(content) {
            this.content = content;
        }

        public string Handle() {
            // TODO db stuff
            var deserializeObj = JsonSerializer.Deserialize<userTest>(content);
            return "DB stuff happened";
        }
    }
}
