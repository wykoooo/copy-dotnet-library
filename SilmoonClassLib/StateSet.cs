using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public class StateSet
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public StateSet Set(int code, string message)
        {
            Code = code;
            Message = message;
            return this;
        }
    }
}
