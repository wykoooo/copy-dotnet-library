using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Net;

namespace Silmoon.Web
{
    public class HttpHelper
    {
        public static string UrlPathEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            int index = str.IndexOf('?');
            if (index >= 0)
            {
                return (UrlPathEncode(str.Substring(0, index), e) + str.Substring(index));
            }
            return UrlEncodeSpaces(UrlEncodeNonAscii(str, e));
        }
        public static string WebRoot
        {
            get
            {
                string r = HttpRuntime.AppDomainAppVirtualPath;
                if (r == "/" || r == "\\") return "";
                else return r;
            }
        }
        public static string PathRoot
        {
            get { return HttpRuntime.AppDomainAppPath; }
        }
        public static IPAddress GetClientIPAddress()
        {
            IPAddress result = null;

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["X-FORWARDED-FOR"]))
                return IPAddress.Parse(HttpContext.Current.Request.ServerVariables["X-FORWARDED-FOR"]);

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                return IPAddress.Parse(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]))
                return IPAddress.Parse(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            return result;
        }

        internal static string UrlEncodeSpaces(string str)
        {
            if ((str != null) && (str.IndexOf(' ') >= 0))
            {
                str = str.Replace(" ", "%20");
            }
            return str;
        }
        internal static string UrlEncodeNonAscii(string str, Encoding e)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (e == null)
            {
                e = Encoding.UTF8;
            }
            byte[] bytes = e.GetBytes(str);
            bytes = UrlEncodeBytesToBytesInternalNonAscii(bytes, 0, bytes.Length, false);
            return Encoding.ASCII.GetString(bytes);
        }
        private static byte[] UrlEncodeBytesToBytesInternalNonAscii(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                if (IsNonAsciiByte(bytes[offset + i]))
                {
                    num++;
                }
            }
            if (!alwaysCreateReturnValue && (num == 0))
            {
                return bytes;
            }
            byte[] buffer = new byte[count + (num * 2)];
            int num3 = 0;
            for (int j = 0; j < count; j++)
            {
                byte b = bytes[offset + j];
                if (IsNonAsciiByte(b))
                {
                    buffer[num3++] = 0x25;
                    buffer[num3++] = (byte)IntToHex((b >> 4) & 15);
                    buffer[num3++] = (byte)IntToHex(b & 15);
                }
                else
                {
                    buffer[num3++] = b;
                }
            }
            return buffer;
        }
        private static bool IsNonAsciiByte(byte b)
        {
            if (b < 0x7f)
            {
                return (b < 0x20);
            }
            return true;
        }
        public static void PrintHtml(string title, string message, string filename)
        {
            string s = File.ReadAllText(Silmoon.Web.HttpHelper.PathRoot + "html\\" + filename);
            s = s.Replace("{$Title$}", title).Replace("{$Message$}", message);
            HttpContext.Current.Response.Write(s);
        }
        internal static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }
        public static void Redirect(string url)
        {
            HttpContext.Current.Response.StatusCode = 302;
            HttpContext.Current.Response.Headers.Add("Location", url);
        }
    }
}
