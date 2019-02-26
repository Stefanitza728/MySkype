using MyUdp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

        public void AcceptClients(Action<byte[]> callbackForReceiveData)
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

        protected void Receive(Action<byte[]> callbackForReceiveData = null)
        {
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            callbackForReceiveData(buffer);
        }

        public void CloseConnection()
        {
            client.Close();
            listener.Stop();
        }

    }
}
