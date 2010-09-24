using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using WMPLib;
using System.Timers;
using System.Windows.Forms;

namespace Silmoon.Media
{
    //[
    //ComVisible(true),
    //Guid("88884578-81ea-4850-9911-13ba2d71000b"),
    //]
    public class WMPPlayer : Control, IDisposable
    {
        string _filePath;
        PlayerState _state;
        AxWMPLib.AxWindowsMediaPlayer _wmp = new AxWMPLib.AxWindowsMediaPlayer();
        WMPPlayerArgs _args = new WMPPlayerArgs();
        System.Timers.Timer t = new System.Timers.Timer();

        public event WMPPlayerHander OnWMPPlayerStateChange;
        public event WMPPlayerHander OnPlayContextChange;


        protected virtual void onWMPPlayerStateChange(WMPPlayerArgs args)
        {
            if (OnWMPPlayerStateChange != null)
            {
                OnWMPPlayerStateChange(this, args);
            }
        }
        protected virtual void onPlayContextChange(WMPPlayerArgs args)
        {
            if (OnPlayContextChange != null)
            {
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
                OnPlayContextChange(this, args);
            }
        }


        public double AllTime
        {
            get
            {
                return _wmp.currentMedia.duration;
            }
        }
        public string AllTimeString
        {
            get
            {
                return _wmp.currentMedia.durationString;
            }
        }
        public double NowTime
        {
            get
            {
                return _wmp.Ctlcontrols.currentPosition;
            }
            set
            {
                _wmp.Ctlcontrols.currentPosition = value;
            }
        }
        public string NowTimeString
        {
            get
            {
                return _wmp.Ctlcontrols.currentPositionString;
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public PlayerState State
        {
            get { return _state; }
            set { _state = value; }
        }
        public WMPPlayState WMPState
        {
            get { return _wmp.playState; }
        }
        public int PlayProgressPercentage
        {
            get
            { return ((int)(PlayerDoublePercentage * 100)); }
        }
        public double PlayerDoublePercentage
        {
            get
            {
                try
                { return (double)this.NowTime / (double)this.AllTime; }
                catch
                { return 0; }
            }
        }
        public double PlayerContextCycInterval
        {
            get { return t.Interval; }
            set { value = t.Interval; }
        }
        public string MuiscName
        {
            get { return _wmp.currentMedia.name; }
        }

        public WMPPlayer()
        {
            InitClass();
        }
        public WMPPlayer(string filePath)
        {
            _filePath = filePath;
            InitClass();
        }
        private void InitClass()
        {
            ((System.ComponentModel.ISupportInitialize)(this._wmp)).BeginInit();
            this.Controls.Add(this._wmp);
            ((System.ComponentModel.ISupportInitialize)(this._wmp)).EndInit();

            _wmp.settings.autoStart = false;
            _wmp.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(_wmp_PlayStateChange);
            _state = PlayerState.Null;
            t.Interval = 300;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            refreshArgs();
            onPlayContextChange(_args);
        }
        void _wmp_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (_wmp.playState == WMPPlayState.wmppsPlaying)
            { t.Start(); }
            else { t.Stop(); }
            refreshArgs();
            onWMPPlayerStateChange(_args);
        }


        public void Play()
        {
            _wmp.URL = _filePath;
            _wmp.Ctlcontrols.play();
            _state = PlayerState.Playing;
        }
        public void GoOn()
        {
            if (_state == PlayerState.Puase)
            {
                _wmp.Ctlcontrols.play();
                _state = PlayerState.Playing;
            }
        }
        public void FullScreenPlay()
        {
            _state = PlayerState.Playing;
        }
        public void Pause()
        {
            _wmp.Ctlcontrols.pause();
            _state = PlayerState.Puase;
        }
        public void Stop()
        {
            _wmp.Ctlcontrols.stop();
            _state = PlayerState.Close;
        }

        public void Close()
        {
            Stop();
            _wmp.Dispose();
            GC.Collect();
        }

        private void refreshArgs()
        {
            _args.WMPState = _wmp.playState;
            _args.DoublePercentage = PlayerDoublePercentage;
            _args.ProgressPercentage = ((int)(_args.DoublePercentage * 100));

            _args.NowTime = NowTime;
        }
        public new void Dispose()
        {
            t.Dispose();
            _wmp.Dispose();
            GC.Collect();
        }
    }
    public enum PlayerState
    {
        Null = 0,
        Loaded = 1,
        Playing = 2,
        Puase = 3,
        Stop = 4,
        Close = 5
    }
    public delegate void WMPPlayerHander(object sender, WMPPlayerArgs e);
    public class WMPPlayerArgs : System.EventArgs
    {
        public WMPPlayState WMPState;
        public int ProgressPercentage;
        public double DoublePercentage;
        public double NowTime;
    }
}