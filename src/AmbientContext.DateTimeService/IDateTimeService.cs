using System;

namespace AmbientContext.DateTimeService
{
    public interface IDateTimeService
    {
        /// <summary>
        /// Gets a System.DateTime object that is set to the current date and time on this computer, expressed as the local time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets a System.DateTime object that is set to the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the current date
        /// </summary>
        DateTime Today { get; }

        /// <summary>
        /// Gets the date 1 day from Now
        /// </summary>
        DateTime Tomorrow { get; }

        /// <summary>
        /// Return a new instance of Stopwatch via IStopwatch
        /// </summary>
        IStopwatch CreateStopwatch { get; }

        /// <summary>
        /// Return a new instance of Stopwatch and start it running via IStopwatch
        /// </summary>
        IStopwatch StartNewStopwatch { get; }
    }
}