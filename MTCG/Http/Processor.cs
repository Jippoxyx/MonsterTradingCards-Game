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
    class Processor
    {
        private TcpClient socket;
        private Server httpServer;
       
        public Processor(TcpClient s, Server httpServer)
        {
            this.socket = s;
            this.httpServer = httpServer;
        }

        public void Process()
        {
            var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(socket.GetStream());
            Request req = new Request();
            Response res = new Response();
            Console.WriteLine();
            
            // read (and handle) the full HTTP-request
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (line.Length == 0)
                    break;  // empty line means next comes the content (which is skipped in header assembly)

                // handle first line of HTTP
                if (req.Method == null)
                {
                    var parts = line.Split(' ');
                    req.Method = parts[0]; // POST
                    req.Path = parts[1]; // /users 
                    req.Version = parts[2]; // HTTP/1.1
                }
                // handle HTTP headers
                else
                {
                    var parts = line.Split(' ');
                    req.AddHeaders(parts[0].TrimEnd(':'), parts[1]);
                }
            }
            if (req.Headers.ContainsKey("Content-Length"))
            {
                string contentLength;
                req.Headers.TryGetValue("Content-Length", out contentLength);

                if (int.Parse(contentLength) > 0)
                {              
                     char[] buffer = new char[Convert.ToInt32(req.Headers["Content-Length"])];
                     reader.Read(buffer, 0, Convert.ToInt32(req.Headers["Content-Length"]));
                     string content_string = new(buffer);
                    req.Content = content_string;

                    res = req.ProcessContent(req);
                }                   
            }        
         res.sendResponse(writer);
        }  
    }
}