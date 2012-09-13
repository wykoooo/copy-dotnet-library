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
            CloseStyle = WindowCloseStyle.MixStyleExt;
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

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("项目", typeof(System.String));
            dt.Columns.Add("耗值", typeof(System.Int32));
            //dt.Rows.Add("学车", 40);
            dt.Rows.Add("情感(现在)", 30);
            dt.Rows.Add("情感(回忆)", 10);
            dt.Rows.Add("个人工作", 30);
            dt.Rows.Add("路途颠簸", 20);
            dt.Rows.Add("其他", 10);
            Bitmap graph = Silmoon.Imaging.ChartUtil.GetPieGraph("2012年9月8日 个人内耗列表", 600, 500, 100, 40, dt);
            pictureBox1.Image = graph;
        }
    }
}