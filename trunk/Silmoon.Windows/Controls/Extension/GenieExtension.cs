using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace Silmoon.Windows.Controls.Extension
{
    public class GenieExtension
    {
        Control control = null;
        /// <summary>
        /// 处理特效的控件的父控件
        /// </summary>
        /// <param name="control"></param>
        public GenieExtension(Control control)
        {
            this.control = control;
        }
        public void ShowPanel(Control panel)
        {
            Thread thread = new Thread(colorProc);
            thread.IsBackground = true;
            thread.Start(panel);
            Thread thread2 = new Thread(scrollProc);
            thread2.IsBackground = true;
            thread2.Start(panel);
        }
        void colorProc(object obj)
        {
            Control panel = obj as Control;
            int a, r, g, b;
            a = panel.BackColor.A;
            r = panel.BackColor.R;
            g = panel.BackColor.G;
            b = panel.BackColor.B;

            control.Invoke(new Action<int>(delegate(int i) { panel.BackColor = Color.White; }), 0);

            bool stop = false;
            while (!stop)
            {
                if (panel.BackColor.A == a && panel.BackColor.R == r && panel.BackColor.G == g && panel.BackColor.B == b)
                    break;

                Color beforeColor = panel.BackColor;
                Color newColor = Color.Empty;
                int na = 0, nr = 0, ng = 0, nb = 0;

                if (panel.BackColor.A != a)
                    na = beforeColor.A - 1;
                else na = beforeColor.A;

                if (panel.BackColor.R != r)
                    nr = beforeColor.R - 1;
                else na = beforeColor.R;

                if (panel.BackColor.G != g)
                    ng = beforeColor.G - 1;
                else na = beforeColor.G;

                if (panel.BackColor.B != b)
                    nb = beforeColor.B - 1;
                else na = beforeColor.B;


                newColor = Color.FromArgb(na, nr, ng, nb);
                control.Invoke(new Action<int>(delegate(int i) { panel.BackColor = newColor; }), 0);
                Thread.Sleep(100);
            }
        }
        void scrollProc(object obj)
        {
            Control panel = obj as Control;

            int w = panel.Width;
            int h = panel.Height;

            control.Invoke(new Action<int>(delegate(int i)
            {
                panel.Visible = true;
                panel.Width = 1;
                panel.Height = 20;
            }), 0);

            while (panel.Width != w)
            {
                control.Invoke(new Action<int>(delegate(int i) { panel.Width += 1; }), 0);
                Thread.Sleep(1);
            }

            control.Invoke(new Action<int>(delegate(int i) { panel.Height = 1; }), 0);
            while (panel.Height != h)
            {
                control.Invoke(new Action<int>(delegate(int i) { panel.Height += 1; }), 0);
                Thread.Sleep(1);
            }
        }
    }
}
