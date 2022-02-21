using System;
using System.Collections.Generic;
using System.IO;

namespace MTCG.BL.Service
{



    public sealed class Logger
    {
        //thread-safe
        private static Logger instance = null;
        private static readonly object singletonLock = new object();
        private readonly List<Logger> logs = new List<Logger>();

        Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                lock (singletonLock)
                {
                    if (instance == null)
                    {
                        instance = new Logger();
                    }
                    return instance;
                }
            }
        }
            
        public void AddBattleLog(string log)
        {

        }

        public void PrintBattleLogs()
        {

        }

        public void AddTransactionLog(string log)
        {
        
        }

        public void CreateTransactionsLogs()
        {
      
        }

    }
}
