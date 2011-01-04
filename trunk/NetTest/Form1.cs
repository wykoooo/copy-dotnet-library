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
            lock (arraylist)
            {
                arraylist.Add(DateTime.Now.ToString());
                Thread.Sleep(3000);
            }
            listBox1.Items.Add("thread " + Thread.CurrentThread.ManagedThreadId + " end." + DateTime.Now);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] items1 = new string[] { "1", "2", "3", "4", "5" };
            string[] items2 = new string[] { "1", "3", "4" };
            string[] missedItems = SmString.MissedItems(items1, items2);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SmTcp tcp = new SmTcp();
            tcp.AsyncStartListen(IPAddress.Any, 8889);
            tcp.OnReceivedData += new TcpReceiveDataEventHander(tcp_OnReceivedData);
        }

        void tcp_OnReceivedData(TcpStruct localTcpInfo, TcpStruct remoteTcpInfo, byte[] data, ITcpReader tcpReader)
        {
            MessageBox.Show(System.Text.Encoding.Default.GetString(data));
        }
    }
}