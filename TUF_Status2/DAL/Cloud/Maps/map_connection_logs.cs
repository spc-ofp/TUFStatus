using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUF_Status.Domain.Cloud.Logs;

namespace TUF_Status.DAL.Cloud.Maps
{
    public class map_cloud_connection_logs : ClassMap<connection_logs>
    {
        public map_cloud_connection_logs()
        {
            Schema("logs");
            Table("connection_logs");

            Id(x => x.connection_id).GeneratedBy.Sequence("logs.connection_id_seq");
            //Id(x => x.connection_id).GeneratedBy.Identity();
            Map(x => x.installation_id);
            Map(x => x.application_id);
            Map(x => x.connection_time);
            Map(x => x.disconnection_time);
        }
    }
}
