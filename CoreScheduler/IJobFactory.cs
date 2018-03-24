using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler
{
    public interface IJobFactory
    {
        IJob GetInstance<T>() where T : IJob;
    }
}
