using MTCG.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using MTCG.DAL.Database;
using MTCG.DAL.Access;


namespace MTCG.Http
{
    class Request
    {
        public string Method { get;  set; }
        public string Path { get;  set; }
        public string Version { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> Headers { get; }

        public Request()
        {
            Headers = new Dictionary<string, string>();
        }

        public void AddHeaders(string key, string value)
        {
            Headers.Add(key, value);
        }
    }
}
