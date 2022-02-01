using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Handlers
{
    // necessary to enforce constructor
    public abstract class HandlerBase<T>
    {
        public HandlerBase(T parameters) { }
    }
}
