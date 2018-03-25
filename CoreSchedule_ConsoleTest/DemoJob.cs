using CoreScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSchedule_ConsoleTest
{
    public class DemoJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Now:" + DateTime.Now);
        }
    }
}
