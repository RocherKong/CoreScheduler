using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler
{
    public class JobFactory : IJobFactory
    {
        public IJob GetInstance<T>() where T : IJob
        {
            return Activator.CreateInstance<T>();   
        }
    }
}
