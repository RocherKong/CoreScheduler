using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Event
{
    public class JobStartedEvent : EventBase
    {
        public DateTime StartTime { get; set; }
    }
}
