using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Silmoon.Windows.Desktops
{
    public class Desktop
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        };

        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll", EntryPoint = "DwmEnableComposition")]
        public extern static uint Win32DwmEnableComposition(uint uCompositionAction);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public extern static bool DwmIsCompositionEnabled();

        public static bool SetAreoArea(IntPtr ptr, ref MARGINS margins)
        {
            try
            {
                int hr = DwmExtendFrameIntoClientArea(ptr, ref margins);
                if (hr < 0)
                {
                    return false;
                }
            }
            catch (DllNotFoundException)
            {
                return false;
            }
            return true;
        }

    }
}
