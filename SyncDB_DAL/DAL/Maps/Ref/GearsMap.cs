using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using TUFMAN.Domain.Ref;

namespace SyncDB.DAL.Maps.Ref
{
    
    
    public class GearsMap : ClassMap<Gears> {
        
        public GearsMap() {
            Schema("ref");
			Table("gears");
			Id(x => x.gear_code).GeneratedBy.Assigned().Column("shortgear_code");
			Map(x => x.gear_code_2).Column("longgear_code").Length(2);
			Map(x => x.gear_desc).Column("gear_desc").Length(30);
			//Map(x => x.mimra_type_code).Column("mimra_type_code").Length(1);
            Map(x => x.entered_date).Column("entered_date");
            Map(x => x.changed_date).Column("changed_date");
        }
    }
}
