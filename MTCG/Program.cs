﻿using MTCG.DAL.Database;
using MTCG.Http;
using System;

namespace MTCG
{
    class Program
    {
        static void Main(string[] args)
        {   
            Server newServer = new Server(10001);
            newServer.Run();
        }
    }
}
