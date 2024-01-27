using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComMonitor
{
    public class Protcol : IDisposable
    {
        System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort();
        public string PortName { get; set; }
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public int StopBits { get; set; } = 1;
        public int Parity { get; set; } = 0;

        public Action<byte[]> ReciveCallback { get; set; }
        public void Connect()
        {
            port.PortName = PortName;
            port.BaudRate = BaudRate;
            port.DataBits = DataBits;
            port.StopBits = (System.IO.Ports.StopBits)StopBits;
            port.Parity = (System.IO.Ports.Parity)Parity;
            port.ReadTimeout = port.WriteTimeout = 45000;
            port.Open();
            Task.Run(() =>
            {
                while (true)
                {
                    var c = port.BytesToRead;
                    if (c > 0)
                    {
                        var bytes = new byte[c];
                        port.Read(bytes, 0, c);
                        ReciveCallback?.Invoke(bytes);
                    }
                    System.Threading.Thread.Sleep(100);
                }
            });
        }

        private object locker = new object();
        public void Write(byte[] bs)
        {
            if (port.IsOpen)
            {
                lock (locker)
                {
                    port.Write(bs, 0, bs.Length);
                }
            }
        }
        public void Close()
        {
            port.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
