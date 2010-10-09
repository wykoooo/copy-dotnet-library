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
                    newRecord.Isp = DNSPod.StringToISP(node["line"].InnerText);
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

        public static ISP StringToISP(string isp)
        {

            switch (isp.ToLower())
            {
                case "tel":
                    return ISP.TEL;
                case "cnc":
                    return ISP.CNC;
                case "cuc":
                    return ISP.CNC;
                case "edu":
                    return ISP.EDU;
                case "cmc":
                    return ISP.CMC;
                case "foreign":
                    return ISP.FOREIGN;
                case "for":
                    return ISP.FOREIGN;
                case "电信":
                    return ISP.TEL;
                case "联通":
                    return ISP.CNC;
                case "网通":
                    return ISP.CNC;
                case "教育":
                    return ISP.EDU;
                case "移动":
                    return ISP.CMC;
                case "海外":
                    return ISP.FOREIGN;
                case "anhui_tel":
                    return ISP.anhui_tel;
                case "anhui_cnc":
                    return ISP.anhui_cnc;
                case "aomen":
                    return ISP.aomen;
                case "beijing_tel":
                    return ISP.beijing_tel;
                case "beijing_cnc":
                    return ISP.beijing_cnc;
                case "chongqing_tel":
                    return ISP.chongqing_tel;
                case "chongqing_cnc":
                    return ISP.chongqing_cnc;
                case "fujian_tel":
                    return ISP.fujian_tel;
                case "fujian_cnc":
                    return ISP.fujian_cnc;
                case "gansu_tel":
                    return ISP.gansu_tel;
                case "gansu_cnc":
                    return ISP.gansu_cnc;
                case "guangdong_tel":
                    return ISP.guangdong_tel;
                case "guangdong_cnc":
                    return ISP.guangdong_cnc;
                case "guangxi_tel":
                    return ISP.guangxi_tel;
                case "guangxi_cnc":
                    return ISP.guangxi_cnc;
                case "guizhou_tel":
                    return ISP.guizhou_tel;
                case "guizhou_cnc":
                    return ISP.guizhou_cnc;
                case "hainan_tel":
                    return ISP.hainan_tel;
                case "hainan_cnc":
                    return ISP.hainan_cnc;
                case "hebei_tel":
                    return ISP.hebei_tel;
                case "hebei_cnc":
                    return ISP.hebei_cnc;
                case "henan_tel":
                    return ISP.henan_tel;
                case "henan_cnc":
                    return ISP.henan_cnc;
                case "heilongjiang_tel":
                    return ISP.heilongjiang_tel;
                case "heilongjiang_cnc":
                    return ISP.heilongjiang_cnc;
                case "hubei_tel":
                    return ISP.hubei_tel;
                case "hubei_cnc":
                    return ISP.hubei_cnc;
                case "hunan_tel":
                    return ISP.hunan_tel;
                case "hunan_cnc":
                    return ISP.hunan_cnc;
                case "jilin_tel":
                    return ISP.jilin_tel;
                case "jilin_cnc":
                    return ISP.jilin_cnc;
                case "jiangsu_tel":
                    return ISP.jiangsu_tel;
                case "jiangsu_cnc":
                    return ISP.jiangsu_cnc;
                case "jiangxi_tel":
                    return ISP.jiangxi_tel;
                case "jiangxi_cnc":
                    return ISP.jiangxi_cnc;
                case "liaoning_tel":
                    return ISP.liaoning_tel;
                case "liaoning_cnc":
                    return ISP.liaoning_cnc;
                case "neimeng_tel":
                    return ISP.neimeng_tel;
                case "neimeng_cnc":
                    return ISP.neimeng_cnc;
                case "ningxia_tel":
                    return ISP.ningxia_tel;
                case "ningxia_cnc":
                    return ISP.ningxia_cnc;
                case "qinghai_tel":
                    return ISP.qinghai_tel;
                case "qinghai_cnc":
                    return ISP.qinghai_cnc;
                case "shandong_tel":
                    return ISP.shandong_tel;
                case "shandong_cnc":
                    return ISP.shandong_cnc;
                case "shanxi_tel":
                    return ISP.shanxi_tel;
                case "shanxi_cnc":
                    return ISP.shanxi_cnc;
                case "shaanxi_tel":
                    return ISP.shaanxi_tel;
                case "shaanxi_cnc":
                    return ISP.shaanxi_cnc;
                case "shanghai_tel":
                    return ISP.shanghai_tel;
                case "shanghai_cnc":
                    return ISP.shanghai_cnc;
                case "sichuan_tel":
                    return ISP.sichuan_tel;
                case "sichuan_cnc":
                    return ISP.sichuan_cnc;
                case "taiwan":
                    return ISP.taiwan;
                case "tianjin_tel":
                    return ISP.tianjin_tel;
                case "tianjin_cnc":
                    return ISP.tianjin_cnc;
                case "xizang_tel":
                    return ISP.xizang_tel;
                case "xizang_cnc":
                    return ISP.xizang_cnc;
                case "xianggang":
                    return ISP.xianggang;
                case "xinjiang_tel":
                    return ISP.xinjiang_tel;
                case "xinjiang_cnc":
                    return ISP.xinjiang_cnc;
                case "yunnan_tel":
                    return ISP.yunnan_tel;
                case "yunnan_cnc":
                    return ISP.yunnan_cnc;
                case "zhejiang_tel":
                    return ISP.zhejiang_tel;
                case "zhejiang_cnc":
                    return ISP.zhejiang_cnc;
                default:
                    return ISP.DEFAULT;
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
            switch (grade)
            {
                case "Free":
                    return DomainGrade.Free;
                case "Express":
                    return DomainGrade.Express;
                case "Extra":
                    return DomainGrade.Extra;
                case "Ultra":
                    return DomainGrade.Ultra;
                default:
                    return DomainGrade.Free;
            }
        }

        public static string ISPToText(ISP isp)
        {
            switch (isp)
            {
                case ISP.DEFAULT:
                    return "";
                case ISP.TEL:
                    return "电信";
                case ISP.CNC:
                    return "联通";
                case ISP.EDU:
                    return "教育";
                case ISP.CMC:
                    return "移动";
                case ISP.FOREIGN:
                    return "海外";
                case ISP.anhui_tel:
                    return "安徽电信";
                case ISP.anhui_cnc:
                    return "安徽网通";
                case ISP.aomen:
                    return "澳门";
                case ISP.beijing_tel:
                    return "北京电信";
                case ISP.beijing_cnc:
                    return "北京网通";
                case ISP.chongqing_tel:
                    return "重庆电信";
                case ISP.chongqing_cnc:
                    return "重庆网通";
                case ISP.fujian_tel:
                    return "福建电信";
                case ISP.fujian_cnc:
                    return "福建网通";
                case ISP.gansu_tel:
                    return "甘肃电信";
                case ISP.gansu_cnc:
                    return "甘肃网通";
                case ISP.guangdong_tel:
                    return "广东电信";
                case ISP.guangdong_cnc:
                    return "广东网通";
                case ISP.guangxi_tel:
                    return "广西电信";
                case ISP.guangxi_cnc:
                    return "广西网通";
                case ISP.guizhou_tel:
                    return "贵州电信";
                case ISP.guizhou_cnc:
                    return "贵州网通";
                case ISP.hainan_tel:
                    return "海南电信";
                case ISP.hainan_cnc:
                    return "海南网通";
                case ISP.hebei_tel:
                    return "河北电信";
                case ISP.hebei_cnc:
                    return "河北网通";
                case ISP.henan_tel:
                    return "河南电信";
                case ISP.henan_cnc:
                    return "河南网通";
                case ISP.heilongjiang_tel:
                    return "黑龙江电信";
                case ISP.heilongjiang_cnc:
                    return "黑龙江网通";
                case ISP.hubei_tel:
                    return "湖北电信";
                case ISP.hubei_cnc:
                    return "湖北网通";
                case ISP.hunan_tel:
                    return "湖南电信";
                case ISP.hunan_cnc:
                    return "湖南网通";
                case ISP.jilin_tel:
                    return "吉林电信";
                case ISP.jilin_cnc:
                    return "吉林网通";
                case ISP.jiangsu_tel:
                    return "江苏电信";
                case ISP.jiangsu_cnc:
                    return "江苏网通";
                case ISP.jiangxi_tel:
                    return "江西电信";
                case ISP.jiangxi_cnc:
                    return "江西网通";
                case ISP.liaoning_tel:
                    return "辽宁电信";
                case ISP.liaoning_cnc:
                    return "辽宁网通";
                case ISP.neimeng_tel:
                    return "内蒙电信";
                case ISP.neimeng_cnc:
                    return "内蒙网通";
                case ISP.ningxia_tel:
                    return "宁夏电信";
                case ISP.ningxia_cnc:
                    return "宁夏网通";
                case ISP.qinghai_tel:
                    return "青海电信";
                case ISP.qinghai_cnc:
                    return "青海网通";
                case ISP.shandong_tel:
                    return "山东电信";
                case ISP.shandong_cnc:
                    return "山东网通";
                case ISP.shanxi_tel:
                    return "山西电信";
                case ISP.shanxi_cnc:
                    return "山西网通";
                case ISP.shaanxi_tel:
                    return "陕西电信";
                case ISP.shaanxi_cnc:
                    return "陕西网通";
                case ISP.shanghai_tel:
                    return "上海电信";
                case ISP.shanghai_cnc:
                    return "上海网通";
                case ISP.sichuan_tel:
                    return "四川电信";
                case ISP.sichuan_cnc:
                    return "四川网通";
                case ISP.taiwan:
                    return "台湾";
                case ISP.tianjin_tel:
                    return "天津电信";
                case ISP.tianjin_cnc:
                    return "天津网通";
                case ISP.xizang_tel:
                    return "西藏电信";
                case ISP.xizang_cnc:
                    return "西藏网通";
                case ISP.xianggang:
                    return "香港";
                case ISP.xinjiang_tel:
                    return "新疆电信";
                case ISP.xinjiang_cnc:
                    return "新疆网通";
                case ISP.yunnan_tel:
                    return "云南电信";
                case ISP.yunnan_cnc:
                    return "云南网通";
                case ISP.zhejiang_tel:
                    return "浙江电信";
                case ISP.zhejiang_cnc:
                    return "浙江网通";
                default:
                    return isp.ToString();
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
        Free = 0,
        Express = 1,
        Extra = 2,
        Ultra = 3
    }
    public class RecordInfo
    {
        public int ID;
        public string Subname;
        public ISP Isp = ISP.DEFAULT;
        public RecordType Type;
        public int TTL = 3600;
        public string Value;
        public int MXLevel = 5;
        public bool Enable = true;
        public DNSPodUnitValidate Validate = DNSPodUnitValidate.New;
        public string Note;
        public RecordInfo() { }
        public RecordInfo(string subname, ISP isp, RecordType type, string value)
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
    public enum ISP
    {
        DEFAULT = 1,
        TEL = 2,
        CNC = 3,
        EDU = 4,
        CMC = 5,
        FOREIGN = 6,

        anhui_tel = 101,
        anhui_cnc = 102,
        aomen = 103,
        beijing_tel = 104,
        beijing_cnc = 105,
        chongqing_tel = 106,
        chongqing_cnc = 107,
        fujian_tel = 108,
        fujian_cnc = 109,
        gansu_tel = 110,
        gansu_cnc = 111,
        guangdong_tel = 112,
        guangdong_cnc = 113,
        guangxi_tel = 114,
        guangxi_cnc = 115,
        guizhou_tel = 116,
        guizhou_cnc = 117,
        hainan_tel = 118,
        hainan_cnc = 119,
        hebei_tel = 120,
        hebei_cnc = 121,
        henan_tel = 122,
        henan_cnc = 123,
        heilongjiang_tel = 124,
        heilongjiang_cnc = 125,
        hubei_tel = 126,
        hubei_cnc = 127,
        hunan_tel = 128,
        hunan_cnc = 129,
        jilin_tel = 130,
        jilin_cnc = 131,
        jiangsu_tel = 132,
        jiangsu_cnc = 133,
        jiangxi_tel = 134,
        jiangxi_cnc = 135,
        liaoning_tel = 136,
        liaoning_cnc = 137,
        neimeng_tel = 138,
        neimeng_cnc = 139,
        ningxia_tel = 140,
        ningxia_cnc = 141,
        qinghai_tel = 142,
        qinghai_cnc = 143,
        shandong_tel = 144,
        shandong_cnc = 145,
        shanxi_tel = 146,
        shanxi_cnc = 147,
        shaanxi_tel = 148,
        shaanxi_cnc = 149,
        shanghai_tel = 150,
        shanghai_cnc = 151,
        sichuan_tel = 152,
        sichuan_cnc = 153,
        taiwan = 154,
        tianjin_tel = 155,
        tianjin_cnc = 156,
        xizang_tel = 157,
        xizang_cnc = 158,
        xianggang = 159,
        xinjiang_tel = 160,
        xinjiang_cnc = 161,
        yunnan_tel = 162,
        yunnan_cnc = 163,
        zhejiang_tel = 164,
        zhejiang_cnc = 165,
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