using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2
{
    class ConnectionManager : IConnection
    {
        readonly int port = - 1;
        private TcpClient connection;

        public ConnectionManager(string ip, int port) {
            this.port = port;
        }

        void IConnection.Connect()
        {
           
        }

        public void WriteData(string data)
        {
            
        }

        public string ReadData()
        {
            
        }
    }
}