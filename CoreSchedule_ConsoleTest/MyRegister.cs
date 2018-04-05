using CoreScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSchedule_ConsoleTest
{
    public class MyRegister : Registry
    {
        public MyRegister()
        {
            Schedule<DemoJob>().ToRunNow().AndEvery(3).DelaySecondUnit(10);
        }
    }
}
