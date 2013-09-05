using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUFStatus.Domain.Cloud.App;

namespace TUFStatus.Domain.Cloud.Logs
{
    public class StatusLogs
    {
        public virtual int status_log_id { get; set; }
        //public virtual int installation_id { get; set; }
        //public virtual int application_id { get; set; }
        public virtual Nullable<DateTime> status_time { get; set; }

        public virtual Installations installation { get; set; }
        public virtual Applications application { get; set; }
    }
}
