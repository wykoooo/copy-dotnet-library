using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Silmoon.Windows.Controls
{
    public partial class SwitchButton : UserControl
    {
        SwitchStateType switchState = SwitchStateType.Off;

        public SwitchStateType SwitchState
        {
            get
            {
                { return switchState; }
            }
            set
            {
                switchState = value;
                _switch(value == SwitchStateType.On);
            }
        }

        bool mouseDown = false;
        public SwitchButton()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            pictureBox1.BackgroundImage = WindowsResource.SwitchButton_ButtonDown;
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            pictureBox1.BackgroundImage = WindowsResource.SwitchButton_ButtonUp;
        }

        public enum SwitchStateType
        {
            Off=0,On=1
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (SwitchState == SwitchStateType.Off)
                SwitchState = SwitchStateType.On;
            else SwitchState = SwitchStateType.Off;
        }


        void _switch(bool on)
        {
            Thread _t = null;
            if (on) _t = new Thread(_tOn);
            else _t = new Thread(_tOff);
            _t.IsBackground = true;
            _t.Start();
        }
        void _tOn()
        {
            for (int i = 0; i < 31; i++)
            {
                try
                {
                    pictureBox1.Location = new Point(i, 0);
                    ctlEnableBIMG.Location = new Point(-30 + i, 0);
                    ctlDisableBIMG.Location = new Point(0 + i, 0);
                }
                catch { }
            }
        }
        void _tOff()
        {
            for (int i = 0; i < 31; i++)
            {
                try
                {
                    pictureBox1.Location = new Point(30 - i, 0);
                    ctlEnableBIMG.Location = new Point(0 - i, 0);
                    ctlDisableBIMG.Location = new Point(30 - i, 0);
                }
                catch { }
            }
        }

        private void ctlDisableBIMG_Click(object sender, EventArgs e)
        {
            if (SwitchState == SwitchStateType.Off)
                SwitchState = SwitchStateType.On;
            else SwitchState = SwitchStateType.Off;
        }

        private void ctlEnableBIMG_Click(object sender, EventArgs e)
        {
            if (SwitchState == SwitchStateType.Off)
                SwitchState = SwitchStateType.On;
            else SwitchState = SwitchStateType.Off;
        }
    }
}
