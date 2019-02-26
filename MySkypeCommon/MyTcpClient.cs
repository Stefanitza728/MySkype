using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyUdp
{
    public class MyTcpClient : MyTcpConnection
    {
        protected TcpClient client { get; set; }
        protected NetworkStream nwStream { get; set; }

        public MyTcpClient(string address, int port) : base(address,port)
        {
        }

        public void OpenConnection()
        {
            client = new TcpClient();
            client.Connect(Address, Port);
            nwStream = client.GetStream();
        }

        public void Send(byte[] data)
        {
            nwStream.Write(data, 0, data.Length);
        }
        public void CloseConnection()
        {
            client.Close();
        }
    }
}
