using System.Net.Sockets;

namespace MySkypeNetworking
{
    public abstract class MyUdpConnection{
        protected UdpClient UdpConnection { get; set; }
        protected string Address { get; set;}
        protected int Port { get;set; }
        public MyUdpConnection(string address, int port)
        {
            this.Port = port;
            this.Address = address;
        }

        public virtual MyUdpConnection CloseConnection()
        {
            UdpConnection.Close();
            return this;
        }
    }
}
