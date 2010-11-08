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

namespace NetTest
{
    public partial class Form1 : Form
    {
        ArrayList arraylist = new ArrayList();
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Threads.ExecAsync(t);

        }
        void t()
        {
            listBox1.Items.Add("thread " + Thread.CurrentThread.ManagedThreadId + " start." + DateTime.Now);
            //lock (arraylist)
            //{
                arraylist.Add(DateTime.Now.ToString());
                Thread.Sleep(3000);
            //}
            listBox1.Items.Add("thread " + Thread.CurrentThread.ManagedThreadId + " end." + DateTime.Now);
        }
    }
}