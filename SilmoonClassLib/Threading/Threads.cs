using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Silmoon.Threading
{
    public class Threads
    {
        public static Thread ExecAsync(ThreadStart start)
        {
            Thread _th = new Thread(start);
            _th.IsBackground = true;
            _th.Start();
            return _th;
        }
        public static Thread ExecAsync(ThreadStart start, ThreadExceptionEventHandler onExceptionCallback)
        {
            internalProtectExecuteClass executeClass = new internalProtectExecuteClass(start, onExceptionCallback);
            Thread _th = new Thread(executeClass.Execute);
            _th.IsBackground = true;
            _th.Start();
            return _th;
        }

        class internalProtectExecuteClass
        {
            event ThreadStart _start;
            event ThreadExceptionEventHandler _onExceptionCallback;
            public internalProtectExecuteClass(ThreadStart start, ThreadExceptionEventHandler onExceptionCallback)
            {
                _start = start;
                _onExceptionCallback = onExceptionCallback;
            }
            public void Execute()
            {
                try
                {
                    if (_start != null)
                        _start();
                    else throw (ThreadStartException)new SystemException("没有执行委托代码");
                }
                catch (Exception ex)
                {
                    if (_onExceptionCallback != null)
                        _onExceptionCallback(this, new ThreadExceptionEventArgs(ex));
                }
            }
        }
        public delegate bool StatusStart();
    }
}
