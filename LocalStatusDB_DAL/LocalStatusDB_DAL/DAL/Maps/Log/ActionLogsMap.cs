using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Local.Logs;

namespace TUFStatus.DAL.Local.Maps.Log
{
    class ActionLogsMap : ClassMap<ActionLogs>
    {
        public ActionLogsMap()
        {
            Table("action_logs");

            Id(x => x.local_action_log_id).GeneratedBy.Native();
            //Map(x => x.installation_id);
            Map(x => x.application_id);
            Map(x => x.action_type_id);
            Map(x => x.action_time);
            Map(x => x.action_result);
            Map(x => x.action_messages);
            Map(x => x.had_error);
            Map(x => x.gear_code);
            Map(x => x.is_transferred);

            References(x => x.installation).Column("installation_id");
        }

    }
}
