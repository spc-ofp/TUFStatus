using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Cloud.Logs;

namespace TUFStatus.DAL.Cloud.Maps.App
{
    class ErrorLogsMap : ClassMap<ErrorLogs>
    {
        public ErrorLogsMap()
        {
            Schema("logs");
            Table("error_logs");

            Id(x => x.error_log_id).GeneratedBy.Sequence("logs.error_log_id_seq");
            //Map(x => x.installation_id);
            //Map(x => x.application_id);
            Map(x => x.action_type_id);
            Map(x => x.error_time);
            Map(x => x.error_message);
            Map(x => x.error_info);
            Map(x => x.gear_code);

            References(x => x.installation).Column("installation_id");
            References(x => x.application).Column("application_id");
        }
    }
}
