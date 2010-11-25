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
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        };

        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

        public static bool SetAreoArea(IntPtr ptr)
        {
            try
            {
                // 设置Margins
                MARGINS margins = new MARGINS();

                // 扩展Aero Glass
                margins.cxLeftWidth = -1;
                margins.cxRightWidth = -1;
                margins.cyTopHeight = -1;
                margins.cyBottomHeight = -1;

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
