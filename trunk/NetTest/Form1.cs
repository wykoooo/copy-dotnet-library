using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Silmoon.Net;
using System.Net;
using Silmoon.Memory;
using System.Collections;
using Silmoon;
using System.Threading;
using Silmoon.Threading;
using System.Security.Cryptography;
using Silmoon.Windows.Controls.Extension;

namespace NetTest
{
    public partial class Form1 : Silmoon.Windows.Forms.GenieForm
    {
        GenieExtension ge = null;
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            ge = new GenieExtension(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CloseStyle = WindowCloseStyle.MaxStyleExt;
            checkBox1.Checked = false;
            ge.FocusSlide(textBox1, 150, 100, 10);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (panel1 != null)
                panel1.Height = this.Height - 17;
            base.OnSizeChanged(e);
        }
        protected override void OnShown(EventArgs e)
        {
            ShowEx();
            base.OnShown(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Silmoon.Security.CSEncrypt c = new Silmoon.Security.CSEncrypt();
            byte[] d = { 129 };
            d = c.Encrypt(d);
            d = c.Decrypt(d);


            MessageBox.Show(d[0].ToString());
        }
    }
}