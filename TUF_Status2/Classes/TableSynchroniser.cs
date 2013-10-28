using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using TUFMAN.Domain.Audit;

namespace TUFStatus.Classes
{
    public class TableSynchroniser<T>
        where T : class
    {
        //public int SynchroniseTable(ISession sourceSession, ISession destinationSession,Synchroniser.SyncDirection syncDirection, Synchroniser.SyncType syncType)   //, List<T> sourceList
        //{
        //    int result = 0;
        //    int batchSize = 50;
        //    int numBatches = 0;

        //    // get the source list -- need to add filtering though
        //    TUFStatus.DAL.Repositories.Repository<ISession, T> sourceRepo = new TUFStatus.DAL.Repositories.Repository<ISession, T>(sourceSession);
        //    List<T> sourceList = sourceRepo.All().ToList();

        //    // destination repo
        //    //TUFStatus.DAL.Repositories.Repository<ISession, T> destRepo = new TUFStatus.DAL.Repositories.Repository<ISession, T>(destinationSession);

        //    // setbatchsize does not work for postgres, batching not supported for that
        //    //destinationSession.SetBatchSize(100);

        //    // split into batch-size chunks

        //    numBatches = sourceList.Count() / batchSize;
        //    if ((sourceList.Count() % batchSize) > 0)
        //        numBatches += 1;

        //    for (int b = 0; b < numBatches; b++)
        //    {
        //        var xa = destinationSession.BeginTransaction();
        //        for (int i = 0; i < batchSize; i++)
        //        {
        //            int item = (b * batchSize) + i;

        //            if (item < sourceList.Count())
        //            {
        //                destinationSession.Merge(sourceList[(b * batchSize) + i]);
        //                result += 1;
        //            }
        //        }
        //        xa.Commit();
        //    }

        //    //var xa = destinationSession.BeginTransaction();
        //    //foreach (T sourceRecord in sourceList)
        //    //{
        //    //    //destinationSession.SaveOrUpdate(sourceRecord);
        //    //    destinationSession.Merge(sourceRecord);
        //    //    //destRepo.Save(sourceRecord);
        //    //}
        //    //xa.Commit();
            
        //    sourceRepo = null;

        //    return result;
        //}

        public int SynchroniseTable(ISession sourceSession, ISession destinationSession,IStatelessSession destinationStatelessSession, Func<T, bool> predicate, Synchroniser.SyncDirection syncDirection, Synchroniser.SyncType syncType,bool forcePredicate)   //, List<T> sourceList
        {
            int result = 0;
            int batchSize = 50;
            int numBatches = 0;
            List<T> sourceList;

            try
            {
                // get the source list -- don't use filtering if in replace mode
                TUFStatus.DAL.Repositories.Repository<ISession, T> sourceRepo = new TUFStatus.DAL.Repositories.Repository<ISession, T>(sourceSession);

                if (syncType == Synchroniser.SyncType.Replace)
                {
                    // delete all records first
                    var xa = destinationSession.BeginTransaction();

                    destinationSession.Delete("from " + typeof(T));

                    xa.Commit();

                    if (forcePredicate)
                        sourceList = sourceRepo.All().Where(predicate).ToList();
                    else
                        sourceList = sourceRepo.All().ToList();

                    numBatches = sourceList.Count() / batchSize;
                    if ((sourceList.Count() % batchSize) > 0)
                        numBatches += 1;

                    for (int b = 0; b < numBatches; b++)
                    {
                        var xb = destinationStatelessSession.BeginTransaction();
                        for (int i = 0; i < batchSize; i++)
                        {
                            int item = (b * batchSize) + i;

                            if (item < sourceList.Count())
                            {
                                destinationStatelessSession.Insert(sourceList[(b * batchSize) + i]);  
                                result += 1;
                            }
                        }
                        xb.Commit();
                    }
                }
                else // merge
                {
                    sourceList = sourceRepo.All().Where(predicate).ToList();

                    // split into batch-size chunks

                    numBatches = sourceList.Count() / batchSize;
                    if ((sourceList.Count() % batchSize) > 0)
                        numBatches += 1;

                    for (int b = 0; b < numBatches; b++)
                    {
                        var xa = destinationSession.BeginTransaction();
                        for (int i = 0; i < batchSize; i++)
                        {
                            int item = (b * batchSize) + i;

                            if (item < sourceList.Count())
                            {
                                destinationSession.Merge(sourceList[(b * batchSize) + i]); //Merge 
                                result += 1;
                            }
                        }
                        xa.Commit();
                    }
                    sourceRepo = null;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error syncronising " + typeof(T).ToString() + ":", ex.Message);
                result = -1;
            }
            return result;
        }

        public int SynchroniseTableInserts(ISession sourceSession, IStatelessSession destinationSession, Func<T, bool> predicate, Synchroniser.SyncDirection syncDirection, Synchroniser.SyncType syncType)   //, List<T> sourceList
        {
            int result = 0;
            int batchSize = 50;
            int numBatches = 0;
            List<T> sourceList;

            try
            {
                // get the source list -- don't use filtering if in replace mode
                TUFStatus.DAL.Repositories.Repository<ISession, T> sourceRepo = new TUFStatus.DAL.Repositories.Repository<ISession, T>(sourceSession);

                if (syncType == Synchroniser.SyncType.Replace)
                {
                    // delete all records first
                    var xa = destinationSession.BeginTransaction();

                    ///var metaData = destinationSession.SessionFactory.GetClassMetadata(typeof(T)) as NHibernate.Persister.Entity.AbstractEntityPersister;
                    ///string table = metaData.TableName;
                    ///string deleteAll = string.Format("DELETE FROM \"{0}\"", table);
                    ///destinationSession.CreateSQLQuery(deleteAll).ExecuteUpdate();
                    //destinationSession.Delete("Select * from " + typeof(T));

                    destinationSession.Delete("from " + typeof(T));


                    //TUFStatus.DAL.Repositories.Repository<ISession, T> destRepo = new TUFStatus.DAL.Repositories.Repository<ISession, T>(destinationSession);

                    //List<T> deleteList = sourceRepo.All().ToList() ;
                    //foreach (T deleteItem in deleteList)
                    //{
                    //    destRepo.Delete(deleteItem);
                    //}

                    xa.Commit();

                    sourceList = sourceRepo.All().ToList();
                }
                else
                    sourceList = sourceRepo.All().Where(predicate).ToList();

                // split into batch-size chunks

                numBatches = sourceList.Count() / batchSize;
                if ((sourceList.Count() % batchSize) > 0)
                    numBatches += 1;

                for (int b = 0; b < numBatches; b++)
                {
                    var xa = destinationSession.BeginTransaction();
                    for (int i = 0; i < batchSize; i++)
                    {
                        int item = (b * batchSize) + i;

                        if (item < sourceList.Count())
                        {
                            destinationSession.Insert(sourceList[(b * batchSize) + i]); //Merge 
                            result += 1;
                        }
                    }
                    xa.Commit();

                    sourceRepo = null;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error syncronising inserts for " + typeof(T).ToString() + ":", ex.Message);
                result = -1;
            }
            return result;
        }

        public int SynchroniseTableDeletes(ISession sourceSession, ISession destinationSession, Func<T, bool> predicate, DateTime? syncDate, Int16? tableID)   //, List<T> sourceList
        {
            int result = 0;
            //int batchSize = 50;
            //int numBatches = 0;
            List<DeletedLog> deleteList;
            string keyClause="";
            TUFStatus.DAL.Repositories.Repository<ISession, DeletedLog> sourceRepo = null;

            try
            {
                // get the source delete list
                sourceRepo = new TUFStatus.DAL.Repositories.Repository<ISession, DeletedLog>(sourceSession);

                if (syncDate == null)
                    deleteList = sourceRepo.All().Where(x => x.audited_table.table_id == tableID).ToList();
                else
                    deleteList = sourceRepo.All().Where(x => x.audited_table.table_id == tableID).Where(x => x.deleted_date > syncDate).ToList();

                result = deleteList.Count;

                // build the key clause
                // **********************************************
                // may need to break this into chunks of ~50 records, in case the list is too long

                if (deleteList.Count > 0)
                {
                    for (int i = 0; i < deleteList.Count; i++)
                    {
                        string keyVal;

                        keyVal = deleteList[i].keyvalue;
                        if (deleteList[i].audited_table.key_field_type == "string")
                            keyVal = "'" + keyVal + "'";

                        if (i == 0)
                            keyClause = keyVal;
                        else
                            keyClause = keyClause + "," + keyVal;
                    }

                    var xa = destinationSession.BeginTransaction();

                    destinationSession.Delete("from " + typeof(T) + " where " + deleteList[0].audited_table.key_field + " in (" + keyClause + ")");  // where + key_field in (list)


                    xa.Commit();


                    // split into batch-size chunks

                    //numBatches = sourceList.Count() / batchSize;
                    //if ((sourceList.Count() % batchSize) > 0)
                    //    numBatches += 1;

                    //for (int b = 0; b < numBatches; b++)
                    //{
                    //    var xa = destinationSession.BeginTransaction();
                    //    for (int i = 0; i < batchSize; i++)
                    //    {
                    //        int item = (b * batchSize) + i;

                    //        if (item < sourceList.Count())
                    //        {
                    //            destinationSession.Merge(sourceList[(b * batchSize) + i]);
                    //            result += 1;
                    //        }
                    //    }
                    //    xa.Commit();


                    //}
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error syncronising table deletes for " + typeof(T).ToString() + ":", ex.Message);
                result = -1;
            }
            finally
            {
                sourceRepo = null;
            }
            return result;
        }
    }
}
