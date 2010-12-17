using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Silmoon.Web.Controls
{
    public class SingleUserLoginSessionControl
    {
        string _userName;
        string _password;
        int _userLevel = -1;
        LoginState _state;
        UserLimit _userLimit;
        int _sessionTimeout = 10;

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
                HttpContext.Current.Session["SmUserName"] = value;
                _userName = value;
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                HttpContext.Current.Session["SmPassword"] = value;
                _password = value;
            }
        }
        public int UserLevel
        {
            get { return _userLevel; }
            set
            {
                HttpContext.Current.Session["SmUserLevel"] = value;
                _userLevel = value;
            }
        }
        public LoginState State
        {
            get { return _state; }
            set
            {
                HttpContext.Current.Session["SmUserState"] = Convert.ToInt32(value);
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
            get { return (StateFlag)HttpContext.Current.Session["SmUserFlag"]; }
            set { HttpContext.Current.Session["SmUserFlag"] = value; }
        }
        public object UserObject
        {
            get { return HttpContext.Current.Session["SmUserObject"]; }
            set { HttpContext.Current.Session["SmUserObject"] = value; }
        }


        public SingleUserLoginSessionControl()
        {
            InitClass();
        }
        private void InitClass()
        {
            _state = LoginState.Null;
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
            if (HttpContext.Current.Session["SmClassSession"] == null)
            {
                _state = LoginState.Null;
            }
            else if (((bool)HttpContext.Current.Session["SmClassSession"]))
            {
                _userName = (string)HttpContext.Current.Session["SmUserName"];
                _password = (string)HttpContext.Current.Session["SmPassword"];
                _userLevel = (int)HttpContext.Current.Session["SmUserLevel"];
                _state = (LoginState)HttpContext.Current.Session["SmUserState"];
                _userLimit = (UserLimit)HttpContext.Current.Session["SmUserLimit"];
            }
            else
            {
                _state = LoginState.Null;
            }
        }
        public string ReadSession(string field)
        {
            return ((string)HttpContext.Current.Session[field]);
        }
        public void WriteSession(string field, string value)
        {
            HttpContext.Current.Session[field] = value;
        }
        public void DoLogin(string username, string password, int userLevel)
        {
            HttpContext.Current.Session.Timeout = _sessionTimeout;
            _userName = username;
            _password = password;
            _userLevel = userLevel;
            HttpContext.Current.Session["SmUserName"] = username;
            HttpContext.Current.Session["SmPassword"] = password;
            HttpContext.Current.Session["SmUserLevel"] = userLevel;
            HttpContext.Current.Session["SmUserState"] = Convert.ToInt32(LoginState.Login);
            HttpContext.Current.Session["SmClassSession"] = true;
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
            HttpContext.Current.Session["SmPassword"] = null;
            HttpContext.Current.Session["SmUserLevel"] = -1;
            HttpContext.Current.Session["SmUserState"] = Convert.ToInt32(LoginState.Logout);
            HttpContext.Current.Session["SmUserLimit"] = null;
            HttpContext.Current.Session["SmClassSession"] = null;

            if (UserLogout != null) UserLogout(this, EventArgs.Empty);
        }
        public void Clear()
        {
            HttpContext.Current.Session.Remove("SmPassword");
            HttpContext.Current.Session.Remove("SmUserLevel");
            HttpContext.Current.Session.Remove("SmUserName");
            HttpContext.Current.Session.Remove("SmUserState");
            HttpContext.Current.Session.Remove("SmUserLimit");
            HttpContext.Current.Session.Remove("SmUserFlag");
            HttpContext.Current.Session.Remove("SmUserObject");
        }
    }
    public enum LoginState
    {
        Null=0,
        None=1,
        Login=2,
        Logout=3,
    }
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
    public interface IObjectWithText
    {
        string Text { get; set; }
    }
    public class SmControl
    {
        public static Control FindControl(Control obj, string id)
        {
            obj.FindControl(id);
            return obj;
        }
    }
}