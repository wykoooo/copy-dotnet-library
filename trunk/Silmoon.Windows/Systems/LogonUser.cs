using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Windows.Win32.API.APIStructs;

namespace Silmoon.Windows.Systems
{
    public class LogonUser
    {
        #region 用户信息字段
        private string userName;
        private string clientUserName;
        private string sessionType;
        private WTS_CONNECTSTATE_CLASS connectState;

        /// <summary>
        /// 连接状态
        /// </summary>
        public WTS_CONNECTSTATE_CLASS ConnectState
        {
            get { return connectState; }
            set { connectState = value; }
        }
        /// <summary>
        /// 会话类型
        /// </summary>
        public string SessionType
        {
            get { return sessionType; }
            set { sessionType = value; }
        }
        /// <summary>
        /// 客户端名
        /// </summary>
        public string ClientUserName
        {
            get { return clientUserName; }
            set { clientUserName = value; }
        }
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }


        #endregion
    }
}
