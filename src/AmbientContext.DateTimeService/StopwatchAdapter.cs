using System;
using System.Diagnostics;

namespace AmbientContext.DateTimeService
{
    internal class StopwatchAdapter : IStopwatch
    {
        private readonly Stopwatch _stopwatch;

        public StopwatchAdapter()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start() => _stopwatch.Start();
        public void Stop() => _stopwatch.Stop();
        public void Restart() => _stopwatch.Restart();
        public void Reset() => _stopwatch.Reset();
        public TimeSpan Elapsed => _stopwatch.Elapsed;
        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
        public long ElapsedTicks => _stopwatch.ElapsedTicks;
        public bool IsRunning => _stopwatch.IsRunning;
    }
}