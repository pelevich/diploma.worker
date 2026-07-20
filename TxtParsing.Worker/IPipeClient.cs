using System;
using System.Collections.Generic;
using System.Text;

namespace TxtParsing.Worker
{
    public interface IPipeClient
    {
        bool IsConnected { get; set; }
        public void ConnectedServer(string server_name);
        public void Write(string message);
        public string Read(int bufferSize = 4096);
    }
}
