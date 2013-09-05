using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUFStatus.Domain.Cloud.App
{
    public class Applications
    {
        public virtual int application_id { get; set; }
        public virtual string application_desc { get; set; }
    }
}
