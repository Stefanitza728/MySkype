using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUdp;
using NAudio.Wave;

namespace MySkypeCommon
{
    public class MyDataComunication
    {
        protected MyUdpClient UdpClient { get; set; }
        protected MyUdpServer UdpServer { get; set; }
        protected Task RecordProcess { get; set; }
        protected Task PlayProcess { get; set; }
        protected WaveInEvent WaveIn { get; set; }
        protected WaveOut WaveOut { get; set; }
        protected bool IsRecording { get; set; }
        public MyDataComunication OpenCommunication(string ipAddress)
        {
            UdpClient = new MyUdpClient(ipAddress, 27000);
            UdpClient.OpenConnection();
            IsRecording = true;
            RecordProcess = Task.Run(() => {
                WaveIn = new WaveInEvent();
                WaveIn.DataAvailable += new EventHandler<WaveInEventArgs>((s, x) => {
                    UdpClient.Send(x.Buffer);
                });

                WaveIn.StartRecording();
                while (IsRecording) ;
                WaveIn.StopRecording();
            });


            var bufferProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));

            WaveOut = new WaveOut();
            WaveOut.Init(bufferProvider);
            WaveOut.Play();

            UdpServer = new MyUdpServer(Constants.LocalLisenerConnection, 27000);
            UdpServer.AcceptClients((bytes) => {
                bufferProvider.AddSamples(bytes, 0, bytes.Length);
            });
            PlayProcess = Task.Run(async () => await UdpServer.WaitForClients);
            return this;
        }

        public MyDataComunication CloseCommunication()
        {
            IsRecording = false;
            WaveOut.Stop();
            UdpClient.CloseConnection();
            UdpServer.CloseConnection();
            RecordProcess = null;
            PlayProcess = null;
            return this;
        }
    }
}
