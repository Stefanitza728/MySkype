using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MyUdp;

namespace MyUdp
{
    public class MyUdpServer: MyUdpConnection
    {
        public Task WaitForClients{ get;set;}
        public bool IsOpen{get;set;}
        public MyUdpServer(string address, int port):base(address,port)
        {
        }
       
        public void AcceptClients(Action<byte[]> callbackForReceiveData)
        {
            IsOpen = true;
            Client = new UdpClient(Port);

            if (callbackForReceiveData == null)
                callbackForReceiveData = d => { };

            WaitForClients = Task.Run(async () => {
                    while(IsOpen)
                        await Receive(callbackForReceiveData);
            });
        }

        protected async Task Receive(Action<byte[]> callbackForReceiveData = null)
        {
            var udpResult = await Client.ReceiveAsync();
            byte[] receivedData = udpResult.Buffer;
            callbackForReceiveData(receivedData);
        }

        public override MyUdpConnection CloseConnection()
        {
            IsOpen = false;
            return base.CloseConnection();
        }
    }
}
