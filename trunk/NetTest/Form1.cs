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
using Silmoon.Data.SqlClient;

namespace NetTest
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                textBox2.Text = wc.UploadString("http://stateserver.silmoon.com/pushStateLog", textBox1.Text);
            }
        }
    }
}