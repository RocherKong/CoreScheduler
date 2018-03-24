using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreScheduler
{
    public class Schedule
    {
        public Schedule(Action action) : this(new[] { action })
        {

        }

        public Schedule(IEnumerable<Action> actions)
        {
            Disabled = false;
            Jobs = actions.ToList();
            AdditionalSchedules = new List<Schedule>();
            PendingRunOnce = false;
            Reentrant = null;
        }
        /// <summary>
        /// Name of Schedule
        /// </summary>
        public String Name { get; internal set; }
        /// <summary>
        /// Next Run time
        /// </summary>
        public DateTime NextRun { get; internal set; }

        /// <summary>
        /// Flag Status if Schedule is Disabled
        /// </summary>
        public bool Disabled { get; private set; }

        internal List<Action> Jobs { get; private set; }

        internal Func<DateTime, DateTime> CalculateNextRunTime { get; set; }

        internal TimeSpan DelayRunFor { get; set; }

        internal ICollection<Schedule> AdditionalSchedules { get; set; }

        internal Schedule Parent { get; set; }

        internal bool PendingRunOnce { get; set; }

        /// <summary>
        /// can be run while running.
        /// </summary>
        internal object Reentrant { get; set; }


        public Schedule NonReentrantInstance()
        {
            Reentrant = Reentrant ?? new object();
            return this;
        }

    }
}
