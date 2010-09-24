using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Silmoon.Utility.FTP
{
    /// <summary>
    /// Serv-U FTP服务器客户端功能类
    /// 
    /// 要使用本类型必须将Serv-U的服务器端口设置成 21，
    /// 并且设置IP为任何可以IP
    /// </summary>
    public class ServUClient
    {
        TcpClient _client;
        NetworkStream _stream;
        string _password;

        /// <summary>
        /// 设置连接Serv-U的密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// 使用指定密码创建Serv-U客户端的新实例
        /// </summary>
        /// <param name="password">Serv-U的密码</param>
        public ServUClient(string password)
        {
            _password = password;
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="userpath">用户目录</param>
        /// <returns></returns>
        public FtpStatusResult CreateUser(string username, string password, string userpath)
        {
            _client = new TcpClient();
            FtpStatusResult ftpStatus;

            _client.Connect("localhost", 43958);
            _stream = _client.GetStream();
            Receive(_stream);

            Send("user localadministrator\r\n");
            Receive(_stream);

            Send("pass " + _password + "\r\n");
            string passData = Receive(_stream);
            string[] passLine = passData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string passLastLine = passLine[passLine.Length - 1];
            if (passLastLine.Substring(0, 3) != "230")
            {
                string[] passstatus = passLastLine.Split(new string[] { " " }, 2, StringSplitOptions.None);
                ftpStatus.StatusCode = int.Parse(passstatus[0]);
                ftpStatus.StatusText = passstatus[1];
                _client.Close();
                return ftpStatus;
            }

            Send("site maintenance\r\n");
            Receive(_stream, 3);

            Send("-SETUSERSETUP\r\n");
            Send("-IP=0.0.0.0\r\n");
            Send("-PortNo=21\r\n");
            Send("-User=" + username + "\r\n");
            Send("-Password=" + password + "\r\n");
            Send("-HomeDir=" + userpath + "\r\n");
            Send("-Access=" + userpath + "|RWAMLCDP\r\n");
            Send(" RelPaths=1\r\n");
            string resultData = Receive(_stream);
            string[] resultLine = resultData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            _client.Close();
            string[] status = resultLine[resultLine.Length - 1].Split(new string[] { " " }, 2, StringSplitOptions.None);

            ftpStatus.StatusCode = int.Parse(status[0]);
            ftpStatus.StatusText = status[1];
            return ftpStatus;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="username">指定的用户名</param>
        /// <returns></returns>
        public FtpStatusResult DeleteUser(string username)
        {
            _client = new TcpClient();
            FtpStatusResult ftpStatus;

            _client.Connect("localhost", 43958);
            _stream = _client.GetStream();
            Receive(_stream);

            Send("user localadministrator\r\n");
            Receive(_stream);

            Send("pass " + _password + "\r\n");
            string passData = Receive(_stream);
            string[] passLine = passData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string passLastLine = passLine[passLine.Length - 1];
            if (passLastLine.Substring(0, 3) != "230")
            {
                string[] passstatus = passLastLine.Split(new string[] { " " }, 2, StringSplitOptions.None);
                ftpStatus.StatusCode = int.Parse(passstatus[0]);
                ftpStatus.StatusText = passstatus[1];
                _client.Close();
                return ftpStatus;
            }

            Send("site maintenance\r\n");
            Receive(_stream, 3);

            Send("-DELETEUSERSETUP\r\n");
            Send("-IP=0.0.0.0\r\n");
            Send("-PortNo=21\r\n");
            Send(" User=" + username + "\r\n");
            string resultData = Receive(_stream);
            string[] resultLine = resultData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            _client.Close();
            string[] status = resultLine[resultLine.Length - 1].Split(new string[] { " " }, 2, StringSplitOptions.None);

            ftpStatus.StatusCode = int.Parse(status[0]);
            ftpStatus.StatusText = status[1];
            return ftpStatus;
        }

        string Receive(NetworkStream ns, int endTag)
        {
            int i = 0;
            byte[] dataCache = new byte[1024];
            string data = "";
            string[] lineArray = null;
            int endTagCount = 0;

            while ((i = ns.Read(dataCache, 0, dataCache.Length)) > 0)
            {
                string lineString = Encoding.Default.GetString(dataCache).Substring(0, i);
                data += lineString;
                if (lineString.Substring(lineString.Length - 2, 2) == "\r\n")
                {
                    lineArray = lineString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in lineArray)
                    {
                        if (char.IsWhiteSpace(s[3]))
                        {
                            endTagCount++;
                            if (endTagCount == endTag)
                                return data;
                        }
                    }
                }
            }
            return data;
        }
        string Receive(NetworkStream ns)
        {
            return Receive(ns, 1);
        }
        void Send(string s)
        {
            byte[] bytes = Encoding.Default.GetBytes(s);
            _stream.Write(bytes, 0, bytes.Length);
        }

        public static FtpResultType GetStatusResultType(FtpStatusResult status)
        {
            switch (status.StatusCode)
            {
                case 530:
                    return FtpResultType.LoginFail;
                case 200:
                    return FtpResultType.Success;
                default:
                    return FtpResultType.Unknown;
            }
        }
    }
    /// <summary>
    /// Serv-U返回的执行状态
    /// </summary>
    public struct FtpStatusResult
    {
        public int StatusCode;
        public string StatusText;
    }
    public enum FtpResultType
    {
        Unknown = 0,
        Success = 1,
        LoginFail = 2,
    }
}