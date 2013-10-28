using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using TUFMAN.Domain.Ves;

namespace SyncDB.DAL.Maps.Ves{
    
    
    public class VesselsMap : ClassMap<Vessels> {
        
        public VesselsMap() {
            Schema("ves");
			Table("vessels");
			Id(x => x.vessel_id).GeneratedBy.Assigned().Column("vessel_id");
            //Map(x => x.gear_code).Column("gear_code").Length(1);
			References(x => x.gears).Column("gear_code");
            //Map(x => x.flag_code).Column("flag_code").Length(2);
			References(x => x.flag_country).Column("flag_code");
            //Map(x => x.flag_conv_code).Column("flag_conv_code").Length(2);
    		References(x => x.flag_conv_country).Column("flag_conv_code");
			Map(x => x.first_date).Column("first_date");
			Map(x => x.last_date).Column("last_date");
			Map(x => x.vessel_name).Column("vessel_name").Not.Nullable().Length(30);
			Map(x => x.regist_no).Column("regist_no").Length(20);
			Map(x => x.grt).Column("grt").Precision(8).Scale(2);
			Map(x => x.ircs).Column("ircs").Length(10);
			Map(x => x.alc_id).Column("alc_id").Precision(10);
			Map(x => x.ffa_vid).Column("ffa_vid").Precision(6).Scale(0);
			Map(x => x.owner_id).Column("owner_id").Precision(10);
			Map(x => x.agent_id).Column("agent_id").Precision(10);
			Map(x => x.captain_id).Column("captain_id").Precision(10);
			Map(x => x.fishmaster_id).Column("fishmaster_id").Precision(10);
			Map(x => x.inactive).Column("inactive").Not.Nullable();
			Map(x => x.win).Column("win").Length(10);
			Map(x => x.comments).Column("comments").Length(255);
			Map(x => x.vregport).Column("vregport").Length(50);
			Map(x => x.vownname).Column("vownname").Length(250);
			Map(x => x.vownaddress).Column("vownaddress").Length(250);
			Map(x => x.vmastname).Column("vmastname").Length(250);
			Map(x => x.vmastnationality).Column("vmastnationality").Length(250);
			Map(x => x.vbuiltwhere).Column("vbuiltwhere").Length(50);
			Map(x => x.vbuiltwhen).Column("vbuiltwhen").Length(50);
			Map(x => x.vprevname).Column("vprevname").Length(50);
			Map(x => x.vprevircs).Column("vprevircs").Length(50);
			Map(x => x.vprevflag).Column("vprevflag").Length(50);
			Map(x => x.vprevregno).Column("vprevregno").Length(50);
			Map(x => x.vinmarsatchannels).Column("vinmarsatchannels").Length(250);
			Map(x => x.vinmarsatnumbers).Column("vinmarsatnumbers").Length(50);
			Map(x => x.vinmarsatano).Column("vinmarsatano").Length(50);
			Map(x => x.vinmarsatbno).Column("vinmarsatbno").Length(50);
			Map(x => x.vinmarsatcno).Column("vinmarsatcno").Length(50);
			Map(x => x.vinmarsatfno).Column("vinmarsatfno").Length(50);
			Map(x => x.vinmarsatmno).Column("vinmarsatmno").Length(50);
			Map(x => x.vcrew).Column("vcrew").Precision(5);
			Map(x => x.vlength).Column("vlength").Precision(10).Scale(2);
			Map(x => x.vlengthtype).Column("vlengthtype").Length(1);
			Map(x => x.vlengthunits).Column("vlengthunits").Length(1);
			Map(x => x.vmdepth).Column("vmdepth").Precision(10).Scale(2);
			Map(x => x.vbeam).Column("vbeam").Precision(10).Scale(2);
			Map(x => x.vpower).Column("vpower").Precision(10).Scale(1);
			Map(x => x.vpowerunits).Column("vpowerunits").Length(2);
			Map(x => x.vfreezertypes).Column("vfreezertypes").Length(50);
			Map(x => x.vstoremethod1).Column("vstoremethod1").Not.Nullable();
			Map(x => x.vstoremethod2).Column("vstoremethod2").Not.Nullable();
			Map(x => x.vstoremethod3).Column("vstoremethod3").Not.Nullable();
			Map(x => x.vstoremethod4).Column("vstoremethod4").Not.Nullable();
			Map(x => x.vfreezerno).Column("vfreezerno").Precision(5);
			Map(x => x.vfreezercapacity).Column("vfreezercapacity").Precision(10).Scale(2);
			Map(x => x.vfreezcapunits).Column("vfreezcapunits").Length(2);
			Map(x => x.vfholdcapacity).Column("vfholdcapacity").Precision(10).Scale(2);
			Map(x => x.vcapacityunits).Column("vcapacityunits").Length(2);
			Map(x => x.vauthtype).Column("vauthtype").Length(50);
			Map(x => x.vauthno).Column("vauthno").Length(50);
			Map(x => x.vauthareas).Column("vauthareas").Length(50);
			Map(x => x.vauthspecies).Column("vauthspecies").Length(50);
			Map(x => x.vauthperiod).Column("vauthperiod").Length(50);
			Map(x => x.refrig_seawater).Column("refrig_seawater").Not.Nullable();
			Map(x => x.refrig_aircoils).Column("refrig_aircoils").Not.Nullable();
			Map(x => x.refrig_airblast).Column("refrig_airblast").Not.Nullable();
			Map(x => x.refrig_brinenacl).Column("refrig_brinenacl").Not.Nullable();
			Map(x => x.refrig_ice).Column("refrig_ice").Not.Nullable();
			Map(x => x.refrig_brinecacl).Column("refrig_brinecacl").Not.Nullable();
			Map(x => x.refrig_other).Column("refrig_other").Length(50);
			Map(x => x.boat_id).Column("boat_id").Precision(10);
			Map(x => x.vessel_category_code).Column("vessel_category_code").Length(2);
			Map(x => x.standard_vessel_name).Column("standard_vessel_name").Length(50);
			Map(x => x.builder_name).Column("builder_name").Length(50);
			Map(x => x.net_tonnage).Column("net_tonnage").Precision(8).Scale(2);
			Map(x => x.construct_material).Column("construct_material").Length(50);
			Map(x => x.inmarsat_imn).Column("inmarsat_imn").Length(50);
			Map(x => x.inmarsat_maker).Column("inmarsat_maker").Length(50);
			Map(x => x.inmarsat_model).Column("inmarsat_model").Length(50);
			Map(x => x.inmarsat_serial_no).Column("inmarsat_serial_no").Length(50);
			Map(x => x.inmarsat_security_no).Column("inmarsat_security_no").Length(50);
            Map(x => x.entered_date).Column("entered_date");
            Map(x => x.changed_date).Column("changed_date");
        }
    }
}
