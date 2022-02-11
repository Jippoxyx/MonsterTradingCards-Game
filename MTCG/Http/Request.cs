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

        public Response ProcessContent(Request req)
        {
            // Execute process Member of called type (for instance: users)
            Type pathClass = getPathOfRequest();
            Response resp = new Response();

            /*
            if (pathClass != null)
             {
                resp = (HttpResponse)getMethodOfType(pathClass, Method)
            .Invoke(Activator.CreateInstance(pathClass, content_string), null);
            }
             else
            {
                    res.StatusCode = (int)HttpStatusCode.BadRequest;
                    res.Content = "Something went wrong";
            return res;
            }
            */
            Users user = new Users(req);
            Sessions use = new Sessions(req);         
            resp = user.POST();

            return resp;
        }

        private Type getPathOfRequest()
        {
            // types must have unique names not case sensitive
            return Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => t.Name.ToLower() == Path.Trim().Replace("/", "").ToLower());
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
