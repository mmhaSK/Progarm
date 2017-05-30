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
using System.IO;

namespace App2
{
    class ConnectionManager : IConnection
    {
        readonly int telnet_port = 23456;
        const string server_ip = "192.168.0.16";
        private StreamWriter writer;
        private StreamReader reader;
        private static ConnectionManager manager = new ConnectionManager();
        private ConnectionManager() {}

        public static ConnectionManager GetInstance() {
            return manager;
        }

        public bool Connect()
        {
            try
            {
                TcpClient connection = new TcpClient(server_ip, telnet_port);
                // po 10 sekundach vyhodi chybu
                connection.ReceiveTimeout = 10000;
                //vrati stream socketu
                NetworkStream stream = connection.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
                return true;
            }
            catch (Exception) {
                reader = null;
                writer = null;
                return false;
            }
        }

        public void WriteData(string data)
        {
            writer.WriteLine(data);
        }

        public string ReadData()
        {
            // ak sa po nastavenej dobe neprimu data vyhodi chybu
            try
            {
                return reader.ReadToEnd();
            }
            catch (IOException) {
                //reconnect v nadradenej metode
                return null;
            }
        }

        public void Hello()
        {
            WriteData("Hello packet");
        }
    }
}