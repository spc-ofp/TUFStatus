using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUFStatus;
using TUFStatus.DAL;
using TUFStatus.DAL.Configuration;
using TUFMAN.Domain;
using TUFMAN.Domain.Ves;
using NHibernate;

// class to handle synchronisation for the portal
namespace TUFStatus.Classes
{
    public class Synchroniser
    {
        public int Synchronise(int installationID, CloudStatusDB cloudStatusDB, int mode)
        {
            // mode 0 = partial, 1 = full (i.e. mergeorReplace become replace)
            int result = 0;
            ISession IMSSession = null;
            IStatelessSession IMSStatelessSession = null;
            ISession tufmanSession = null;
            SyncDirection syncDirection;
            SyncType syncType;

            // retrieve a list of the synchronisation items
            List<TUFStatus.Domain.Cloud.App.Sync> syncList;
            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Cloud.App.Sync>(cloudStatusDB.session);
                //var xa = cloudStatusDB.session.BeginTransaction();
                syncList = repository.FilterBy(x => x.installation.installation_id == installationID).OrderBy(x => x.sync_order).ToList();
                
                //xa.Dispose();
                //xa.Rollback();
                // Cloud IMS session
                IMSSession = TUFStatus.DAL.Configuration.IMSNHibernateHelper.CreateSessionFactory().OpenSession();
                IMSStatelessSession = TUFStatus.DAL.Configuration.IMSNHibernateHelper.CreateSessionFactory().OpenStatelessSession();


                foreach (TUFStatus.Domain.Cloud.App.Sync syncItem in syncList)
                {
                    string tablename;
                    int tableResult = 0;
                    string countryCode;

                    //Type open = typeof(TableSynchroniser<>); 
                    ////Type closed = open.MakeGenericType(typeof(TUFMAN.Domain.Ves.VesselCategories));

                    //Type closed = open.MakeGenericType(Type.GetType(GetTableClass(tablename)));
                    //// Type closed2 = open.MakeGenericType(x.GetType());
                    //dynamic ts = Activator.CreateInstance(closed);

                    //TUFMAN.Domain.Ves.VesselActAnnual v;


                    // create the TUFMAN session

                    //UnitOfWork unitOfWorkTUFMAN = new UnitOfWork(TufmanNHibernateHelper.CreateSessionFactory("Server=NOUSQL50\\SQLEXPRESS;Database=tufman_ws;Trusted_Connection=True"));
                    tufmanSession = TufmanNHibernateHelper.CreateSessionFactory().OpenSession();

                    tablename = syncItem.schemaname + "." + syncItem.tablename;

                    countryCode = Program.tufmanInstallation.CountryCode();

                    // Check the direction of syncronisation to do
                    switch (syncItem.direction_code)
                    {
                        case "up": // merge or replace
                            syncDirection = SyncDirection.Up;
                            break;
                        case "dn": // replace
                            syncDirection = SyncDirection.Down;
                            break;
                        default: // anything else, only "up" is supported at present though
                            syncDirection = SyncDirection.Up;
                            break;
                    }

                    // Check the type of syncronisation to do
                    switch (syncItem.sync_type_code)
                    {
                        case "mr": // merge or replace
                            syncType = SyncType.Merge;
                            break;
                        case "rp": // replace
                            syncType = SyncType.Replace;
                            break;
                        case "dl": // delete
                            syncType = SyncType.Delete;
                            break;
                        default: // 'mg' - merge only
                            syncType = SyncType.MergeOrReplace;
                            break;
                    }

                    // set sync mode to replace if mode is 'full' and sync type is merge or replace
                    if (mode == 1)
                    {
                        if (syncType == SyncType.MergeOrReplace)
                            syncType = SyncType.Replace;
                    }

                    // checked the last sync date, if null then sync becomes 'replace'
                    if (syncItem.sync_date == null & syncType != SyncType.Delete)
                        syncType = SyncType.Replace;



                    // ********************* need to add filtering to this **************************** //
                    if (syncType == SyncType.Delete)
                    {
                        switch (tablename.ToLower())
                        {
                            case "lic.agr_rep_period":
                                TableSynchroniser<TUFMAN.Domain.Lic.AgrRepPeriod> ts11 = new TableSynchroniser<TUFMAN.Domain.Lic.AgrRepPeriod>();
                                tableResult = ts11.SynchroniseTableDeletes(tufmanSession, IMSSession, x => (x.changed_date >= syncItem.sync_date), syncItem.sync_date,syncItem.table_id);
                                break;
                            default:
                                // report an error here, table is not recognised and won't be synchronised
                                break;
                        }
                    }
                    else
                    {
                        switch (tablename.ToLower())
                        {
                            case "ves.vessel_categories":
                                TableSynchroniser<TUFMAN.Domain.Ves.VesselCategories> ts0 = new TableSynchroniser<TUFMAN.Domain.Ves.VesselCategories>();
                                tableResult = ts0.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType,false);
                                break;
                            case "ves.vessels":
                                TableSynchroniser<TUFMAN.Domain.Ves.Vessels> ts1 = new TableSynchroniser<TUFMAN.Domain.Ves.Vessels>();
                                //ts1.SynchroniseTable(tufmanSession, IMSSession);
                                //tableResult = ts1.SynchroniseTableInserts(tufmanSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType);

                                tableResult = ts1.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "ref.agreement_ownership":
                                TableSynchroniser<TUFMAN.Domain.Ref.AgreementOwnership> ts2 = new TableSynchroniser<TUFMAN.Domain.Ref.AgreementOwnership>();
                                tableResult = ts2.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "ref.agreement_types":
                                TableSynchroniser<TUFMAN.Domain.Ref.AgreementTypes> ts3 = new TableSynchroniser<TUFMAN.Domain.Ref.AgreementTypes>();
                                tableResult = ts3.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "ref.license_status":
                                TableSynchroniser<TUFMAN.Domain.Ref.LicenseStatus> ts4 = new TableSynchroniser<TUFMAN.Domain.Ref.LicenseStatus>();
                                tableResult = ts4.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "ref.license_types":
                                TableSynchroniser<TUFMAN.Domain.Ref.LicenseTypes> ts5 = new TableSynchroniser<TUFMAN.Domain.Ref.LicenseTypes>();
                                tableResult = ts5.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "ref.iaf_zones":
                                TableSynchroniser<TUFMAN.Domain.Ref.IafZones> ts6 = new TableSynchroniser<TUFMAN.Domain.Ref.IafZones>();
                                tableResult = ts6.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "tufman.license_profiles":
                                TableSynchroniser<TUFMAN.Domain.Tufman.LicenseProfiles> ts7 = new TableSynchroniser<TUFMAN.Domain.Tufman.LicenseProfiles>();
                                tableResult = ts7.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "tufman.payment_types":
                                TableSynchroniser<TUFMAN.Domain.Tufman.PaymentTypes> ts8 = new TableSynchroniser<TUFMAN.Domain.Tufman.PaymentTypes>();
                                tableResult = ts8.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "tufman.companies":
                                TableSynchroniser<TUFMAN.Domain.Tufman.Companies> ts9 = new TableSynchroniser<TUFMAN.Domain.Tufman.Companies>();
                                tableResult = ts9.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.agreements":
                                TableSynchroniser<TUFMAN.Domain.Lic.Agreements> ts10 = new TableSynchroniser<TUFMAN.Domain.Lic.Agreements>();
                                tableResult = ts10.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.agr_rep_period":
                                TableSynchroniser<TUFMAN.Domain.Lic.AgrRepPeriod> ts11 = new TableSynchroniser<TUFMAN.Domain.Lic.AgrRepPeriod>();
                                tableResult = ts11.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.agr_lic_profile":
                                TableSynchroniser<TUFMAN.Domain.Lic.AgrLicProfile> ts12 = new TableSynchroniser<TUFMAN.Domain.Lic.AgrLicProfile>();
                                tableResult = ts12.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.license_history":
                                TableSynchroniser<TUFMAN.Domain.Lic.LicenseHistory> ts13 = new TableSynchroniser<TUFMAN.Domain.Lic.LicenseHistory>();
                                tableResult = ts13.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.licenses":
                                TableSynchroniser<TUFMAN.Domain.Lic.Licenses> ts14 = new TableSynchroniser<TUFMAN.Domain.Lic.Licenses>();
                                tableResult = ts14.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.nat_fleet_iaf":
                                TableSynchroniser<TUFMAN.Domain.Lic.NatFleetIaf> ts15 = new TableSynchroniser<TUFMAN.Domain.Lic.NatFleetIaf>();
                                tableResult = ts15.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.nat_fleet_lic":
                                TableSynchroniser<TUFMAN.Domain.Lic.NatFleetLic> ts16 = new TableSynchroniser<TUFMAN.Domain.Lic.NatFleetLic>();
                                tableResult = ts16.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.nat_fleet_reg":
                                TableSynchroniser<TUFMAN.Domain.Lic.NatFleetReg> ts17 = new TableSynchroniser<TUFMAN.Domain.Lic.NatFleetReg>();
                                tableResult = ts17.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "lic.national_fleets":
                                TableSynchroniser<TUFMAN.Domain.Lic.NationalFleets> ts18 = new TableSynchroniser<TUFMAN.Domain.Lic.NationalFleets>();
                                tableResult = ts18.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            case "recon.view_log_yy_mm_eez_all_id":
                                TableSynchroniser<TUFMAN.Domain.Recon.ViewLogYyMmEezAllID> ts19 = new TableSynchroniser<TUFMAN.Domain.Recon.ViewLogYyMmEezAllID>();
                                tableResult = ts19.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.YY >= (DateTime.Now.Year - 4) & x.eez_code == countryCode), syncDirection, syncType, true);
                                break;
                            case "recon.view_unloads_yy_mm_all_id":
                                TableSynchroniser<TUFMAN.Domain.Recon.ViewUnloadsYyMmAllID> ts20 = new TableSynchroniser<TUFMAN.Domain.Recon.ViewUnloadsYyMmAllID>();
                                tableResult = ts20.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.YY >= (DateTime.Now.Year - 4)), syncDirection, syncType, true);
                                break;
                            case "recon.view_samples_yy_mm_all_id":
                                TableSynchroniser<TUFMAN.Domain.Recon.ViewSamplesYyMmAllID> ts21 = new TableSynchroniser<TUFMAN.Domain.Recon.ViewSamplesYyMmAllID>();
                                tableResult = ts21.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.YY >= (DateTime.Now.Year - 4)), syncDirection, syncType, true);
                                break;
                            case "unload.unloadings_ps":
                                TableSynchroniser<TUFMAN.Domain.Unload.UnloadingsPs> ts22 = new TableSynchroniser<TUFMAN.Domain.Unload.UnloadingsPs>();
                                tableResult = ts22.SynchroniseTable(tufmanSession, IMSSession, IMSStatelessSession, x => (x.changed_date >= syncItem.sync_date), syncDirection, syncType, false);
                                break;
                            default:
                                // report an error here, table is not recognised and won't be synchronised
                                break;
                        }
                    }
                    var xa = cloudStatusDB.session.BeginTransaction();
                    syncItem.last_run_date = DateTime.Now;
                    syncItem.last_run_result = tableResult;
                    if (tableResult >= 0)
                    {
                        result += tableResult;
                        syncItem.sync_date = DateTime.Now;
                    }
                    xa.Commit();

                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error running the synchroniser:", ex.Message);
                return -1;
            }
            finally
            {               
                if (IMSSession!=null && IMSSession.IsOpen)
                    IMSSession.Close();

                //IMSNHibernateHelper.SessionFactory.Close();

                if (tufmanSession != null && tufmanSession.IsOpen)
                    tufmanSession.Close();
            }

            return result;
        }

        // couple of enumerations

        public enum SyncDirection
        {
            Up=1,
            Down=2
        }

        public enum SyncType
        {
            Merge = 1,
            Replace = 2,
            MergeOrReplace = 3,
            Delete = 4
        }
    }
}
