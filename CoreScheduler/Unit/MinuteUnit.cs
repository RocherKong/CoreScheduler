using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class MinuteUnit : UnitBase, ITimeRestrictableUnit
    {
        public MinuteUnit(Schedule schedule, int duration) : base(schedule, duration)
        {
            this._schedule.CalculateNextRunTime = x => x.AddMinutes(duration);
        }
        public Schedule GetSchedule => this._schedule;
    }
}
