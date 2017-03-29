using System;

namespace AmbientContext.DateTimeService
{
    public interface IStopwatch
    {
        void Start();
        void Stop();
        void Restart();
        void Reset();
        TimeSpan Elapsed { get; }
        long ElapsedMilliseconds { get; }
        long ElapsedTicks { get; }
        bool IsRunning { get; }
    }
}