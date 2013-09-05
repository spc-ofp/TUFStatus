using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Local.Logs;

namespace TUFStatus.DAL.Cloud.Maps.Log
{
    class ErrorLogsMap : ClassMap<ErrorLogs>
    {
        public ErrorLogsMap()
        {
            Table("error_logs");

            Id(x => x.local_error_log_id).GeneratedBy.Native();
            //Map(x => x.installation_id);
            Map(x => x.application_id);
            Map(x => x.action_type_id);
            Map(x => x.error_time);
            Map(x => x.error_message);
            Map(x => x.error_info);
            Map(x => x.gear_code);
            Map(x => x.is_transferred);

            References(x => x.installation).Column("installation_id");
        }
    }
}
