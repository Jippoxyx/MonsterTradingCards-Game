using MTCG.Http;
using System;

namespace MTCG
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer newServer = new HttpServer(10001);
            newServer.Run();
        }
    }
}
