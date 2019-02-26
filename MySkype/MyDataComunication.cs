using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUdp;
using NAudio.Wave;
using System.Drawing;

namespace MySkypeCommon
{
    public class MyDataComunication
    {
        protected MyUdpClient VoiceUdpClient { get; set; }
        protected MyTcpClient ImageUdpClient { get; set; }
        protected MyUdpServer VoiceUdpServer { get; set; }
        protected MyTcpServer ImageUdpServer { get; set; }
        protected Task RecordProcess { get; set; }
        protected Task PlayProcess { get; set; }
        protected WaveInEvent WaveIn { get; set; }
        protected WaveOut WaveOut { get; set; }
        protected bool IsRecording { get; set; }
        public MyDataComunication OpenCommunication(string ipAddress,Action<Bitmap> callbackDataForImageProcessing)
        {
            VoiceUdpClient = new MyUdpClient(ipAddress, 27000);
            VoiceUdpClient.OpenConnection();

            ImageUdpServer = new MyTcpServer(Constants.LocalLisenerConnection, 27001);
            ImageUdpServer.AcceptClients(callbackDataForImageProcessing);

            ImageUdpClient = new MyTcpClient(ipAddress, 27001);
            ImageUdpClient.OpenConnection();

            IsRecording = true;
            RecordProcess = Task.Run(() => {
                WaveIn = new WaveInEvent();
                WaveIn.DataAvailable += new EventHandler<WaveInEventArgs>((s, x) => {
                    VoiceUdpClient.Send(x.Buffer);
                });

                WaveIn.StartRecording();
                while (IsRecording) ;
                WaveIn.StopRecording();
            });

            var bufferProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));

            WaveOut = new WaveOut();
            WaveOut.Init(bufferProvider);
            WaveOut.Play();

            VoiceUdpServer = new MyUdpServer(Constants.LocalLisenerConnection, 27000);
            VoiceUdpServer.AcceptClients((bytes) => {
                bufferProvider.AddSamples(bytes, 0, bytes.Length);
            });
            PlayProcess = Task.Run(async () => await VoiceUdpServer.WaitForClients);

           
            return this;
        }


        public void SendImage(Bitmap byteArray)
        {
            ImageUdpClient.Send(byteArray);
        }

        public MyDataComunication CloseCommunication()
        {
            IsRecording = false;
            WaveOut.Stop();
            VoiceUdpClient.CloseConnection();
            ImageUdpClient.CloseConnection();
            VoiceUdpServer.CloseConnection();
            RecordProcess = null;
            PlayProcess = null;
            return this;
        }
    }
}
