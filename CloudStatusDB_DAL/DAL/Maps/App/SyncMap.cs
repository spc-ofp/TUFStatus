using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Cloud.App;

namespace TUFStatus.DAL.Cloud.Maps.App
{
    public class SyncMap : ClassMap<Sync>
    {
        public SyncMap()
        {
            Schema("app");
            Table("sync");

            Id(x => x.sync_id);
            Map(x => x.description);
            Map(x => x.schemaname);
            Map(x => x.tablename);
            Map(x => x.direction_code);
            Map(x => x.sync_type_code);
            Map(x => x.sync_date);
            Map(x => x.sync_order);
            Map(x => x.table_id);
            Map(x => x.last_run_date);
            Map(x => x.last_run_result);

            References(x => x.installation).Column("installation_id");
            References(x => x.application).Column("application_id");
        }
    }
}