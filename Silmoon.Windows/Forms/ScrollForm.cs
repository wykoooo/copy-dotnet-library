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
        int fromW = 0;
        int fromH = 0;
        Point location = new Point(0, 0);
        public ScrollForm()
        {
            InitializeComponent();
            closeScrollTimer.Interval = 1;
            closeScrollTimer.Tick += new EventHandler(closeScrollTimer_Tick);
            startScrollTimer.Interval = 1;
            startScrollTimer.Tick += new EventHandler(startScrollTimer_Tick);
            showTimer.Interval = 1;
            showTimer.Tick += new EventHandler(showTimer_Tick);
            hideTimer.Interval = 1;
            hideTimer.Tick += new EventHandler(hideTimer_Tick);
        }

        void closeScrollTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height > 50)
            {
                this.Size = new Size(this.Width, this.Height - 18);
                this.Location = new Point(this.Location.X, this.Location.Y + 9);
                Opacity = Opacity - 0.05;
            }
            else if (this.Width > 150)
            {
                this.Size = new Size(this.Width - 18, this.Height);
                this.Location = new Point(this.Location.X + 9, this.Location.Y);
                Opacity = Opacity - 0.05;
            }
            else
            {
                closeScrollTimer.Stop();
                Close();
            }
        }
        void showTimer_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.02;
            if (this.Opacity == 1) showTimer.Stop();
        }
        void startScrollTimer_Tick(object sender, EventArgs e)
        {
            this.Width += 40;

            if (this.Width >= fromW)
            {
                this.Width = fromW;
                startScrollTimer.Stop();
            }
        }
        void hideTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height > 50)
            {
                this.Size = new Size(this.Width, this.Height - 18);
                this.Location = new Point(this.Location.X, this.Location.Y + 9);
                Opacity = Opacity - 0.05;
            }
            else if (this.Width > 150)
            {
                this.Size = new Size(this.Width - 18, this.Height);
                this.Location = new Point(this.Location.X + 9, this.Location.Y);
                Opacity = Opacity - 0.05;
            }
            else
            {
                hideTimer.Stop();
                this.Visible = false;
                this.Width = fromW;
                this.Height = fromH;
                this.Location = new Point(location.X, location.Y);
            }
        }

        Timer closeScrollTimer = new Timer();
        Timer showTimer = new Timer();
        Timer startScrollTimer = new Timer();
        Timer hideTimer = new Timer();
        FormClosingEventArgs closeArgs;

        protected override void OnLoad(EventArgs e)
        {
            refreshStateParam();
            base.OnLoad(e);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.Height > 50)
                {
                    closeArgs = e;
                    e.Cancel = true;
                    closeScrollTimer.Start();
                    this.Text = "";
                    if (this.WindowState == FormWindowState.Maximized)
                        this.WindowState = FormWindowState.Normal;
                }
                else
                    e.Cancel = false;
            }
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        public void ShowEx()
        {
            this.Show();
            this.Opacity = 0;
            this.Width = 1;
            this.Visible = true;
            showTimer.Start();
            startScrollTimer.Start();
        }
        public void HideEx()
        {
            refreshStateParam();
            hideTimer.Start();
        }

        void refreshStateParam()
        {
            fromW = this.Width;
            fromH = this.Height;
            location = new Point(this.Location.X, this.Location.Y);
        }

        private void ScrollForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            startScrollTimer.Stop();
            closeScrollTimer.Stop();
            showTimer.Stop();
            hideTimer.Stop();

            startScrollTimer.Dispose();
            closeScrollTimer.Dispose();
            showTimer.Dispose();
            hideTimer.Dispose();
        }
    }
}