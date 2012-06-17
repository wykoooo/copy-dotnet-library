using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using System.Collections;
using System.IO;
using Silmoon.Windows.Win32.API.APIStructs;
using Silmoon.Windows.Win32.API.APIEnum;

namespace Silmoon.Windows.Systems
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemInfo : IDisposable
    {
        [DllImport("kernel32")]
        static extern void GetSystemInfo(ref CPU_INFO cpuinfo);
        [DllImport("kernel32")]
        static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        PerformanceCounter cpuTimePc;
        ManagementObjectSearcher searcher;


        /// <summary>
        /// 获取内存信息
        /// </summary>
        public MEMORY_INFO GetMemoryInfo
        {
            get
            {
                MEMORY_INFO meminfo = new MEMORY_INFO();
                GlobalMemoryStatus(ref meminfo);
                return meminfo;
            }
        }
        /// <summary>
        /// 返回CPU信息
        /// </summary>
        public CPU_INFO GetCPUInfo
        {
            get
            {
                CPU_INFO cpuinfo = new CPU_INFO();
                GetSystemInfo(ref cpuinfo);
                return cpuinfo;
            }
        }

        /// <summary>
        /// 获取当前多个CPU时间百分比数组
        /// </summary>
        /// <returns></returns>
        public int[] CPUsLoadPercentage
        {
            get
            {
                if (File.Exists("/proc/stat")) return new int[0];

                ArrayList cpuLoadArr = new ArrayList();
                if (searcher == null) searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    cpuLoadArr.Add(int.Parse(queryObj["LoadPercentage"].ToString()));
                }
                return (int[])cpuLoadArr.ToArray(typeof(int));
            }
        }
        /// <summary>
        /// 获取当前CPU时间百分比
        /// </summary>
        /// <returns></returns>
        public int CPULoadPercentage
        {
            get
            {
                int[] result = CPUsLoadPercentage;
                int c = result.Length;
                int d = 0;
                for (int i = 0; i < c; i++)
                {
                    d += result[i];
                }
                return (d / c);
            }
        }

        /// <summary>
        /// 获取系统地址宽度位数
        /// </summary>
        public static int SystemAddressWidth
        {
            get
            {
                ConnectionOptions oConn = new ConnectionOptions();
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\localhost", oConn);
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
                ManagementObjectCollection oReturnCollection = oSearcher.Get();
                string addressWidth = null;

                foreach (ManagementObject oReturn in oReturnCollection)
                {
                    addressWidth = oReturn["AddressWidth"].ToString();
                }
                if (addressWidth == null) return 0;
                return int.Parse(addressWidth);
            }
        }
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
                        StringBuilder userName = new StringBuilder();           // 用户名
                        StringBuilder clientUser = new StringBuilder();         // 客户端名
                        StringBuilder stateType = new StringBuilder();          // 会话类型
                        byte[] connState = new byte[4];

                        bool userNameBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSUserName, out userName, out count);
                        bool clientUserBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSClientName, out clientUser, out count);
                        bool stateTypeBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSWinStationName, out stateType, out count);
                        bool connStateBool = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, WTSInfoClass.WTSConnectState, out connState, out count);
                        if (userNameBool && clientUserBool && stateTypeBool & connStateBool)
                        {
                            cum = new LogonUser();
                            cum.UserName = userName.ToString();
                            cum.ClientUserName = clientUser.ToString();
                            cum.SessionType = stateType.ToString();
                            cum.ConnectState = (WTS_CONNECTSTATE_CLASS)BitConverter.ToInt32(connState, 0);

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

        #region IDisposable 成员

        public void Dispose()
        {
            //if (cpuTimePc != null) cpuTimePc.Dispose();
            if (searcher != null) searcher.Dispose();
        }

        #endregion
    }
    /// <summary>
    /// CPU信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CPU_INFO
    {
        public uint dwOemId;
        public uint dwPageSize;
        public uint lpMinimumApplicationAddress;
        public uint lpMaximumApplicationAddress;
        public uint dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public uint dwProcessorLevel;
        public uint dwProcessorRevision;
    }
    /// <summary>
    /// 内存信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
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
                    arrSessionInfo[i] = (WTS_SESSION_INFO)Marshal.PtrToStructure(new IntPtr(p),
                        Session_Info.GetType());
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
