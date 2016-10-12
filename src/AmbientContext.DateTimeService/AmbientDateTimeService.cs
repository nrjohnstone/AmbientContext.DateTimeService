using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientContext.DateTimeService
{
    public class AmbientDateTimeService : AmbientService<IDateTime>, IDateTime
    {
        protected override IDateTime DefaultCreate()
        {
            return new DateTimeAdapter();
        }

        public DateTime Now => Instance.Now;
        public DateTime UtcNow => Instance.UtcNow;
        public DateTime Today => Instance.Today;
        public DateTime Tomorrow => Instance.Tomorrow;
    }
}
