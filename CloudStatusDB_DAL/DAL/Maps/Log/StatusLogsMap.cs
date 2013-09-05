using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Cloud.Logs;
using TUFStatus.Domain.Cloud.App;

namespace TUFStatus.DAL.Cloud.Maps.App
{
    public class StatusLogsMap : ClassMap<StatusLogs>
    {
        public StatusLogsMap()
        {
            Schema("logs");
            Table("status_logs");

            Id(x => x.status_log_id).GeneratedBy.Sequence("logs.status_log_id_seq");
            //Map(x => x.installation_id);
            //Map(x => x.application_id);
            Map(x => x.status_time);
 
            References(x => x.installation).Column("installation_id").LazyLoad();
            References(x => x.application).Column("application_id").LazyLoad();
        }
    }
}
