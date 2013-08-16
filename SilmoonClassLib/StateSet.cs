using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public class StateSet<T>
    {
        public T State { get; set; }
        public string Message { get; set; }
        public object UserState { get; set; }

        public StateSet<T> Set<TO>(T state, string message, TO userState)
        {
            State = state;
            Message = message;
            UserState = userState;
            return this;
        }
        public StateSet<T> Set(T state, string message)
        {
            State = state;
            Message = message;
            return this;
        }

    }

}
