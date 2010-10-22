using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.Web;
using System.IO;

namespace Silmoon.Utility
{
    [Serializable()]
    public class DNSPod
    {
        protected internal string _baseAPIUri = "https://www.dnspod.com/";
        string _username;
        string _password;
        string _token;
        bool _isLogin = false;
        string _user_agent = "Unknown_SilmoonAssembly/0.0.0.0";
        public string _result = "";
        int _userID = 0;
        public bool BlackCase = false;
        public ArrayList APIHeaders = new ArrayList();

        /// <summary>
        /// 获取状态是否是已经登录
        /// </summary>
        public bool IsLogin
        {
            get { return _isLogin; }
            private set { _isLogin = value; }
        }
        /// <summary>
        /// 获取或设置当前用户名
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// 获取或设置用户密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// 设置暗箱操作使用的Token
        /// </summary>
        public string Token
        {
            get { return _token; }
            private set { _token = value; }
        }
        /// <summary>
        /// 获取或设置DNSPod API路径
        /// </summary>
        public string BaseAPIUri
        {
            get { return _baseAPIUri; }
            set { _baseAPIUri = value; }
        }
        /// <summary>
        /// 获取UA
        /// </summary>
        public string User_Agent
        {
            get { return _user_agent; }
        }
        /// <summary>
        /// 获取用户ID
        /// </summary>
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        /// <summary>
        /// 类型构造器
        /// </summary>
        public DNSPod()
        {

        }
        /// <summary>
        /// 类型构造器，指定使用的UA
        /// </summary>
        /// <param name="user_agent"></param>
        public DNSPod(string user_agent)
        {
            _user_agent = user_agent;
        }

