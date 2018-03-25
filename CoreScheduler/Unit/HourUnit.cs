using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class HourUnit : UnitBase, ITimeRestrictableUnit
    {
        public HourUnit(Schedule schedule, int duration) : base(schedule, duration)
        {
            this._schedule.CalculateNextRunTime = x => x.AddHours(_duration);
        }
        public Schedule GetSchedule => this._schedule;
    }
}
