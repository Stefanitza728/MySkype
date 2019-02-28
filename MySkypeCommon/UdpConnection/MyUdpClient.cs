using System.Net.Sockets;

namespace MySkypeNetworking
{
    public class MyUdpClient : MyUdpConnection
    {
        public MyUdpClient(string address, int port) :base(address,port)
        {

        }

        public void OpenConnection()
        {
            UdpConnection = new UdpClient();
            UdpConnection.Connect(Address,Port);
        }

        public void Send(byte[] data)
        {
            UdpConnection.SendAsync(data, data.Length);
        }

    }
}
