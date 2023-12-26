using ComMonitor;
using System;

namespace DishiDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var p = new Protcol { PortName = "" };
            var cd = new CD(p);
            p.ReciveCallback = cd.Recive;
        }
    }


    public class CD
    {
        private int state = 0;
        public CD(Protcol protcol)
        {
            p = protcol;
        }

        public Protcol p { get; }

        public void Recive(byte[] c)
        {
            switch (state)
            {
                case 0:
                    if (c[0] == 0x05 && c[1] == 0x38 && c[2] == 0x34 && c[3] == 0x30)
                    {
                        state = 1;
                        p.Write(new byte[] { 0x06 });
                    }
                    break;
                case 1:
                    if (c[0] == 0x02 && c[1] == 0x30)
                    {
                        state = 2;
                        p.Write(new byte[] { 0x06 });
                    }
                    break;
                case 2:
                    if (c[0] == 0x06)
                    {
                        state = 3;
                        p.Write(new byte[] { 0x73, 0x2F, 0x65, 0x78, 0x70, 0x6F, 0x72, 0x74, 0x2F, 0x50, 0x52, 0x4F, 0x44, 0x2F, 0x2F, 0x33, 0x35, 0x31, 0x63, 0x66, 0x32, 0x34, 0x32, 0x61, 0x5F, 0x38, 0x2E, 0x72, 0x74, 0x0D, 0x52, 0x4D, 0x3D, 0x0D, 0x49, 0x53, 0x3D, 0x4E, 0x4F, 0x52, 0x4D, 0x41, 0x4C, 0x0D, 0x44, 0x49, 0x45, 0x3D, 0x61, 0x31, 0x31, 0x0D, 0x03, 0x9C });
                    }
                    break;
                case 3:
                    if (c[0] == 0x06)
                    {
                        if (NotFiniteNumberException)
                        {
                            state = 4;
                        }
                        else
                        {
                            state = 3;
                        }
                        p.Write(new byte[] { 0x06 });
                    }
                    break;
                default:
                    break;
            }

        }
    }
}