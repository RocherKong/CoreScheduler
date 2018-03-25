using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class DayUnit : UnitBase, ITimeRestrictableUnit
    {
        public DayUnit(Schedule schedule, int duration) : base(schedule, duration)
        {
            this._schedule.CalculateNextRunTime = x => x.AddDays(_duration);
        }
        public Schedule GetSchedule => this._schedule;
    }
}
