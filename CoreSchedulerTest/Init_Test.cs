using CoreScheduler;
using System;
using System.Diagnostics;
using Xunit;

namespace CoreSchedulerTest
{
    public class Init_Test
    {
        [Fact]
        public void Init_Test1()
        {
            JobManager.Initialize(new MyRegister());
            Assert.NotEmpty("1");
        }
    }

    public class DemoJob : IJob
    {
        public void Execute()
        {
            Trace.WriteLine("Now:" + DateTime.Now);
            Console.WriteLine("Now:" + DateTime.Now);
        }
    }

    public class MyRegister : Registry
    {

        public MyRegister()
        {
            Schedule<DemoJob>().ToRunNow().AndEvery(5).SecondUnit();
        }
    }
}
