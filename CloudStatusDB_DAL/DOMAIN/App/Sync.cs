using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUFStatus.Domain.Cloud.App
{
    public class Sync
    {
        public virtual int sync_id { get; set; }
        public virtual Installations installation { get; set; }
        public virtual string description { get; set; }
        public virtual Applications application { get; set; }
        public virtual string schemaname { get; set; }
        public virtual string tablename { get; set; }
        public virtual string direction_code { get; set; }
        public virtual string sync_type_code { get; set; }
        public virtual Int32? sync_order { get; set; }
        public virtual DateTime? sync_date { get; set; }
        public virtual Int16? table_id { get; set; }
        public virtual DateTime? last_run_date { get; set; }
        public virtual Int32 last_run_result { get; set; }

        public virtual int unmappedthing { get; set; }
    }
}
