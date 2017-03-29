using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientContext.DateTimeService
{
    public class AmbientDateTimeService : AmbientService<IDateTimeService>, IDateTimeService
    {
        protected override IDateTimeService DefaultCreate()
        {
            return new DateTimeAdapter();
        }

        public DateTime Now => Instance.Now;
        public DateTime UtcNow => Instance.UtcNow;
        public DateTime Today => Instance.Today;
        public DateTime Tomorrow => Instance.Tomorrow;

        public IStopwatch CreateStopwatch() => Instance.CreateStopwatch();
        public IStopwatch StartNewStopwatch() => Instance.StartNewStopwatch();
    }
}
