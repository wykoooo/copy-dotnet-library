using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Silmoon.Windows.Systems;

namespace NetTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //RDController rdc = new RDController();

            //var users = RDController.GetLogonUserList();

            //foreach (var item in users)
            //{
            //    Console.WriteLine(item.UserName + "(" + item.SessionId + ")");
            //    Console.WriteLine(item.ProtocalType.ToString());
            //    Console.WriteLine();
            //}

            //rdc.Disconnect(1, true);
        }
    }
}