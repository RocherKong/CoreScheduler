using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class WeekUnit : UnitBase, ITimeRestrictableUnit
    {
        public WeekUnit(Schedule schedule, int duration) : base(schedule, duration)
        {
            this._schedule.CalculateNextRunTime = x => x.AddDays(_duration * 7);
        }
        public Schedule GetSchedule => this._schedule;
    }
}
