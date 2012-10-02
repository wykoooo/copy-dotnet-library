using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Silmoon.Windows.Win32.API.APIEnum;
using Silmoon.Windows.Win32.API.APIStructs;

namespace Silmoon.Windows.Systems
{
    public class RDController
    {
        /// <summary>
        /// 获取TS用户回话列表
        /// </summary>
        /// <returns></returns>
        public static List<LogonUser> GetLogonUserList()
        {
            List<LogonUser> LogonUsers = null;
            #region 查询代码
            WTS_SESSION_INFO[] pSessionInfo = TSControl.SessionEnumeration();
            LogonUser cum = null;
            LogonUsers = new System.Collections.Generic.List<LogonUser>();
            for (int i = 0; i < pSessionInfo.Length; i++)
            {
                if ("RDP-Tcp" != pSessionInfo[i].pWinStationName)
                {
                    try
                    {
                        int count = 0;
                        IntPtr buffer = IntPtr.Zero;
                        StringBuilder userName = new StringBuilder();
                        StringBuilder clientUser = new StringBuilder();
                        StringBuilder stateType = new StringBuilder();
                        byte[] protocalType = new byte[2];
                        byte[] connState = new byte[1];
                        StringBuilder clientAddress = new StringBuilder();

                        bool userNameBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSUserName, out userName, out count);
                        bool clientUserBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSClientName, out clientUser, out count);
                        bool stateTypeBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSWinStationName, out stateType, out count);
                        bool protocalTypeBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSClientProtocolType, out protocalType, out count);
                        bool connStateBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSConnectState, out connState, out count);
                        bool clientAddressBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSClientAddress, out clientAddress, out count);

                        if (userNameBool && clientUserBool && stateTypeBool & connStateBool)
                        {
                            cum = new LogonUser();
                            cum.SessionId = pSessionInfo[i].SessionID;
                            cum.UserName = userName.ToString();
                            cum.ClientUserName = clientUser.ToString();
                            cum.SessionType = stateType.ToString();
                            cum.ProtocalType = (Silmoon.Windows.Systems.LogonUser.ClientProtocalType)((int)protocalType[0]);
                            cum.ConnectState = (WTS_CONNECTSTATE_CLASS)connState[0];

                            WTS_CLIENT_ADDRESS ad = new WTS_CLIENT_ADDRESS();


                            //var aa = clientAddress[1];
                        }
                        LogonUsers.Add(cum);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            #endregion
            return LogonUsers;
        }

        public static bool Disconnect(int sessionid, bool wait = false)
        {
            return TSControl.WTSDisconnectSession(IntPtr.Zero, sessionid, wait);
        }
        public static bool Logoff(int sessionid, bool wait = false)
        {
            return TSControl.WTSLogoffSession(IntPtr.Zero, sessionid, wait);
        }


        #region 本机连接用户信息API封装
        public class TSControl
        {
            [DllImport("wtsapi32", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool WTSEnumerateSessions(int hServer, int Reserved,
                int Version, ref long ppSessionInfo, ref int pCount);
            [DllImport("wtsapi32.dll")]
            public static extern void WTSFreeMemory(System.IntPtr pMemory);
            [DllImport("wtsapi32.dll")]
            public static extern bool WTSLogoffSession(int hServer, long SessionId, bool bWait);
            [DllImport("Wtsapi32.dll")]
            public static extern bool WTSQuerySessionInformation(System.IntPtr hServer, int sessionId, WTSInfoClass wtsInfoClass, out StringBuilder ppBuffer, out int pBytesReturned);
            [DllImport("Wtsapi32.dll")]
            public static extern bool WTSQuerySessionInformation(System.IntPtr hServer, int sessionId, WTSInfoClass wtsInfoClass, out byte[] ppBuffer, out int pBytesReturned);
            [DllImport("Wtsapi32.dll")]
            public static extern bool WTSQuerySessionInformation(System.IntPtr hServer, int sessionId, WTSInfoClass wtsInfoClass, out long ppBuffer, out int pBytesReturned);
            [DllImport("Wtsapi32.dll")]
            public static extern bool WTSDisconnectSession(IntPtr hServer, int sessionid, bool bWait);
            [DllImport("Wtsapi32.dll")]
            public static extern bool WTSLogoffSession(IntPtr hServer, int sessionid, bool bWait);



            public static WTS_SESSION_INFO[] SessionEnumeration()
            {
                //Set handle of terminal server as the current terminal server
                int hServer = 0;
                bool RetVal;
                long lpBuffer = 0;
                int Count = 0;
                long p;
                WTS_SESSION_INFO Session_Info = new WTS_SESSION_INFO();
                WTS_SESSION_INFO[] arrSessionInfo;
                RetVal = WTSEnumerateSessions(hServer, 0, 1, ref lpBuffer, ref Count);
                arrSessionInfo = new WTS_SESSION_INFO[0];
                if (RetVal)
                {
                    arrSessionInfo = new WTS_SESSION_INFO[Count];
                    int i;
                    p = lpBuffer;
                    for (i = 0; i < Count; i++)
                    {
                        arrSessionInfo[i] = (WTS_SESSION_INFO)Marshal.PtrToStructure(new IntPtr(p), Session_Info.GetType());
                        p += Marshal.SizeOf(Session_Info.GetType());
                    }
                    WTSFreeMemory(new IntPtr(lpBuffer));
                }
                else
                {
                    //Insert Error Reaction Here  
                }
                return arrSessionInfo;
            }
        }
        #endregion

    }
}
