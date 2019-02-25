using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyUdp
{
    public abstract class MyUdpConnection{
        protected UdpClient Client { get; set; }
        protected string Address { get; set;}
        protected int Port { get;set; }
        public MyUdpConnection(string address, int port)
        {
            this.Port = port;
            this.Address = address;
        }

        public virtual MyUdpConnection CloseConnection()
        {
            Client.Close();
            return this;
        }
    }
}
