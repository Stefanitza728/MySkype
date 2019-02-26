using MyUdp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MySkypeCommon
{
    public class MyTcpServer : MyTcpConnection
    {
        public Task WaitForClients { get; set; }
        protected IPAddress localAdd { get; set; }
        protected TcpClient client { get; set; }
        protected TcpListener listener { get; set; }

        public MyTcpServer(string address, int port) : base(address, port) {
            localAdd = IPAddress.Parse(address);
            listener = new TcpListener(localAdd, port);
        }

        public void AcceptClients(Action<Bitmap> callbackForReceiveData)
        {
            listener.Start();
            Task.Factory.StartNew(() => client = listener.AcceptTcpClient());

            if (callbackForReceiveData == null)
                callbackForReceiveData = d => { };

            WaitForClients = Task.Run(() =>
            {
                while (true)
                    if (client != null)
                    {
                        Receive(callbackForReceiveData);
                    }
            });
        }

        protected void Receive(Action<Bitmap> callbackForReceiveData = null)
        {
            NetworkStream stream = client.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();
            Bitmap img = (Bitmap)formatter.Deserialize(stream);
            callbackForReceiveData(img);
        }

        public void CloseConnection()
        {
            client.Close();
            listener.Stop();
        }

    }
}
