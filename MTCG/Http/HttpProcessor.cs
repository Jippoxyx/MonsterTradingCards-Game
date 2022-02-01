using MTCG.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.Http
{
    class HttpProcessor
    {
        private TcpClient socket;
        private HttpServer httpServer;

        public string Method { get; private set; }
        public string Path { get; private set; }
        public string Version { get; private set; }

        public Dictionary<string, string> Headers { get; }

        public HttpProcessor(TcpClient s, HttpServer httpServer)
        {
            this.socket = s;
            this.httpServer = httpServer;

            Method = null;
            Headers = new();
        }

        public void Process()
        {
            var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(socket.GetStream());
            Console.WriteLine();
            string answer = "";

            // read (and handle) the full HTTP-request
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (line.Length == 0)
                    break;  // empty line means next comes the content (which is currently skipped)

                // handle first line of HTTP
                if (Method == null)
                {
                    var parts = line.Split(' ');
                    Method = parts[0];
                    Path = parts[1];
                    Version = parts[2];
                }
                // handle HTTP headers
                else
                {
                    var parts = line.Split(' ');
                    Headers.Add(parts[0].TrimEnd(':'), parts[1]);
                }

                //check if content length exists
                if (Headers.ContainsKey("Content-Length"))
                {
                    string contentLength;
                    Headers.TryGetValue("Content-Length", out contentLength);

                    // check if body is not empty
                    if (int.Parse(contentLength) > 0)
                        answer = ProcessContent(reader);
                    
                }
                //WriteAnswer(writer, answer);
            }                   
        }

        private string ProcessContent(StreamReader reader)
        {
            // write content to buffer
            char[] buffer = new char[Convert.ToInt32(Headers["Content-Length"])];
            reader.Read(buffer, 0, Convert.ToInt32(Headers["Content-Length"]));
            string content_string = new(buffer);

            // Execute process Member of called type (for instance: users)
            Type pathClass = getPathOfRequest();
            string answer;
            if (pathClass != null)
                answer = (string)getMethodOfType(pathClass).Invoke(
                    Activator.CreateInstance(pathClass, content_string), null);
            else
            {
                // return: no matching path or not implemented yet
                answer = "No matching pair";
            }
            return answer;
        }

        private Type getPathOfRequest()
        {
            // types must have unique names not case sensitive
            return Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name.ToLower() == Path.Trim().Replace("/", "").ToLower());
        }

        private MethodInfo getMethodOfType(Type type)
        {
            // interface IHandler enforces Handle
            return type.GetMethod(nameof(IHandler.Handle));
        }

        private void WriteLine(StreamWriter writer, string s)
        {
            Console.WriteLine(s);
            writer.WriteLine(s);
        }
    }
}
