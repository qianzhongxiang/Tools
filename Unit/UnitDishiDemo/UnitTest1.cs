using DishiDemo;
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
    }
}