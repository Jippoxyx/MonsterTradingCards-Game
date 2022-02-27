using static MTCG.Http.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.Http {
    class Server {
        protected int port;
        TcpListener listener;

        public Server(int port) {
            this.port = port;
            listener = new TcpListener(IPAddress.Loopback, port);
        }

        public void Run() {
            
            listener.Start(5);
            Console.WriteLine("Press Escape to quit");

            while (true) {
                
                if (listener.Pending()) {
                    TcpClient s = listener.AcceptTcpClient();
                    Processor processor = new(s, this);
                    new Thread(processor.Process).Start();
                    Thread.Sleep(1);
                }

            }
        }
    }
}
