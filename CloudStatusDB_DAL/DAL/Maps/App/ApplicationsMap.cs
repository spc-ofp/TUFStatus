using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Cloud.App;

namespace TUFStatus.DAL.Cloud.Maps.App
{
    public class ApplicationsMap : ClassMap<Applications>
    {
        public ApplicationsMap()
        {
            Schema("app");
            Table("applications");

            Id(x => x.application_id);
            Map(x => x.application_desc);
        }
    }
}
