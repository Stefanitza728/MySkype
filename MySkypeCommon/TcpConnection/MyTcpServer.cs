using MySkypeNetworking;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyTcp
{
    public class MyTcpServer : MyTcpConnection
    {
        public ICollection<Task> ProcessClientsTasks { get; set; }
        protected TcpListener Listener { get; set; }
        protected ICollection<TcpClient> Clients { get; set; }
        protected ICollection<NetworkStream> ClientsStreams { get; set; }
        protected Action<object> CallbackForReceiveData { get; set; }
        protected bool IsOpen { get; set; }

        public MyTcpServer(string address, int port) : base(address, port) {
            Listener = new TcpListener(IPAddress.Parse(address), port);
            Clients = new List<TcpClient>();
            ProcessClientsTasks = new List<Task>();
            ClientsStreams = new List<NetworkStream>();
        }

        public void AcceptClients(Action<object> callbackForReceiveData)
        {
            CallbackForReceiveData = callbackForReceiveData;
            IsOpen = true;
            Listener.Start();

            ProcessClientsTasks.Add(Task.Run(() =>
            {
                try
                {
                    while (IsOpen)
                    {
                        var client = Listener.AcceptTcpClient();
                        Clients.Add(client);
                        ClientsStreams.Add(client.GetStream());
                    }
                }
                catch(System.Net.Sockets.SocketException ex)
                {

                }
            }));

            ProcessClientsTasks.Add(Task.Run(() =>
            {
                while (IsOpen)
                    if (Clients.Count > 0)
                    {
                        Receive();
                    }
            }));
        }

        protected void Receive()
        {
            foreach(var clientStream in ClientsStreams)
            {
                try
                {
                    var data = Formatter.Deserialize(clientStream);
                    CallbackForReceiveData(data);
                }
                catch (System.Runtime.Serialization.SerializationException ex)
                {

                }
            }
        }

        public void CloseConnection()
        {
            IsOpen = false;
            foreach(var client in Clients)
            {
                client.Close();
            }
            foreach(var clientStream in ClientsStreams)
            {
                clientStream.Close();
            }
            Listener.Stop();
        }

    }
}
