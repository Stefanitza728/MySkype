using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTcp
{
    public class MyTcpConnection
    {
        protected string Address { get; set; }
        protected int Port { get; set; }
        protected BinaryFormatter Formatter { get; set; }

        public MyTcpConnection(string address, int port)
        {
            Address = address;
            Port = port;
            Formatter = new BinaryFormatter();
        }
    }
}
