using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public class UnitBase
    {
        protected readonly int _duration;
        protected readonly Schedule _schedule;
        public UnitBase(Schedule schedule, int duration)
        {
            this._duration = duration;
            this._schedule = schedule;
        }
    }
}
