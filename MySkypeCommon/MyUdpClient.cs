using System.Net;
using System.Net.Sockets;
using System.Text;
using MyUdp;

namespace MyUdp
{
    public class MyUdpClient : MyUdpConnection
    {
        public MyUdpClient(string address, int port) :base(address,port)
        {

        }

        public void OpenConnection()
        {
            Client = new UdpClient();
            Client.Connect(Address,Port);
        }

        public void Send(byte[] data)
        {
            Client.SendAsync(data, data.Length);
        }

    }
}
