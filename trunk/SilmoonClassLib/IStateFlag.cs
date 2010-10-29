using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public interface IStateFlag
    {
        string Message
        {
            get;
            set;
        }
        int IntFlag
        {
            get;
            set;
        }
        bool Error
        {
            get;
            set;
        }
        object ObjectReferer
        {
            get;
            set;
        }
    }
}
