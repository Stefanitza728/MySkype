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
        protected Action<object> CallbackForReceiveData { get; set; }
        protected bool IsOpen { get; set; }

        public MyTcpServer(string address, int port) : base(address, port) {
            Listener = new TcpListener(IPAddress.Parse(address), port);
            Clients = new List<TcpClient>();
            ProcessClientsTasks = new List<Task>();
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
            foreach(var client in Clients)
            {
                try
                {
                    var clientStream = client.GetStream();
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
            Listener.Stop();
        }

    }
}
