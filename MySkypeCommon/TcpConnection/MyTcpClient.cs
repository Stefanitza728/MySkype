using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyTcp

{
    public class MyTcpClient : MyTcpConnection
    {
        protected TcpClient TcpConnection { get; set; }
        protected NetworkStream NetworkStream { get; set; }

        public MyTcpClient(string address, int port) : base(address,port)
        {
            Formatter = new BinaryFormatter();
        }

        public void OpenConnection()
        {
            TcpConnection = new TcpClient();
            TcpConnection.Connect(Address, Port);
            NetworkStream = TcpConnection.GetStream();
        }

        public void Send(object data)
        {
            if(!NetworkStream.CanWrite)
            {
                return;
            }
            try
            {
                Formatter.Serialize(NetworkStream, data);
            }
            catch (System.IO.IOException ex)
            {

            }
            catch (System.ObjectDisposedException ex)
            {

            }

            
        }

        public void CloseConnection()
        {
            TcpConnection.Close();
            NetworkStream.Close();
        }
    }
}
