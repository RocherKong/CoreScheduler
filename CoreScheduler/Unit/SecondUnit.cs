using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class SecondUnit : UnitBase, ITimeRestrictableUnit
    {
        public SecondUnit(Schedule schedule, int duration) : base(schedule, duration)
        {
            this._schedule.CalculateNextRunTime = x => x.AddSeconds(_duration);
        }
        public Schedule GetSchedule => this._schedule;
    }
}
