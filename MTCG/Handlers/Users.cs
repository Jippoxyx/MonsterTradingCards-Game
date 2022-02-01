using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Handlers
{
    class Users
    {
        private readonly string content;

        public Users(string content) : base(content)
        {
            this.content = content;
        }

        public string Handle()
        {
            // TODO db stuff
            var deserializeObj = JsonSerializer.Deserialize<userTest>(content);
            return "DB stuff happened";
        }
    }
}
