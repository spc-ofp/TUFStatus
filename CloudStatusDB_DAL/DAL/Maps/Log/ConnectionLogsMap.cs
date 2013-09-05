using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Cloud.Logs;

namespace TUFStatus.DAL.Cloud.Maps.App
{
    public class ConnectionLogsMap : ClassMap<ConnectionLogs>
    {
        public ConnectionLogsMap()
        {
            Schema("logs");
            Table("connection_logs");

            Id(x => x.connection_id).GeneratedBy.Sequence("logs.connection_id_seq");
            //Map(x => x.installation_id);
            //Map(x => x.application_id);
            Map(x => x.connection_time);
            Map(x => x.disconnection_time);

            References(x => x.installation).Column("installation_id");
            References(x => x.application).Column("application_id");
        }
    }
}
