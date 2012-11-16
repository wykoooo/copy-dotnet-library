using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Silmoon.Web.Controls
{
    public abstract class SingleUserLoginSessionControl
    {
        string _userName;
        string _password;
        int _userLevel = -1;
        LoginState _state;
        UserLimit _userLimit;
        int _sessionTimeout = 20;
        RSACryptoServiceProvider rsa = null;
        string loginStateDomain = null;
        DateTime loginStateTimeout = new DateTime();

        public int SessionTimeout
        {
            get { return _sessionTimeout; }
            set { _sessionTimeout = value; }
        }
        public event EventHandler UserLogin;
        public event EventHandler UserLogout;

        public string UserName
        {
            get { return _userName; }
            set
            {
                HttpContext.Current.Session["___silmoon_username"] = value;
                _userName = value;
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                HttpContext.Current.Session["___silmoon_password"] = value;
                _password = value;
            }
        }
        public int UserLevel
        {
            get { return _userLevel; }
            set
            {
                HttpContext.Current.Session["___silmoon_level"] = value;
                _userLevel = value;
            }
        }
        public LoginState State
        {
            get { return _state; }
            set
            {
                HttpContext.Current.Session["___silmoon_state"] = Convert.ToInt32(value);
                _state = value;
            }
        }
        public UserLimit UserLevelLimit
        {
            get { return _userLimit; }
            set { _userLimit = value; }
        }
        public StateFlag UserFlag
        {
            get { return (StateFlag)HttpContext.Current.Session["___silmoon_userflag"]; }
            set { HttpContext.Current.Session["___silmoon_userflag"] = value; }
        }
        public object UserObject
        {
            get { return HttpContext.Current.Session["___silmoon_object"]; }
            set { HttpContext.Current.Session["___silmoon_object"] = value; }
        }
        public RSACryptoServiceProvider RSACookiesCrypto
        {
            get { return rsa; }
            set { rsa = value; }
        }
        public string LoginStateDomain
        {
            get { return loginStateDomain; }
            set { loginStateDomain = value; }
        }
        public DateTime LoginStateTimeout
        {
            get { return loginStateTimeout; }
            set { loginStateTimeout = value; }
        }


        public SingleUserLoginSessionControl()
        {
            InitClass();
        }
        private void InitClass()
        {
            _state = LoginState.None;
            _userLimit = new UserLimit(_userName, _password);
        }
        private void check_sessionOfLogin()
        {
            if (_state != LoginState.Login)
            {
                throw new Exception("获取会话信息错误，用户没有登陆，或者会话已经无效！");
            }
        }

        public void ReadSession()
        {
            if (HttpContext.Current.Session["___silmoon_login_session"] != null && SmString.StringToBool(HttpContext.Current.Session["___silmoon_login_session"].ToString()))
            {
                _userName = (string)HttpContext.Current.Session["___silmoon_username"];
                _password = (string)HttpContext.Current.Session["___silmoon_password"];
                _userLevel = (int)HttpContext.Current.Session["___silmoon_level"];
                _state = (LoginState)HttpContext.Current.Session["___silmoon_state"];
                _userLimit = (UserLimit)HttpContext.Current.Session["___silmoon_userlimit"];
            }
            else
            {
                if (!LoginFromCookie())
                    _state = LoginState.None;
            }
        }
        public object ReadSession(string field)
        {
            return HttpContext.Current.Session[field];
        }
        public void WriteSession(string field, string value)
        {
            HttpContext.Current.Session[field] = value;
        }

        bool LoginFromCookie()
        {
            if (HttpContext.Current.Request.Cookies["___silmoon_user_session"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["___silmoon_user_session"].Value))
            {
                try
                {
                    byte[] data = Convert.FromBase64String(HttpContext.Current.Request.Cookies["___silmoon_user_session"].Value);
                    data = rsa.Decrypt(data, true);
                    string username = Encoding.Default.GetString(data, 2, BitConverter.ToInt16(data, 0));
                    string password = Encoding.Default.GetString(data, BitConverter.ToInt16(data, 0) + 4, data[BitConverter.ToInt16(data, 0) + 2]);
                    return CookieRelogin(username, password);
                }
                catch
                {
                    HttpContext.Current.Response.Cookies.Remove("___silmoon_user_session");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        bool LoginCrossLoginCookie()
        {
            if (HttpContext.Current.Request.Cookies["___silmoon_cross_session"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["___silmoon_cross_session"].Value))
            {
                try
                {
                    byte[] data = Convert.FromBase64String(HttpContext.Current.Request.Cookies["___silmoon_cross_session"].Value);
                    data = rsa.Decrypt(data, true);
                    string username = Encoding.Default.GetString(data, 2, BitConverter.ToInt16(data, 0));
                    string password = Encoding.Default.GetString(data, BitConverter.ToInt16(data, 0) + 4, data[BitConverter.ToInt16(data, 0) + 2]);
                    bool result = CrossLogin(username, password);
                    HttpContext.Current.Response.Cookies.Remove("___silmoon_cross_session");
                    return result;
                }
                catch
                {
                    HttpContext.Current.Response.Cookies.Remove("___silmoon_cross_session");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public abstract bool CookieRelogin(string username, string password);
        public abstract bool CrossLogin(string username, string password);

        public void WriteCookie()
        {
            if (rsa != null)
            {
                byte[] usernameData = Encoding.Default.GetBytes(_userName);
                byte[] passwordData = Encoding.Default.GetBytes(_password);
                byte[] data = new byte[4 + usernameData.Length + passwordData.Length];

                Array.Copy(BitConverter.GetBytes((short)usernameData.Length), 0, data, 0, 2);
                Array.Copy(usernameData, 0, data, 2, usernameData.Length);
                Array.Copy(BitConverter.GetBytes((short)passwordData.Length), 0, data, usernameData.Length + 2, 2);
                Array.Copy(passwordData, 0, data, usernameData.Length + 4, passwordData.Length);

                data = rsa.Encrypt(data, true);

                string sessionInfo = Convert.ToBase64String(data);
                HttpContext.Current.Response.Cookies["___silmoon_user_session"].Value = sessionInfo;
                if (loginStateDomain != null)
                    HttpContext.Current.Response.Cookies["___silmoon_user_session"].Domain = loginStateDomain;
                HttpContext.Current.Response.Cookies["___silmoon_user_session"].Expires = loginStateTimeout;
            }
        }
        public void WriteCrossLoginCookie(string domain = null)
        {
            if (rsa != null)
            {
                byte[] usernameData = Encoding.Default.GetBytes(_userName);
                byte[] passwordData = Encoding.Default.GetBytes(_password);
                byte[] data = new byte[4 + usernameData.Length + passwordData.Length];

                Array.Copy(BitConverter.GetBytes((short)usernameData.Length), 0, data, 0, 2);
                Array.Copy(usernameData, 0, data, 2, usernameData.Length);
                Array.Copy(BitConverter.GetBytes((short)passwordData.Length), 0, data, usernameData.Length + 2, 2);
                Array.Copy(passwordData, 0, data, usernameData.Length + 4, passwordData.Length);

                data = rsa.Encrypt(data, true);

                string sessionInfo = Convert.ToBase64String(data);
                HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Value = sessionInfo;
                if (domain != null)
                    HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Domain = domain;
                HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Expires = DateTime.Now.AddSeconds(5);
            }
        }

        public void DoLogin(string username, string password, int userLevel)
        {
            HttpContext.Current.Session.Timeout = _sessionTimeout;
            _userName = username;
            _password = password;
            _userLevel = userLevel;
            HttpContext.Current.Session["___silmoon_username"] = username;
            HttpContext.Current.Session["___silmoon_password"] = password;
            HttpContext.Current.Session["___silmoon_level"] = userLevel;
            HttpContext.Current.Session["___silmoon_state"] = (int)LoginState.Login;
            HttpContext.Current.Session["___silmoon_login_session"] = true;
            _state = LoginState.Login;

            if (UserLogin != null) UserLogin(this, EventArgs.Empty);
        }
        public void DoLogin()
        {
            DoLogin(_userName, _password, _userLevel);
        }
        public void DoLogout()
        {

            _state = LoginState.Logout;
            HttpContext.Current.Session["___silmoon_password"] = null;
            HttpContext.Current.Session["___silmoon_level"] = -1;
            HttpContext.Current.Session["___silmoon_state"] = Convert.ToInt32(LoginState.Logout);
            HttpContext.Current.Session["___silmoon_userlimit"] = null;
            HttpContext.Current.Session["___silmoon_login_session"] = null;

            if (UserLogout != null) UserLogout(this, EventArgs.Empty);
        }
        public void Clear()
        {
            HttpContext.Current.Response.Cookies["___silmoon_user_session"].Expires = DateTime.Now.AddYears(-10);

            HttpContext.Current.Session.Remove("___silmoon_username");
            HttpContext.Current.Session.Remove("___silmoon_password");
            HttpContext.Current.Session.Remove("___silmoon_level");
            HttpContext.Current.Session.Remove("___silmoon_state");
            HttpContext.Current.Session.Remove("___silmoon_userlimit");
            HttpContext.Current.Session.Remove("___silmoon_userflag");
            HttpContext.Current.Session.Remove("___silmoon_object");
            HttpContext.Current.Session.Remove("___silmoon_login_session");
        }
    }
    [Serializable]
    public enum LoginState
    {
        None=0,
        Login=1,
        Logout=-1,
    }
    [Serializable]
    public class UserLimit
    {
        string _userLevelString;
        string _username;
        string _password;

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string UserLevelString
        {
            get { return _userLevelString; }
            set { _userLevelString = value; }
        }

        public UserLimit()
        {

        }
        public UserLimit(string username, string password)
        {
            _username = username;
            _password = password;
        }
        public void Clear()
        {
            HttpContext.Current.Session["SmUserLimit"] = null;
        }

        public object GetUserLevelStringArrayItem(int i)
        {
            object o = (object)_userLevelString.Split(new string[] { "|" }, StringSplitOptions.None)[i];
            return o;
        }

    }
}