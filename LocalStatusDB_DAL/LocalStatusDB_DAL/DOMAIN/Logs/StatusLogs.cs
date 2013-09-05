using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUFStatus.Domain.Local.App;

namespace TUFStatus.Domain.Local.Logs
{
    public class StatusLogs
    {
        public virtual int local_status_log_id { get; set; }
        //public virtual int installation_id { get; set; }
        public virtual int application_id { get; set; }
        public virtual Nullable<DateTime> status_time { get; set; }
        public virtual Int16 is_transferred { get; set; }

        public virtual Installations installation { get; set; }
    }
}
