using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUFStatus.Domain.Cloud.Logs
{
    public class ErrorLogs
    {
        public virtual int error_log_id { get; set; }
        //public virtual int installation_id { get; set; }
        //public virtual int application_id { get; set; }
        public virtual int action_type_id { get; set; }
        public virtual Nullable<DateTime> error_time { get; set; }
        public virtual string error_message { get; set; }
        public virtual string error_info { get; set; }
        public virtual string gear_code { get; set; }

        public virtual App.Installations installation { get; set; }
        public virtual App.Applications application { get; set; }
    }
}
