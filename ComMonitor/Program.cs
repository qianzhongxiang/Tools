// See https://aka.ms/new-console-template for more information
using ComMonitor;
using System;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, World!");


        var p1 = Setting.Instance.P1;
        var p2 = Setting.Instance.P2;
        var p1s = File.Open($"p1.txt", FileMode.Create, FileAccess.Write, FileShare.Read);
        var p2s = File.Open($"p2.txt", FileMode.Create, FileAccess.Write, FileShare.Read);

        p1.ReciveCallback = b =>
        {
            var s = string.Join(" ", b.Select(i => i.ToString("x")));
            var str = System.Text.Encoding.ASCII.GetBytes($"{Environment.NewLine}{DateTime.Now.ToString("HH:mm:ss:ffff")}{Environment.NewLine}{s}{Environment.NewLine}");
            p1s.Write(str, 0, str.Length);
            p1s.Write(b, 0, b.Length);
            p1s.Flush();

            p2.Write(b);
        };

        p2.ReciveCallback = b =>
        {
            var s = string.Join(" ", b.Select(i => i.ToString("x")));
            var str = System.Text.Encoding.ASCII.GetBytes($"{Environment.NewLine}{DateTime.Now.ToString("HH:mm:ss:ffff")}{Environment.NewLine}{s}{Environment.NewLine}");
            p2s.Write(str, 0, str.Length);
            p2s.Write(b, 0, b.Length);
            p2s.Flush();

            p1.Write(b);
        };
        p1.Connect();
        p2.Connect();

        Console.ReadLine();
    }
}