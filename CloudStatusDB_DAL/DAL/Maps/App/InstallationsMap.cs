using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TUFStatus.Domain.Cloud.App;

namespace TUFStatus.DAL.Cloud.Maps.App
{
    public class InstallationsMap : ClassMap<Installations>
    {
        public InstallationsMap()
        {
            Schema("app");
            Table("installations");

            Id(x => x.installation_id);
            Map(x => x.country_code);
            Map(x => x.install_no);
            Map(x => x.description);
            Map(x => x.tufman_driver);
            Map(x => x.tufman_server);
            Map(x => x.tufman_database);
            Map(x => x.tufman_userlogin);
            Map(x => x.tufman_username);
            Map(x => x.tufman_password);
            Map(x => x.run_backup);
            Map(x => x.backup_folder);
            Map(x => x.backup_copy_folder);
            Map(x => x.portal_driver);
            Map(x => x.portal_server);
            Map(x => x.portal_database);
            Map(x => x.portal_userlogin);
            Map(x => x.portal_username);
            Map(x => x.portal_password);
            Map(x => x.run_sync);
        }
    }
}