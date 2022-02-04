using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http
{
    public enum HttpStatusCode
    {
        OK = 200,      
        Created = 201,
        NotFound = 404,
        bad = 400
    }

    class HttpResponse
    {
        public Dictionary<string, string> Headers { get; set; }
        public int StatusCode{ get; set; }
        public string Content { get; set; }     
        public string Version { get; set; }

        public void sendResponse(StreamWriter writer)
        {
            // write the full HTTP-response


            //parseStatus(STATUS)

            // TODO Modify with anwser
            /*
            string content = $"<html><body><h1>test server</h1>" +
                $"Current Time: {DateTime.Now}" +
                $"<form method=\"GET\" action=\"/form\">" +
                $"<input type=\"text\" name=\"foo\" value=\"foovalue\">" +
                $"<input type=\"submit\" name=\"bar\" value=\"barvalue\">" +
                $"</form></html>";

            content = answer;
            
            Console.WriteLine();
            WriteLine(writer, $"HTTP/1.1");
            WriteLine(writer, "Server: My simple HttpServer");
            WriteLine(writer, $"Current Time: {DateTime.Now}");
            WriteLine(writer, $"Content-Length: {content.Length}");
            WriteLine(writer, "Content-Type: text/html; charset=utf-8");
            WriteLine(writer, "");
            WriteLine(writer, content);
            */
            writer.WriteLine();
            writer.Flush();
            writer.Close();
        }

        private void WriteLine(StreamWriter writer, string s)
        {
            Console.WriteLine(s);
            writer.WriteLine(s);
        }
    }
}
