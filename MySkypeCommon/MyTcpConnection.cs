using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyUdp
{
    public class MyTcpConnection
    {
        protected string Address { get; set; }
        protected int Port { get; set; }

        public MyTcpConnection(string address, int port)
        {
            Address = address;
            Port = port;
        }
    }
}
