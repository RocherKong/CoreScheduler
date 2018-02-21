using CoreScheduler.Event;
using CoreScheduler.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreScheduler
{
    public static class JobManager
    {
        private const uint _maxTimerInterval = 0xfffffffe;
        private static bool _useUtc = false;
        private static Timer _timer = new Timer(state => ScheduleJobs(), null, Timeout.Infinite, Timeout.Infinite);
        private static ScheduleCollection _schedules = new ScheduleCollection();
        private static readonly ISet<Tuple<Schedule, Task>> _running = new HashSet<Tuple<Schedule, Task>>();

        public static event Action<JobExceptionRaisedEvent> JobException;
        public static event Action<JobStartedEvent> JobStart;
        public static event Action<JobEndedEvent> JobEnd;
        internal static DateTime Now
        {
            get
            {
                return _useUtc ? DateTime.UtcNow : DateTime.Now;
            }
        }

       


        /// <summary>
        /// Initalize Schedules NextRunTime While Run Time. 
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        private static IEnumerable<Schedule> CalculateNextRun(IEnumerable<Schedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                //calc null nextruntime schedule
                if (schedule.CalculateNextRunTime == null)
                {
                    if (schedule.DelayRunFor > TimeSpan.Zero)
                    {
                        //delayed Job
                        schedule.NextRun = Now.Add(schedule.DelayRunFor);
                        _schedules.Add(schedule);
                    }
                    else
                    {
                        //run now
                        yield return schedule;
                    }
                    var hasAdded = false;
                    foreach (var child in schedule.AdditionalSchedules.Where(x => x.CalculateNextRunTime != null))
                    {
                        var nextRun = child.CalculateNextRunTime(Now.Add(child.DelayRunFor).AddMilliseconds(1));
                        if (!hasAdded || schedule.NextRun > nextRun)
                        {
                            schedule.NextRun = nextRun;
                            hasAdded = true;
                        }
                    }
                }
                else
                {
                    schedule.NextRun = schedule.CalculateNextRunTime(Now.Add(schedule.DelayRunFor));
                    _schedules.Add(schedule);
                }

                foreach (var childSchedule in schedule.AdditionalSchedules)
                {
                    if (childSchedule.CalculateNextRunTime == null)
                    {
                        if (childSchedule.DelayRunFor > TimeSpan.Zero)
                        {
                            //delayed Job
                            childSchedule.NextRun = Now.Add(childSchedule.DelayRunFor);
                            _schedules.Add(childSchedule);
                        }
                        else
                        {
                            //run now
                            yield return childSchedule;
                            continue;
                        }
                    }
                    else
                    {
                        childSchedule.NextRun = childSchedule.CalculateNextRunTime(Now.Add(childSchedule.DelayRunFor));
                        _schedules.Add(childSchedule);
                    }
                }
            }
        }

        private static void ScheduleJobs()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _schedules.Sort();
            if (!_schedules.Any())
            {
                return;
            }

            var firstJob = _schedules.First();
            if (firstJob.NextRun <= Now)
            {
                //run job
                RunJob(firstJob);
                if (firstJob.CalculateNextRunTime == null)
                {
                    // probably a ToRunNow().DelayFor() job,that's no NextRun Time
                }
                else
                {
                    firstJob.NextRun = firstJob.CalculateNextRunTime(Now.AddMilliseconds(1));
                }
                if (firstJob.NextRun <= Now || firstJob.PendingRunOnce)
                {
                    _schedules.Remove(firstJob);
                }
                firstJob.PendingRunOnce = false;
                ScheduleJobs();
                return;
            }
            var interval = firstJob.NextRun - Now;
            if (interval <= TimeSpan.Zero)
            {
                ScheduleJobs();
                return;
            }
            else
            {
                if (interval.TotalMilliseconds > _maxTimerInterval)
                {
                    interval = TimeSpan.FromMilliseconds(_maxTimerInterval);
                }
                _timer.Change(interval, interval);

            }
        }

        internal static void RunJob(Schedule schedule)
        {
            if (schedule.Disabled)
            {
                return;
            }

            lock (_running)
            {
                if (schedule.Reentrant != null
                    && _running.Any(t => ReferenceEquals(t.Item1.Reentrant, schedule.Reentrant)))
                {
                    //non-reentrant and exists current schedule running job.
                    return;
                }
            }
            Tuple<Schedule, Task> tuple = null;
            var task = new Task(() =>
            {
                var start = Now;
                JobStart?.Invoke(new JobStartedEvent
                {
                    Name = schedule.Name,
                    StartTime = start
                });
                var stopWatch = new Stopwatch();
                try
                {
                    stopWatch.Start();
                    schedule.Jobs.ForEach(action =>
                    {
                        //Record Time Duration.
                        Task.Factory.StartNew(action).Wait();
                    });
                }
                catch (Exception ex)
                {
                    if (JobException != null)
                    {
                        if (ex is AggregateException aggregate && aggregate.InnerExceptions.Count == 1)
                        {
                            ex = aggregate.InnerExceptions.Single();
                        }
                        JobException(new JobExceptionRaisedEvent
                        {
                            Name = schedule.Name,
                            Message = ex.Message,
                            Exception = ex,
                            RaiseTime = DateTime.Now
                        });
                    }
                }
                finally
                {
                    lock (_running)
                    {
                        _running.Remove(tuple);
                    }
                    JobEnd?.Invoke(new JobEndedEvent
                    {
                        Duration = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds),
                        Name = schedule.Name,
                        NextRun = schedule.NextRun,
                        StartTime = start,
                    });
                }
            }, TaskCreationOptions.PreferFairness);
            tuple = new Tuple<Schedule, Task>(schedule, task);
            lock (_running)
            {
                _running.Add(tuple);
            }
            task.Start();
        }
    }
}
