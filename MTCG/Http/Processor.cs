﻿using MTCG.DAL.Access;
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

        Request req = new Request();
        Response res = new Response();

        public void Process()
        {
            var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(socket.GetStream());
           
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

                    res = ProcessContent(req);
                }                   
            }        
         res.sendResponse(writer);
        }

        public Response ProcessContent(Request req)
        {
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
            Sessions usee = new Sessions(req);
            resp = usee.POST();
        
            return resp;
        }
      
        public Type getPathOfRequest()
        {
            // types must have unique names not case sensitive
            return Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => t.Name.ToLower() == req.Path.Trim().Replace("/", "").ToLower());
        }

        public MethodInfo getMethodOfType(Type type, string method)
        {
            // interface IHandler enforces Handle
            return type.GetMethod(nameof(method));
        }
    }
}