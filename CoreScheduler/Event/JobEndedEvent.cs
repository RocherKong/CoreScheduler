using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Event
{
    /// <summary>
    /// Event Raise when Job End.
    /// </summary>
    public class JobEndedEvent : EventBase
    {
        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime? NextRun { get; set; }
    }
}
