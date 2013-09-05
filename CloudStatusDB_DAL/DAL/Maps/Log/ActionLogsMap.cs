using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Cloud.Logs;

namespace TUFStatus.DAL.Cloud.Maps.App
{
    public class ActionLogsMap : ClassMap<ActionLogs>
    {
        public ActionLogsMap()
        {
            Schema("logs");
            Table("action_logs");

            Id(x => x.action_log_id).GeneratedBy.Sequence("logs.action_log_id_seq");
            //Map(x => x.installation_id);
            //Map(x => x.application_id);
            Map(x => x.action_type_id);
            Map(x => x.action_time);
            Map(x => x.action_result);
            Map(x => x.action_messages);
            Map(x => x.had_error);
            Map(x => x.gear_code);

            References(x => x.installation).Column("installation_id");
            References(x => x.application).Column("application_id");
        }
    }
}
