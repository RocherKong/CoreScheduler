using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class SepcificTimeUnit
    {
        internal Schedule Schedule { get; private set; }
        internal SepcificTimeUnit(Schedule schedule)
        {
            Schedule = schedule;
        }

        public TimeUnit AndEvery(int interval)
        {
            var Parent = Schedule.Parent ?? Schedule;
            var Child = new Schedule(Schedule.Jobs)
            {
                Parent=Parent,
                Reentrant=Parent.Reentrant,
                Name=Parent.Name
            };
            if (Parent.CalculateNextRunTime != null) {
                var now = JobManager.Now;
                var delay = Parent.CalculateNextRunTime(now) - now;
                if (delay>TimeSpan.Zero)
                {
                    Child.DelayRunFor = delay;
                }
            }
            Child.Parent.AdditionalSchedules.Add(Child);
            return Child.ToRunEvery(interval);
        }

    }
}
