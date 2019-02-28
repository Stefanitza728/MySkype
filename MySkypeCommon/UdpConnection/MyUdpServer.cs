using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MySkypeNetworking
{
    public class MyUdpServer: MyUdpConnection
    {
        public Task ProcessClientTask{ get;set;}
        public bool IsOpen{get;set;}
        public MyUdpServer(string address, int port):base(address,port)
        {
        }
       
        public void AcceptClients(Action<byte[]> callbackForReceiveData)
        {
            IsOpen = true;
            UdpConnection = new UdpClient(Port);

            if (callbackForReceiveData == null)
                callbackForReceiveData = a => { };

            ProcessClientTask = Task.Run(async () => {

                    while(IsOpen)
                        await Receive(callbackForReceiveData);

            });
        }

        protected async Task Receive(Action<byte[]> callbackForReceiveData = null)
        {
            var udpResult = await UdpConnection.ReceiveAsync();
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
