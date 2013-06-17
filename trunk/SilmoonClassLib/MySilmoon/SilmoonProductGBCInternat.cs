using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using Silmoon.MySilmoon.Instance;
using Silmoon.Threading;

namespace Silmoon.MySilmoon
{
    /// <summary>
    /// 对银月产品公共库公共属性进行重用
    /// </summary>
    public class SilmoonProductGBCInternat : RunningAble, ISilmoonProductGBCInternat
    {
        private string _productString = "NULL";
        private int _revision = 0;
        private RunningState _runningState = RunningState.Stopped;
        private bool _initProduceInfo = false;

        public event OutputTextMessageHandler OnOutputTextMessage;
        public event OutputTextMessageHandler OnInputTextMessage;
        public event ThreadExceptionEventHandler OnThreadException;
        public event Action<VersionResult> OnValidateLicense;

        /// <summary>
        /// 标识产品名称字符串
        /// </summary>
        public string ProductString
        {
            get { return _productString; }
            set { _productString = value; }
        }
        /// <summary>
        /// 产品发布序号
        /// </summary>
        public int Revision
        {
            get { return _revision; }
            set { _revision = value; }
        }

        public SilmoonProductGBCInternat()
        {

        }

        public void onOutputText(string message)
        {
            onOutputText(message, 0);
        }
        public void onOutputText(string message, int flag)
        {
            if (OnOutputTextMessage != null) OnOutputTextMessage(message, flag);
        }
        public void onInputText(string message)
        {
            onInputText(message, 0);
        }
        public void onInputText(string message, int flag)
        {
            if (OnInputTextMessage != null) OnInputTextMessage(message, flag);
        }
        public void onThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (OnThreadException != null) OnThreadException(sender, e);
        }

        public void AsyncValidateLicense()
        {
            Threads.ExecAsync(delegate()
            {
                if (OnValidateLicense != null)
                {
                    var result = MyConfigure.GetVersion(_productString);
                    OnValidateLicense(result);
                }
            });
        }


        /// <summary>
        /// 初始化公共属性
        /// </summary>
        /// <param name="productString">指定产品名称字符串</param>
        /// <param name="revision">指定发布产品的序号</param>
        public bool InitProductInfo(string productString, int revision)
        {
            if (!_initProduceInfo)
            {
                _productString = productString;
                _revision = revision;
                return true;
            }
            else
                return false;
        }


    }
    public delegate void OutputTextMessageHandler(string message, int flag);
}