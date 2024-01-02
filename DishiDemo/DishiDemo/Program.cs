using ComMonitor;
using System;
using System.Security.Cryptography.X509Certificates;

namespace DishiDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var p = new Protcol { PortName = "" };
            p.Connect();
            var cd = new DataSolver(p);
        }
    }


    public class DataSolver
    {
        private int state = 0;
        public DataSolver(Protcol protcol)
        {
            p = protcol;
            p.ReciveCallback = Recive;

        }

        public Protcol p { get; }
        private bool Finished = false;
        public void Recive(byte[] c)
        {
            switch (state)
            {
                case 0:
                    if (c[0] == 0x05 && c[1] == 0x38 && c[2] == 0x34 && c[3] == 0x30)
                    {
                        state = 1;
                        p.Write(new byte[] { 0x06 });
                        Finished = false;
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
                        if (Finished)
                        {
                            state = 0;
                            p.Write(new byte[] { 0x04 });
                        }
                        else
                        {
                            var res = DataSolver.WrapData(DataSolver.SingleLineData(1, 1));
                            //var res = DataSolver.MultiData(new (double, double)[] { (90566.81, 118942.98), (91274.17, 116942.09), (20, 20) });

                            p.Write(res);

                            Finished = true;
                        }

                    }
                    break;
                default:
                    break;
            }

        }
        public static byte[] MultiData((double x, double y)[] values)
        {
            byte[] res = new byte[values.Length * 15 + 2];
            for (int i = 0; i < values.Length; i++)
            {
                var x = values[i].x;
                var y = values[i].y;
                var xi = (int)System.Math.Round(x);
                var x1 = Convert.ToByte((xi % 100).ToString(), 16);
                var x2 = Convert.ToByte((xi / 100 % 100).ToString(), 16);
                var x3 = Convert.ToByte((xi / 10000 % 100).ToString(), 16);
                var yi = (int)System.Math.Round(y);
                var y1 = Convert.ToByte((yi % 100).ToString(), 16);
                var y2 = Convert.ToByte((yi / 100 % 100).ToString(), 16);
                var y3 = Convert.ToByte((yi / 10000 % 100).ToString(), 16);
                byte[] data = new byte[] { x1, x2, x3, 0x00, y1, y2, y3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                data.CopyTo(res, i * 15);
            }
            res[res.Length - 2] = 0x03;
            res[res.Length - 1] = 0xEE;
            return res;
        }
        public static byte[] SingleLineData(double x, double y)
        {
            var xi = (int)System.Math.Round(x);
            var x1 = Convert.ToByte((xi % 100).ToString(), 16);
            var x2 = Convert.ToByte((xi / 100 % 100).ToString(), 16);
            var x3 = Convert.ToByte((xi / 10000 % 100).ToString(), 16);
            var yi = (int)System.Math.Round(y);
            var y1 = Convert.ToByte((yi % 100).ToString(), 16);
            var y2 = Convert.ToByte((yi / 100 % 100).ToString(), 16);
            var y3 = Convert.ToByte((yi / 10000 % 100).ToString(), 16);
            return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, x1, x2, x3, 0x00, y1, y2, y3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        public static byte[] WrapData(byte[] data)
        {
            var dl = data.Length + 1;
            var bytes = System.Text.Encoding.ASCII.GetBytes(dl.ToString());
            var res = new byte[dl + 2 + 2 + 2];
            bytes.CopyTo(res, 2);
            res[0] = 0x02;
            res[1] = 0x30;
            res[4] = 0x44;
            res[5] = 0x52;
            res[6] = 0x01;
            data.CopyTo(res, 7);
            byte check = 0;
            for (int i = 1; i < res.Length; i++)
            {
                check -= res[i];
            }

            //var checkbytes = BitConverter.GetBytes((check & 0xffff));
            res[res.Length - 2] = 0x03;
            res[res.Length - 1] = check;

            return res;
        }
    }
}