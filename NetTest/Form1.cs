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

namespace NetTest
{
    public partial class Form1 : Form
    {
        Smmp smp = new Smmp();
        __listen__readSmmp r;

        Tcp tcp = new Tcp();
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            smp.OnReceivedData += new SmmpReceiveDataEventHander(smp_OnReceivedData);
            smp.OnConnectionEvent += new SmmpOnConnectionEventHander(smp_OnConnectionEvent);
        }

        void smp_OnConnectionEvent(TcpStruct localTcpInfo, TcpStruct remoteTcpInfo, __listen__readSmmp tcpReader, int clientID)
        {
            if (tcpReader != null)
            {
                SmmpPacket packet = new SmmpPacket(1232, 0);
                packet.ContentBuffer = smp.DataEncoding.GetBytes("silmoon");
                packet.Messages.Add("messagetype", "test");
                tcpReader.SendData(packet);
                r = tcpReader;
            }
            else r = null;
        }

        void smp_OnReceivedData(TcpStruct localTcpInfo, TcpStruct remoteTcpInfo, SmmpPacket packet, __listen__readSmmp tcpReader)
        {
            try
            {
                string s = "MessageID:" + packet.MessageID + "\r\n";
                s += "ResponseID:" + packet.ResponseID + "\r\n";
                s += "MessageBytes:" + packet.Messages.Count + "\r\n";
                s += "ContentData:" + smp.DataEncoding.GetString(packet.ContentBuffer) + "\r\n";
                MessageBox.Show(this, s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            smp.AsyncStartListen(IPAddress.Any, 9433);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SmmpPacket packet = new SmmpPacket(new Random().Next(1, 999999), -1);
            packet.MakeByteData(Encoding.Unicode.GetBytes(textBox1.Text));
            packet.Messages.Add("test", "true");
            r.SendData(packet);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tcp.OnConnectionEvent += new TcpOnConnectionEventHander(tcp_OnConnectionEvent);
            tcp.AsyncStartListen(IPAddress.Any, 6669);
            
        }

        void tcp_OnConnectionEvent(TcpStruct localTcpInfo, TcpStruct remoteTcpInfo, ITcpReader tcpReader, int clientID)
        {

        }

        void tcp_OnReceivedData(TcpStruct localTcpInfo, TcpStruct remoteTcpInfo, byte[] data, ITcpReader tcpReader)
        {

        }
         
        private void button4_Click(object sender, EventArgs e)
        {
            byte[] b1 = { 1, 2, 3, 4 };
            byte[] b2 = { 5, 6, 7, 8 };
            Memory.MemCpy(ref b1, ref b2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] s1 = new string[] { "a", "c", "d", "e", "g" };
            string[] s2 = new string[] { "a", "b", "c", "d", "g" };
            string[] result = SmString.MissedItems(s1, s2);
        }
    }
}