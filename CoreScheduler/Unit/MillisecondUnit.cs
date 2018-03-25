using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class MillisecondUnit : ITimeRestrictableUnit
    {
        private readonly Schedule _schedule;

        private readonly int _duration;
        public MillisecondUnit(Schedule schedule, int duration)
        {
            this._schedule = schedule;
            this._duration = duration;
            _schedule.CalculateNextRunTime = x => x.AddMilliseconds(_duration);
        }

        public Schedule GetSchedule
        {
            get
            {
                return this._schedule;
            }
        }
    }
}
