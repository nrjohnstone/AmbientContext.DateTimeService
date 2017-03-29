using System;

namespace AmbientContext.DateTimeService
{
    internal class DateTimeAdapter : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Today => DateTime.Today;
        public DateTime Tomorrow => DateTime.Now.AddDays(1);
        public IStopwatch CreateStopwatch() => new StopwatchAdapter();

        public IStopwatch StartNewStopwatch()
        {            
            var stopwatch = new StopwatchAdapter();
            stopwatch.Start();
            return stopwatch;                            
        }

        
    }
}