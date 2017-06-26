using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace App2
{
    class ConnectionManager : IConnection
    {
        readonly int telnet_port = 23456;
        const string server_ip = "192.168.0.100";
        private NetworkStream stream;
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
                Console.WriteLine("Pripojene " + connection.ToString());
                // po 10 sekundach vyhodi chybu
                connection.ReceiveTimeout = 10000;
                //vrati stream socketu
                stream = connection.GetStream();
                return true;
            }
            catch (Exception) {
                stream = null;
                return false;
            }
        }

        public void WriteData(string data)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(data);
            stream.Write(messageBytes, 0, messageBytes.Length);
        }

        public string ReadData()
        {
            // ak sa po nastavenej dobe neprimu data vyhodi chybu
            try
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, 1024);
                return System.Text.Encoding.UTF8.GetString(buffer);
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

        public void Close()
        {
            stream.Close();
        }

        public bool isOpen() {
            return stream.CanRead && stream.CanWrite;
        }
    }
}