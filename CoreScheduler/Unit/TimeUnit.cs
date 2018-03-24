using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public sealed class TimeUnit
    {
        private readonly int _interval;
        private readonly Schedule _schedule;
        public TimeUnit(Schedule schedule ,int interval)
        {
            _schedule = schedule;
            _interval = interval;
        }
    }
}
