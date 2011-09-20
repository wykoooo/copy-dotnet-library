using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon
{
    public abstract class RunningAble : IRunningAble, IDisposable
    {
        public RunningAble()
        {

        }
        public event OperateHandler OnStart;
        public event OperateHandler OnStop;
        public event OperateHandler OnSuspend;
        public event OperateHandler OnResume;

        #region IRunningAble 成员
        private RunningState _runningState = RunningState.Stopped;
        public RunningState RunningState
        {
            get { return _runningState; }
            private set { _runningState = value; }
        }
        public void StartA()
        {
            Start();
        }
        public bool Start()
        {
            bool success = RunningState == MySilmoon.RunningState.Stopped;
            RunningState = MySilmoon.RunningState.Running;
            bool success2 = OnStart(success);
            if (!success2 || !success) RunningState = RunningState.Stopped;
            return success;
        }
        public bool Stop()
        {
            MySilmoon.RunningState runstate = RunningState;
            bool success = RunningState != MySilmoon.RunningState.Stopped;
            RunningState = MySilmoon.RunningState.Stopped;
            bool success2 = OnStop(success);
            if (!success2 || !success) RunningState = runstate;
            return success;
        }
        public bool Suspend()
        {
            bool success = RunningState == MySilmoon.RunningState.Running;
            RunningState = MySilmoon.RunningState.Suspended;
            bool success2 = OnSuspend(success);
            if (!success2 || !success) RunningState = RunningState.Running;
            return success;
        }
        public bool Resume()
        {
            bool success = RunningState == MySilmoon.RunningState.Suspended;
            RunningState = MySilmoon.RunningState.Running;
            bool success2 = OnResume(success);
            if (!success2 || !success) RunningState = RunningState.Suspended;
            return success;
        }
        #endregion

        #region IDisposable 成员

        public virtual void Dispose()
        {
            try
            { OnStop(true); }
            catch { }
            Stop();
        }

        #endregion

        public delegate bool OperateHandler(bool success);
    }
}
