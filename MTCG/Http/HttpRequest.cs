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
    class HttpRequest
    {
        public string Method { get;  set; }
        public string Path { get;  set; }
        public string Version { get; set; }
        public Dictionary<string, string> Headers { get; }

        public HttpRequest()
        {
            Headers = new Dictionary<string, string>();
        }

        public HttpResponse ProcessContent(StreamReader reader)
        {
            // write content to buffer
            char[] buffer = new char[Convert.ToInt32(Headers["Content-Length"])];
            reader.Read(buffer, 0, Convert.ToInt32(Headers["Content-Length"]));
            string content_string = new(buffer);

            // Execute process Member of called type (for instance: users)
            Type pathClass = getPathOfRequest();
            HttpResponse resp = new HttpResponse();

            /*
            if (pathClass != null)
                resp = (HttpResponse)getMethodOfType(pathClass, Method).Invoke(Activator.CreateInstance(pathClass, content_string), null);
            //error catch wenn path nicht existiert 
            else
            {
                res.Content = null;
                resp.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            

            resp.StatusCode = (int)HttpStatusCode.BadRequest;           
            */   
            

            Users use = new Users(content_string);         
            resp = use.POST();

            //ERROR HANDLING
            return resp;
        }

        private Type getPathOfRequest()
        {
            // types must have unique names not case sensitive
            return Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name.ToLower() == Path.Trim().Replace("/", "").ToLower());
        }

        private MethodInfo getMethodOfType(Type type, string method)
        {
            // interface IHandler enforces Handle
            return type.GetMethod(nameof(method));
        }

        public void AddHeaders(string key, string value)
        {
            Headers.Add(key, value);
        }
    }
}
