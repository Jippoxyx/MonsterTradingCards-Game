using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Handlers
{
    // All paths need to implement the handler interface
    class IHandler
    {
        /// <summary>
        /// processes task
        /// </summary>
        /// <returns>answer to handle task</returns>
        public string Handle();
    }
}
