﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Unit
{
    public sealed class TimeUnit
    {
        private readonly int _duration;
        private readonly Schedule _schedule;
        public TimeUnit(Schedule schedule, int interval)
        {
            _schedule = schedule;
            _duration = interval;
        }

        public MillisecondUnit MillisecondUnit()
        {
            return new Unit.MillisecondUnit(_schedule, _duration);
        }

        public SecondUnit SecondUnit()
        {
            return new SecondUnit(_schedule, _duration);
        }

        public MinuteUnit MinuteUnit()
        {
            return new Unit.MinuteUnit(_schedule, _duration);
        }
    }
}