        /// <summary>
        /// 向服务器发送POST请求
        /// </summary>
        /// <param name="apiField">API字段</param>
        /// <param name="data">POST数据</param>
        /// <returns>服务器返回的数据</returns>
        public string GetDNSPodServerXml(string apiField, string data)
        {
            WebClient _wclit = new WebClient();
            try
            {
                _wclit.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                _wclit.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                foreach (string s in APIHeaders)
                    _wclit.Headers.Add(s);
                _wclit.Headers.Add("User-Agent", "(SM)" + _user_agent);
                byte[] bytes = _wclit.UploadData(new Uri(_baseAPIUri + "API/" + apiField), Encoding.UTF8.GetBytes(data));
                _result = Encoding.UTF8.GetString(bytes);
                _wclit.Dispose();
                return Encoding.UTF8.GetString(bytes);
            }
            catch (WebException ex)
            {
                try
                {
                    File.WriteAllText(@"C:\DNSPodClientErrXmlString.xml.txt", DateTime.Now + "\r\n\r\n" + ex + "\r\n\r\n");
                    if (ex.Response != null)
                        File.AppendAllText(@"C:\DNSPodClientErrXmlString.xml.txt", new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                }
                catch { }
                _wclit.Dispose();
                return "!" + ex.ToString();
            }
        }

        /// <summary>
        /// 指定使用的UA
        /// </summary>
        /// <param name="user_agent"></param>
        public void SetUserAgent(string user_agent)
        {
            _user_agent = user_agent;
        }
        /// <summary>
        /// 指定DNSPod API路径
        /// </summary>
        /// <param name="apiUrl"></param>
        public void SetAPIUrl(string apiUrl)
        {
            _baseAPIUri = apiUrl;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserInfo Login(string username, string password)
        {
            BlackCase = false;

            _username = username;
            _password = password;
            return GetUserInfo();
        }
        /// <summary>
        /// 设置登录状态
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="login">是否设置为已经登录</param>
        public void SetLoginState(string username, string password, bool login)
        {
            BlackCase = false;
            _username = username;
            _password = password;
            _isLogin = login;
        }
        /// <summary>
        /// 黑箱操作登录
        /// </summary>
        /// <param name="token">TOKEN</param>
        /// <param name="validate">是否验证TOKEN</param>
        public UserInfo BlackCaseLogin(int userID, string token, bool validate = false)
        {
            BlackCase = true;
            UserID = userID;
            Token = token;
            if (validate)
            {
                return GetUserInfo();
            }
            return null;
        }
        /// <summary>
        /// 设置登录状态
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="login">是否设置为已经登录</param>
        public void SetBlackLoginState(int userID, string token, bool login)
        {
            BlackCase = true;
            UserID = userID;
            Token = token;
            _isLogin = login;
        }

        /// <summary>
        /// 获取用户的所有域名
        /// </summary>
        /// <returns></returns>
        public DomainInfo[] GetDomains()
        {
            return GetDomains(DomainListType.all);
        }
        /// <summary>
        /// 获取用户指定类型的域名
        /// </summary>
        /// <param name="type">域名所属类型</param>
        /// <returns></returns>
        public DomainInfo[] GetDomains(DomainListType type)
        {
            XmlDocument _xml = new XmlDocument();

            string resultXml = GetDNSPodServerXml("Domain.List", AuthConnection() + "&type=" + type.ToString());
            if (resultXml == "") return null;

            LoadXml(ref resultXml, ref _xml);
            ArrayList binfoArr = new ArrayList();
            XmlNode domainsNode = _xml["dnspod"]["domains"];

            foreach (XmlNode node in domainsNode)
            {
                DomainInfo newInfo = new DomainInfo();
                newInfo.DomainName = node["name"].InnerText;
                newInfo.State = SmString.StringToBool(node["status"].InnerText);
                newInfo.Records = int.Parse(node["records"].InnerText);
                newInfo.ID = int.Parse(node["id"].InnerText);
                newInfo.Grade = StringToGrade(node["grade"].InnerText);
                if (node["shared_from"] != null)
                    newInfo.ShardForm = node["shared_from"].InnerText;
                newInfo.Validate = DNSPodUnitValidate.FromDNSPod;
                binfoArr.Add(newInfo);
            }
            return (DomainInfo[])binfoArr.ToArray(typeof(DomainInfo));
        }
        /// <summary>
        /// 获取用户域名记录
        /// </summary>
        /// <param name="domainID">域名ID</param>
        /// <returns>域名记录数组</returns>
        public RecordInfo[] GetRecords(int domainID)
        {
            XmlDocument _xml = new XmlDocument();
            ArrayList array = new ArrayList();

            string resultXml = GetDNSPodServerXml("Record.List", AuthConnection() + "&domain_id=" + domainID);
            if (resultXml == "") return null;
            LoadXml(ref resultXml, ref _xml);

            if (_xml.GetElementsByTagName("code")[0].InnerText == "1" || _xml.GetElementsByTagName("code")[0].InnerText == "7")
            {
                XmlNode recordsNode = _xml["dnspod"]["records"];
                foreach (XmlNode node in recordsNode)
                {
                    RecordInfo newRecord = new RecordInfo();
                    newRecord.Enable = SmString.StringToBool(node["enabled"].InnerText);
                    newRecord.ID = int.Parse(node["id"].InnerText);
                    newRecord.Isp = node["line"].InnerText;
                    newRecord.MXLevel = int.Parse(node["mx"].InnerText);
                    newRecord.Subname = node["name"].InnerText;
                    newRecord.TTL = int.Parse(node["ttl"].InnerText);
                    newRecord.Type = DNSPod.StringToRecordType(node["type"].InnerText);
                    newRecord.Value = node["value"].InnerText;
                    newRecord.Validate = DNSPodUnitValidate.FromDNSPod;
                    array.Add(newRecord);
                }
            }
            return (RecordInfo[])array.ToArray(typeof(RecordInfo));
        }
        /// <summary>
        /// 从域名ID和记录ID获取记录信息
        /// </summary>
        /// <param name="domainID">域名ID</param>
        /// <param name="recordID">记录ID</param>
        /// <returns></returns>
        public RecordInfo GetRecord(int domainID, int recordID)
        {
            string s = GetDNSPodServerXml("Record.Info", AuthConnection() + "&domain_id=" + domainID + "&recordid=" + recordID);
            XmlDocument xml = new XmlDocument();
            LoadXml(ref s, ref xml);
            RecordInfo result = null;
            if (xml["dnspod"]["status"]["code"].InnerText == "1")
            {
                XmlNode node = xml["dnspod"]["record"];
                result = new RecordInfo();
                result.Enable = SmString.StringToBool(node["enabled"].InnerText);
                result.ID = int.Parse(node["id"].InnerText);
                result.Subname = node["sub_domain"].InnerText;
                result.Isp = node["record_line"].InnerText;
                result.Type = DNSPod.StringToRecordType(node["record_type"].InnerText);
                result.Validate = DNSPodUnitValidate.FromDNSPod;
                result.Value = node["value"].InnerText;
                result.MXLevel = int.Parse(node["mx"].InnerText);
                result.TTL = int.Parse(node["ttl"].InnerText);
            }
            return result;
        }
        /// <summary>
        /// 设置域名状态
        /// </summary>
        /// <param name="domainID">域名ID</param>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        public StateFlag SetDomainState(int domainID, bool enable)
        {
            XmlDocument _xml = new XmlDocument();

            StateFlag result = new StateFlag();
            string enableArgs = "";
            if (enable) enableArgs = "enable"; else enableArgs = "disable";
            string resultXml = GetDNSPodServerXml("Domain.Status", AuthConnection() + "&domain_id=" + domainID + "&status=" + enableArgs);
            if (resultXml == "")
            {
                result.DoubleStateFlag = false;
                result.IntStateFlag = -99;
                result.Message = "server error";
                return result;
            }
            LoadXml(ref resultXml, ref _xml);
            result.IntStateFlag = int.Parse(_xml.GetElementsByTagName("code")[0].InnerText);
            if (result.IntStateFlag == 1) result.DoubleStateFlag = true;
            result.Message = _xml.GetElementsByTagName("message")[0].InnerText;
            return result;
        }
        /// <summary>
        /// 添加域名
        /// </summary>
        /// <param name="domain">要添加的域名</param>
        /// <returns></returns>
        public StateFlag CreateDomain(string domain)
        {
            XmlDocument _xml = new XmlDocument();

            StateFlag result = new StateFlag();
            string resultXml = GetDNSPodServerXml("Domain.Create", AuthConnection() + "&domain=" + domain);
            if (resultXml == "")
            {
                result.DoubleStateFlag = false;
                result.IntStateFlag = -99;
                result.Message = "server error";
                return result;
            }
            LoadXml(ref resultXml, ref _xml);
            result.IntStateFlag = int.Parse(_xml.GetElementsByTagName("code")[0].InnerText);
            if (result.IntStateFlag == 1) result.DoubleStateFlag = true;
            result.Message = _xml.GetElementsByTagName("message")[0].InnerText;
            return result;
        }
        /// <summary>
        /// 删除域名
        /// </summary>
        /// <param name="domainID">要删除的域名ID</param>
        /// <returns></returns>
        public StateFlag RemoveDomain(int domainID)
        {
            XmlDocument _xml = new XmlDocument();

            StateFlag result = new StateFlag();
            string resultXml = GetDNSPodServerXml("Domain.Remove", AuthConnection() + "&domain_id=" + domainID);
            if (resultXml == "")
            {
                result.DoubleStateFlag = false;
                result.IntStateFlag = -99;
                result.Message = "server error";
                return result;
            }
            LoadXml(ref resultXml, ref _xml);
            result.IntStateFlag = int.Parse(_xml.GetElementsByTagName("code")[0].InnerText);
            if (result.IntStateFlag == 1) result.DoubleStateFlag = true;
            result.Message = _xml.GetElementsByTagName("message")[0].InnerText;
            return result;
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="record">域名记录信息</param>
        /// <param name="domainID">域名ID</param>
        /// <returns>状态集</returns>
        public StateFlag CreateRecord(RecordInfo record, int domainID)
        {
            XmlDocument _xml = new XmlDocument();

            StateFlag result = new StateFlag();
            string urlArgs = AuthConnection();
            urlArgs += "&domain_id=" + domainID;
            urlArgs += "&sub_domain=" + record.Subname;
            urlArgs += "&record_type=" + record.Type.ToString();
            urlArgs += "&record_line=" + record.Isp.ToString().ToLower();
            urlArgs += "&value=" + HttpUtility.UrlEncode(record.Value);
            urlArgs += "&mx=" + record.MXLevel;
            urlArgs += "&ttl=" + record.TTL;
            string resultXml = GetDNSPodServerXml("Record.Create", urlArgs);
            if (resultXml == "")
            {
                result.DoubleStateFlag = false;
                result.IntStateFlag = -99;
                result.Message = "server error";
                return result;
            }
            LoadXml(ref resultXml, ref _xml);
            if (_xml.GetElementsByTagName("id").Count == 0)
                result.ID = 0;
            else
                result.ID = int.Parse(_xml.GetElementsByTagName("id")[0].InnerText);

            result.IntStateFlag = int.Parse(_xml.GetElementsByTagName("code")[0].InnerText);
            if (result.IntStateFlag == 1) result.DoubleStateFlag = true;
            result.Message = _xml.GetElementsByTagName("message")[0].InnerText;
            return result;
        }
        /// <summary>
        /// 编辑域名记录
        /// </summary>
        /// <param name="record">域名记录信息</param>
        /// <param name="domainID">域名ID</param>
        /// <returns>状态集</returns>
        public StateFlag ModifyRecord(RecordInfo record, int domainID)
        {
            XmlDocument _xml = new XmlDocument();

            StateFlag result = new StateFlag();
            string urlArgs = AuthConnection();
            urlArgs += "&domain_id=" + domainID;
            urlArgs += "&record_id=" + record.ID;
            urlArgs += "&sub_domain=" + record.Subname;
            urlArgs += "&record_type=" + record.Type.ToString();
            urlArgs += "&record_line=" + record.Isp.ToString().ToLower();
            urlArgs += "&value=" + HttpUtility.UrlEncode(record.Value);
            urlArgs += "&mx=" + record.MXLevel;
            urlArgs += "&ttl=" + record.TTL;
            string resultXml = GetDNSPodServerXml("Record.Modify", urlArgs);
            if (resultXml == "")
            {
                result.DoubleStateFlag = false;
                result.IntStateFlag = -99;
                result.Message = "server error";
                return result;
            }
            LoadXml(ref resultXml, ref _xml);
            result.IntStateFlag = int.Parse(_xml.GetElementsByTagName("code")[0].InnerText);
            if (result.IntStateFlag == 1) result.DoubleStateFlag = true;
            result.Message = _xml.GetElementsByTagName("message")[0].InnerText;
            return result;
        }
        /// <summary>
        /// 删除域名记录
        /// </summary>
        /// <param name="domainID">域名ID</param>
        /// <param name="recordID">记录ID</param>
        /// <returns>结果集</returns>
        public StateFlag RemoveRecord(int domainID, int recordID)
        {
            XmlDocument _xml = new XmlDocument();

            StateFlag result = new StateFlag();
            string resultXml = GetDNSPodServerXml("Record.Remove", AuthConnection() + "&record_id=" + recordID + "&domain_id=" + domainID);
            if (resultXml == "")
            {
                result.DoubleStateFlag = false;
                result.IntStateFlag = -99;
                result.Message = "server error";
                return result;
            }

            LoadXml(ref resultXml, ref _xml);
            result.IntStateFlag = int.Parse(_xml.GetElementsByTagName("code")[0].InnerText);
            if (result.IntStateFlag == 1) result.DoubleStateFlag = true;
            result.Message = _xml.GetElementsByTagName("message")[0].InnerText;
            return result;
        }
        /// <summary>
        /// 设置域名记录状态
        /// </summary>
        /// <param name="domainID">域名ID</param>
        /// <param name="recordID">记录ID</param>
        /// <param name="enable">是否启用</param>
        /// <returns>结果集</returns>
        public StateFlag SetRecordState(int domainID, int recordID, bool enable)
        {
            XmlDocument _xml = new XmlDocument();

            StateFlag result = new StateFlag();
            string enableArgs = "";
            if (enable) enableArgs = "enable"; else enableArgs = "disable";
            string resultXml = GetDNSPodServerXml("Record.Status", AuthConnection() + "&record_id=" + recordID + "&domain_id=" + domainID + "&status=" + enableArgs);
            LoadXml(ref resultXml, ref _xml);
            result.IntStateFlag = int.Parse(_xml.GetElementsByTagName("code")[0].InnerText);
            if (result.IntStateFlag == 1) result.DoubleStateFlag = true;
            result.Message = _xml.GetElementsByTagName("message")[0].InnerText;
            return result;
        }
        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="domainID">域名ID</param>
        /// <returns>域名信息</returns>
        public DomainInfo GetDomainInfo(int domainID)
        {
            DomainInfo[] domains = GetDomains();
            foreach (DomainInfo domain in domains)
            {
                if (domain.ID == domainID)
                    return domain;
            }
            return null;
        }
        /// <summary>
        /// 获取域名信息从域名信息数组中
        /// </summary>
        /// <param name="domainID">域名ID</param>
        /// <param name="domains">域名信息数组</param>
        /// <returns>域名信息</returns>
        public DomainInfo GetDomainInfo(int domainID, DomainInfo[] domains)
        {
            foreach (DomainInfo domain in domains)
            {
                if (domain.ID == domainID)
                    return domain;
            }
            return null;
        }
        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns>域名信息</returns>
        public DomainInfo GetDomainInfo(string domain)
        {
            DomainInfo[] domains = GetDomains();
            foreach (DomainInfo domainInfo in domains)
            {
                if (domainInfo.DomainName.ToLower() == domain.ToLower())
                    return domainInfo;
            }
            return null;
        }
        /// <summary>
        /// 获取域名信息从域名信息数组中
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="domains">域名信息数组</param>
        /// <returns>域名信息</returns>
        public DomainInfo GetDomainInfo(string domain, DomainInfo[] domains)
        {
            foreach (DomainInfo domainInfo in domains)
            {
                if (domainInfo.DomainName.ToLower() == domain.ToLower())
                    return domainInfo;
            }
            return null;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetUserInfo()
        {
            XmlDocument _xml = new XmlDocument();

            UserInfo userInfo = new UserInfo();
            string resultXml = GetDNSPodServerXml("User.Info", AuthConnection());
            if (resultXml == "")
                userInfo.StateCode = -99;
            else
            {
                LoadXml(ref resultXml, ref _xml);
                userInfo.StateCode = int.Parse(_xml["dnspod"]["status"]["code"].InnerText);
                userInfo.Message = _xml["dnspod"]["status"]["message"].InnerText;
                if (userInfo.StateCode == 1)
                {
                    userInfo.LoginOK = true;
                    IsLogin = true;
                    userInfo.UserID = int.Parse(_xml["dnspod"]["user"]["id"].InnerText);
                    userInfo.Username = _xml["dnspod"]["user"]["email"].InnerText;
                    Username = userInfo.Username;
                    UserID = userInfo.UserID;
                }
                else
                {
                    userInfo.LoginOK = false;
                    IsLogin = false;
                }
            }

            return userInfo;
        }
        /// <summary>
        /// 获取DNSPod一次性密码
        /// </summary>
        /// <returns>一次性密码</returns>
        public string GetOncePassword()
        {
            XmlDocument _xml = new XmlDocument();

            string resultXml = GetDNSPodServerXml("Login.Key", AuthConnection());
            LoadXml(ref resultXml, ref _xml);
            if (_xml.GetElementsByTagName("code")[0].InnerText == "1")
            {
                return _xml.GetElementsByTagName("key")[0].InnerText;
            } return "";
        }
        /// <summary>
        /// 获取域名的可用线路
        /// </summary>
        /// <param name="domainInfo">域名信息</param>
        /// <returns></returns>
        public string[] GetDomainNetworkType(DomainInfo domainInfo)
        {
            string s = GetDNSPodServerXml("Record.Line", AuthConnection() + "&domain_grade=" + domainInfo.Grade);
            XmlDocument xml = new XmlDocument();
            ArrayList array = new ArrayList();

            LoadXml(ref s, ref xml);
            if (xml["dnspod"]["status"]["code"].InnerText == "1")
            {
                XmlNode xmlNode = xml["dnspod"]["lines"];
                foreach (XmlNode item in xmlNode)
                {
                    array.Add(item.InnerText);
                }
            }
            return (string[])array.ToArray(typeof(string));
        }
        /// <summary>
        /// Load Xml from string
        /// </summary>
        /// <param name="xmlString">string</param>
        /// <param name="xmlDoc">xml object</param>
        public void LoadXml(ref string xmlString, ref XmlDocument xmlDoc)
        {
            try
            {
                xmlDoc.LoadXml(xmlString);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public static RecordType StringToRecordType(string type)
        {
            switch (type.ToLower())
            {
                case "a":
                    return RecordType.A;
                case "cname":
                    return RecordType.CNAME;
                case "mx":
                    return RecordType.MX;
                case "url":
                    return RecordType.URL;
                case "ns":
                    return RecordType.NS;
                case "txt":
                    return RecordType.TXT;
                case "aaaa":
                    return RecordType.AAAA;
                case "cn":
                    return RecordType.CNAME;
                case "v6":
                    return RecordType.AAAA;
                default:
                    return RecordType.A;
            }
        }
        public static DomainGrade StringToGrade(string grade)
        {
            switch (grade.ToLower())
            {
                case "d_free":
                    return DomainGrade.D_Free;
                case "d_express":
                    return DomainGrade.D_Express;
                case "d_plus":
                    return DomainGrade.D_Plus;
                case "d_extra":
                    return DomainGrade.D_Extra;
                case "d_expert":
                    return DomainGrade.D_Expert;
                case "d_ultra":
                    return DomainGrade.D_Ultra;
                default:
                    return DomainGrade.D_Free;
            }
        }

        public string AuthConnection()
        {
            if (BlackCase)
            {
                return "login_id=" + UserID + "&login_token=" + Token + "&format=xml";
            }
            else
            {
                return "login_email=" + HttpUtility.UrlEncode(_username) + "&login_password=" + HttpUtility.UrlEncode(_password) + "&format=xml";
            }
        }

        public enum DomainListType
        {
            all, share, mine
        }
    }
    public class DomainInfo
    {
        public string DomainName;
        public bool State;
        public int Records;
        public DomainGrade Grade;
        public int ID;
        public string ShardForm;
        public DNSPodUnitValidate Validate = DNSPodUnitValidate.New;
    }
    public enum DomainGrade
    {
        D_Free = 0,
        D_Express = 1,
        D_Plus = 2,
        D_Extra = 3,
        D_Expert = 4,
        D_Ultra = 5
    }
    public class RecordInfo
    {
        public int ID;
        public string Subname;
        public string Isp = "默认";
        public RecordType Type;
        public int TTL = 3600;
        public string Value;
        public int MXLevel = 5;
        public bool Enable = true;
        public DNSPodUnitValidate Validate = DNSPodUnitValidate.New;
        public string Note;
        public RecordInfo() { }
        public RecordInfo(string subname, string isp, RecordType type, string value)
        {
            Subname = subname;
            Isp = isp;
            Type = type;
            Value = value;
        }
    }
    public class UserInfo
    {
        public string Username;
        public int UserID;
        public int StateCode = -99;
        public bool LoginOK = false;
        public string Message;
    }
    public enum AgentGrade
    {
        unknown = 0,
        bronze = 1,
        silver = 2,
        gold = 3,
        diamond = 4,
    }

    public enum RecordType
    {
        A = 1,
        CNAME = 2,
        MX = 3,
        URL = 4,
        NS = 5,
        TXT = 6,
        AAAA = 7,
    }
    public enum DNSPodUnitValidate
    {
        FromDNSPod,
        Invalid,
        New,
    }
}