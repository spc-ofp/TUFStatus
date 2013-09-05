using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUFStatus.Domain.Local.App
{
    public class Installations
    {
        public virtual int installation_id { get; set; }
        public virtual string country_code { get; set; }
        public virtual int install_no { get; set; }
        public virtual string description { get; set; }
        public virtual string tufman_driver { get; set; }
        public virtual string tufman_server { get; set; }
        public virtual string tufman_database { get; set; }
        public virtual Int16 tufman_userlogin { get; set; }
        public virtual string tufman_username { get; set; }
        public virtual string tufman_password { get; set; }
        public virtual Int16 run_backup { get; set; }
        public virtual string backup_folder { get; set; }
        public virtual string backup_copy_folder { get; set; }
        public virtual string portal_driver { get; set; }
        public virtual string portal_server { get; set; }
        public virtual string portal_database { get; set; }
        public virtual Int16 portal_userlogin { get; set; }
        public virtual string portal_username { get; set; }
        public virtual string portal_password { get; set; }
        public virtual Int16 run_sync { get; set; }
    }
}
