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
        NoContent = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404
    }

    class Response
    {
        public Dictionary<string, string> Headers { get; set; }
        public int StatusCode{ get; set; }
        public string Content { get; set; }     
  
        public void sendResponse(StreamWriter writer)
        {                  
            Console.WriteLine();
            WriteLine(writer, $"HTTP/1.1 {StatusCode}");        
            WriteLine(writer, $"Current Time: {DateTime.Now}");
            WriteLine(writer,"Server: MCTG Server");

            if(Content != null)
            {
                WriteLine(writer, $"Content-Length: {Content.Length}");
                WriteLine(writer, "Content-Type: text/html; charset=utf-8");
                WriteLine(writer, "");
                WriteLine(writer, Content);
            }
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
