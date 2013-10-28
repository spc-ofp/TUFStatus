using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using TUFMAN.Domain.Ref;

namespace SyncDB.DAL.Maps.Ref
{
    
    
    public class CountriesMap : ClassMap<Countries> {
        
        public CountriesMap() {
            Schema("ref");
			Table("countries");
			Id(x => x.country_code).GeneratedBy.Assigned().Column("country_code");
			Map(x => x.country_name).Column("country_name").Not.Nullable().Length(50);
			Map(x => x.country_short).Column("country_short").Length(20);
			Map(x => x.spc_member).Column("spc_member").Not.Nullable();
			Map(x => x.ffa_member).Column("ffa_member").Not.Nullable();
			Map(x => x.pna_member).Column("pna_member").Not.Nullable();
			Map(x => x.iso3_code).Column("iso3_code").Length(3);
			Map(x => x.un_id).Column("un_id").Precision(5);
            Map(x => x.entered_date).Column("entered_date");
            Map(x => x.changed_date).Column("changed_date");
        }
    }
}
