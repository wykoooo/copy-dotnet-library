using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Silmoon.Windows.Forms
{
    public partial class ScrollForm : Form
    {
        public ScrollForm()
        {
            InitializeComponent();
            scrollTimer.Interval = 1;
            scrollTimer.Tick += new EventHandler(scrollTimer_Tick);
        }

        int scrollCount = 3;
        void scrollTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height > 50)
            {

                this.Size = new Size(this.Width, this.Height - 60);
                this.Location = new Point(this.Location.X, this.Location.Y + 30);
            }
            else if (this.Width > 150)
            {
                this.Size = new Size(this.Width - 30, this.Height);
                this.Location = new Point(this.Location.X + 15, this.Location.Y);
            }
            else
            {
                scrollTimer.Stop();
                Close();
            }
        }

        Timer scrollTimer = new Timer();
        FormClosingEventArgs closeArgs;
        object closeSender;
        private void ScrollForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.Height > 50)
                {
                    closeArgs = e;
                    closeSender = sender;
                    e.Cancel = true;
                    scrollTimer.Start();
                    this.Text = "";
                    if (this.WindowState == FormWindowState.Maximized)
                        this.WindowState = FormWindowState.Normal;
                }
                else
                    e.Cancel = false;
            }
        }
    }
}