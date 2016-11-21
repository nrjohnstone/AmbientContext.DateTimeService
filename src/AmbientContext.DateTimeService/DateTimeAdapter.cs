using System;

namespace AmbientContext.DateTimeService
{
    internal class DateTimeAdapter : IDateTime
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Today => DateTime.Today;
        public DateTime Tomorrow => DateTime.Now.AddDays(1);
    }
}