using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Silmoon.Windows.Win32.API.APIStructs
{
    public struct WTS_SESSION_INFO
    {
        public int SessionID;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pWinStationName;
        public WTS_CONNECTSTATE_CLASS state;
    }
}
