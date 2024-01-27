using DishiDemo;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace UnitDishiDemo
{
    public class UnitTest1
    {
        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        public ITestOutputHelper TestOutputHelper { get; }

        [Fact]
        public void Test1()
        {
            var res = DataSolver.WrapData(DataSolver.SingleLineData(1, 1));
            var ress = string.Join(" ", res.Select(i => i.ToString("x")));
            TestOutputHelper.WriteLine(ress);
        }
        [Fact]
        public void Test2()
        {
            var res = DataSolver.MultiData(new (double, double)[] { (90566.81, 118942.98), (91274.17, 116942.09), (20, 20) });
            var ress = string.Join(" ", res.Select(i => i.ToString("x")));
            TestOutputHelper.WriteLine(ress);
        }
        [Fact]
        public void Test3()
        {
            var res = DataSolver.WrapData(new byte[] { 0x73, 0x2F, 0x65, 0x78, 0x70, 0x6F, 0x72, 0x74, 0x2F, 0x50, 0x52, 0x4F, 0x44, 0x2F, 0x2F, 0x33, 0x35, 0x31, 0x63, 0x66, 0x32, 0x34, 0x32, 0x61, 0x5F, 0x38, 0x2E, 0x72, 0x74, 0x0D, 0x52, 0x4D, 0x3D, 0x0D, 0x49, 0x53, 0x3D, 0x4E, 0x4F, 0x52, 0x4D, 0x41, 0x4C, 0x0D, 0x44, 0x49, 0x45, 0x3D, 0x61, 0x31, 0x31, 0x0D });
            var ress = string.Join(" ", res.Select(i => i.ToString("x")));
            TestOutputHelper.WriteLine(ress);
        }
        [Fact]
        public void calc()
        {
            var str = "73 2F 65 78 70 6F 72 74 2F 50 52 4F 44 2F 2F 33 35 31 63 66 32 34 32 61 5F 38 2E 72 74 0D 52 4D 3D 0D 49 53 3D 4E 4F 52 4D 41 4C 0D 44 49 45 3D 61 31 31 0D";
            var array = str.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            int res = 0x9f;
            foreach (var item in array)
            {
                res = res + byte.Parse(item, System.Globalization.NumberStyles.HexNumber);
            }
            var resByte = (byte)res;
            var diff = 0x37 - resByte;
            TestOutputHelper.WriteLine(resByte.ToString());
            TestOutputHelper.WriteLine(resByte.ToString("x"));
            TestOutputHelper.WriteLine(diff.ToString());
            TestOutputHelper.WriteLine(diff.ToString("x"));
        }
        [Fact]
        public void calc1()
        {
            var str = "73 2F 65 78 70 6F 72 74 2F 50 52 4F 44 2F 2F 33 35 31 61 35 70 30 30 61 5F 30 37 2E 72 74 0D 52 4D 3D 0D 49 53 3D 4E 4F 52 4D 41 4C 0D 44 49 45 3D 61 31 31 0D";
            var array = str.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            int res = 0x17;
            foreach (var item in array)
            {
                res = res + byte.Parse(item, System.Globalization.NumberStyles.HexNumber);
            }
            var resByte = (byte)res;
            var diff = 0xf5- resByte;
            TestOutputHelper.WriteLine(resByte.ToString());
            TestOutputHelper.WriteLine(resByte.ToString("x"));
            TestOutputHelper.WriteLine(diff.ToString());
            TestOutputHelper.WriteLine(diff.ToString("x"));
        }
        [Fact]
        public void calc2()
        {
            var str = "73 2F 65 78 70 6F 72 74 2F 50 52 4F 44 2F 2F 33 35 31 63 66 32 34 32 61 5F 38 2E 72 74 0D 52 4D 3D 0D 49 53 3D 4E 4F 52 4D 41 4C 0D 44 49 45 3D 61 31 31 0D";
            var array = str.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            int res = 0x16;
            foreach (var item in array)
            {
                res = res + byte.Parse(item, System.Globalization.NumberStyles.HexNumber);
            }
            var resByte = (byte)res;
            var diff = 0xce - resByte;
            TestOutputHelper.WriteLine(resByte.ToString());
            TestOutputHelper.WriteLine(resByte.ToString("x"));
            TestOutputHelper.WriteLine(diff.ToString());
            TestOutputHelper.WriteLine(diff.ToString("x"));
        }
        [Fact]
        public void calc21()
        {
            //30 35 34 44 52 
            var str = "73 2F 65 78 70 6F 72 74 2F 50 52 4F 44 2F 2F 33 35 31 63 66 32 34 32 61 5F 38 2E 72 74 0D 52 4D 3D 0D 49 53 3D 4E 4F 52 4D 41 4C 0D 44 49 45 3D 61 31 31 0D";
            var array = str.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            int res = 0xce;
            foreach (var item in array)
            {
                res = res + byte.Parse(item, System.Globalization.NumberStyles.HexNumber);
            }
            var resByte = (byte)res;
            var diff = 0x16 - resByte;
            TestOutputHelper.WriteLine(resByte.ToString());
            TestOutputHelper.WriteLine(resByte.ToString("x"));
            TestOutputHelper.WriteLine(diff.ToString());
            TestOutputHelper.WriteLine(diff.ToString("x"));
        }
    }
}