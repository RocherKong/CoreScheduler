using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler
{
    public class Registry
    {
        private bool _allJobsConfiguredAsNonReentrant;
        internal List<Schedule> Schedules { get; private set; }

        public Registry()
        {
            this._allJobsConfiguredAsNonReentrant = false;
            Schedules = new List<Schedule>();
        }

        public void NonReentrantAsDefault()
        {
            _allJobsConfiguredAsNonReentrant = true;
            lock (((ICollection)Schedules).SyncRoot)
            {
                foreach (var schedule in Schedules)
                {
                    schedule.NonReentrantInstance();
                }
            }
        }

        public Schedule Schedule(Action job)
        {
            if (job == null)
            {
                throw new ArgumentNullException("job can't be null!");
            }
            return Schedule(job, null);
        }

        public Schedule Schedule(IJob job)
        {
            if (job == null)
            {
                throw new ArgumentNullException("job can't be null!");
            }
            return Schedule(JobManager.GetJobAction(job), null);
        }

        public Schedule Schedule(Func<IJob> job)
        {
            if (job == null)
            {
                throw new ArgumentNullException("Job can't be null!");
            }
            return Schedule(JobManager.GetJobAction(job), null);
        }

        public Schedule Schedule<T>() where T : IJob
        {
            return Schedule(JobManager.GetJobAction<T>(), typeof(T).FullName);
        }

        public Schedule Schedule(Action action, String name)
        {
            var schedule = new Schedule(action);
            if (_allJobsConfiguredAsNonReentrant)
            {
                schedule.NonReentrantInstance();
            }
            lock (((ICollection)Schedules).SyncRoot)
            {
                Schedules.Add(schedule);
            }
            schedule.Name = name;
            return schedule;
        }
    }


}
