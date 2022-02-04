using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http
{
    class HttpRequest
    {
        public string Method { get;  set; }
        public string Path { get;  set; }
        public string Version { get;  set; }

        public Dictionary<string, string> Headers { get; }


        public HttpResponse ProcessContent(StreamReader reader)
        {
            // write content to buffer
            char[] buffer = new char[Convert.ToInt32(Headers["Content-Length"])];
            reader.Read(buffer, 0, Convert.ToInt32(Headers["Content-Length"]));
            string content_string = new(buffer);

            // Execute process Member of called type (for instance: users)
            Type pathClass = getPathOfRequest();
            HttpResponse resp = new HttpResponse();
            if (pathClass != null)
                resp.Content = (string)getMethodOfType(pathClass, Method).Invoke(Activator.CreateInstance(pathClass, content_string), null);
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
    }
}
