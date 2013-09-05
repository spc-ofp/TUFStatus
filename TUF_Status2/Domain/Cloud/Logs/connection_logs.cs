using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUF_Status.Domain.Cloud.Logs
{
    public class connection_logs
    {
        public virtual int connection_id { get; set; }
        public virtual int installation_id { get; set; }
        public virtual int application_id { get; set; }
        public virtual Nullable<DateTime> connection_time { get; set; }
        public virtual Nullable<DateTime> disconnection_time { get; set; }
    }
}
