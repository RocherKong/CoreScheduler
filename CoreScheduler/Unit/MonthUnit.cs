using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class MonthUnit : UnitBase, ITimeRestrictableUnit
    {
        public MonthUnit(Schedule schedule, int duration) : base(schedule, duration)
        {
            this._schedule.CalculateNextRunTime = x => x.AddMonths(duration);
        }

        public Schedule GetSchedule => this._schedule;
    }
}
