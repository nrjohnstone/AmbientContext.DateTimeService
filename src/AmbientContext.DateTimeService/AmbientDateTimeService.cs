using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientContext.DateTimeService
{
    public class AmbientDateTimeService : AmbientService<IDateTime>
    {
        protected override IDateTime DefaultCreate()
        {
            return new DateTimeAdapter();
        }
    }
}
