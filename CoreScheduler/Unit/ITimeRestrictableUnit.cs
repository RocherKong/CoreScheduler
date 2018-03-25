using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public interface ITimeRestrictableUnit
    {
        Schedule GetSchedule { get; }
    }
}
