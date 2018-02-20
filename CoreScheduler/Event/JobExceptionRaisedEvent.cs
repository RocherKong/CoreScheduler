using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Event
{
    public class JobExceptionRaisedEvent : EventBase
    {
        public String Message { get; set; }

        public DateTime RaiseTime { get; set; }

        public Exception Exception { get; set; }
    }
}
