using System;
using System.Threading.Tasks;
using MySkypeNetworking;
using NAudio.Wave;
using Emgu.CV;
using MyTcp;

namespace MySkype
{
    public class MyDataComunication
    {
        protected MyUdpClient VoiceClient { get; set; }
        protected MyTcpClient ImageClient { get; set; }
        protected MyUdpServer VoiceServer { get; set; }
        protected MyTcpServer ImageServer { get; set; }
        protected WaveInEvent WaveIn { get; set; }
        protected WaveOut WaveOut { get; set; }
        protected VideoCapture Capture { get; set; }
        protected bool IsRecording { get; set; }
        public MyDataComunication OpenCommunication(string ipAddress,Action<object> callbackDataForImageProcessing)
        {
            ImageServer = new MyTcpServer(NetworkConstants.LocalLisenerConnection, 27001);
            ImageClient = new MyTcpClient(ipAddress, 27001);
            VoiceClient = new MyUdpClient(ipAddress, 27000);
            VoiceServer = new MyUdpServer(NetworkConstants.LocalLisenerConnection, 27000);
            ReciveImageAndProcess(callbackDataForImageProcessing);
            GrabImageAndSend();
            RecordSoundAndSend();
            ReceiveAndPlaySound();
            return this;
        }

        private void ReciveImageAndProcess(Action<object> callbackDataForImageProcessing)
        {
            if (callbackDataForImageProcessing == null)
                callbackDataForImageProcessing = d => { };

            ImageServer.AcceptClients(callbackDataForImageProcessing);
        }

        private void ReceiveAndPlaySound()
        {
            var bufferProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));

            WaveOut = new WaveOut();
            WaveOut.Init(bufferProvider);
            WaveOut.Play();

            VoiceServer.AcceptClients((bytes) =>
            {
                bufferProvider.AddSamples(bytes, 0, bytes.Length);
            });
            Task.Run(async () => await VoiceServer.ProcessClientTask);
        }

        private void GrabImageAndSend()
        {
            ImageClient.OpenConnection();
            Capture = new VideoCapture();
            var frame = new Mat();
            Capture.ImageGrabbed += (s, ex) =>
            {
                Capture.Read(frame);
                ImageClient.Send(frame.Bitmap);
            };
            Capture.Start();
        }

        private void RecordSoundAndSend()
        {
            VoiceClient.OpenConnection();
            IsRecording = true;
            WaveIn = new WaveInEvent();
            WaveIn.DataAvailable += new EventHandler<WaveInEventArgs>((s, x) =>
            {
                VoiceClient.Send(x.Buffer);
            });

            Task.Run(() =>
            {
                WaveIn.StartRecording();
                while (IsRecording) ;
                WaveIn.StopRecording();
            });
        }

        public MyDataComunication CloseCommunication()
        {
            IsRecording = false;
            WaveIn.StopRecording();
            WaveOut.Stop();
            Capture.Stop();
            ImageClient.CloseConnection();
            ImageServer.CloseConnection();
            VoiceClient.CloseConnection();
            VoiceServer.CloseConnection();
            return this;
        }
    }
}
