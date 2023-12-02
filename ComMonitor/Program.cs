// See https://aka.ms/new-console-template for more information
using ComMonitor;

Console.WriteLine("Hello, World!");


var p1 = Setting.Instance.P1;
var p2 = Setting.Instance.P2;
var p1s = File.Open($"p1.txt", FileMode.Create);
var p2s = File.Open($"p2.txt", FileMode.Create);

p1.ReciveCallback = b =>
{
    var str = System.Text.Encoding.ASCII.GetBytes($"{Environment.NewLine}{Environment.NewLine}{DateTime.Now.ToLongTimeString()}{Environment.NewLine}");
    p1s.Write(str);
    p1s.Write(b);

    p2.Write(b);
};

p2.ReciveCallback = b =>
{
    var str = System.Text.Encoding.ASCII.GetBytes($"{Environment.NewLine}{DateTime.Now.ToLongTimeString()}{Environment.NewLine}");
    p2s.Write(str);
    p2s.Write(b);

    p1.Write(b);
};
p1.Connect();
p2.Connect();

Console.ReadLine();
