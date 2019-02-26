using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        public void Send(Bitmap data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(nwStream, data);
        }
        public void CloseConnection()
        {
            client.Close();
        }
    }
}
