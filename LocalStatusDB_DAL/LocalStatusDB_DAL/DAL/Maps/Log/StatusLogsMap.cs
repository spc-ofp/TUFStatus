using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Local.Logs;

namespace TUFStatus.DAL.Cloud.Maps.Log
{
    public class StatusLogsMap : ClassMap<StatusLogs>
    {
        public StatusLogsMap()
        {
            Table("status_logs");

            Id(x => x.local_status_log_id).GeneratedBy.Native();
            //Map(x => x.installation_id);
            Map(x => x.application_id);
            Map(x => x.status_time);
            Map(x => x.is_transferred);

            References(x => x.installation).Column("installation_id").LazyLoad();
        }
    }
}
