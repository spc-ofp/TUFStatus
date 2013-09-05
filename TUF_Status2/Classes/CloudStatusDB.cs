using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUFStatus.DAL;
using NHibernate;

namespace TUFStatus
{
    public class CloudStatusDB : IStatusDB
    {
        public ISession session;
        //public OdbcConnection connection;

        private int connectionID;

        //public CloudStatusDB(string sqlDriver, string servername)
        //{
        //    string connectionString;

        //    switch (sqlDriver.ToUpper())
        //    {
        //        case "POSTGRESQL35W" :
        //            connectionString = "Driver={PostgreSQL UNICODE};Server=" + servername + ";Database=TUFStatus;UID=ofp_admin;pwd=ofp_admin;";
        //            break;
        //        default:
        //            connectionString = "driver=" + sqlDriver + ";Server=" + servername + ";Database=TUFStatus;User Id=tufman_server;Password=tuf$MAN1x99;";
        //            break;
        //    }

        //    connection = new OdbcConnection(connectionString);
        //    connectionID = 0;
        //    connection.ConnectionTimeout = 60;
        //}

        public CloudStatusDB(ISession iSession)
        {
            session = iSession;

            connectionID = 0;
            //session.Connection.ConnectionTimeout = 60;
        }

        public bool IsCloud
        {
            get { return true; }
        }

        public bool IsLocal
        {
            get { return false; }
        }

        public bool IsConnected
        {
            get { return (session.Connection != null); }
        }

        public System.Data.ConnectionState ConnectionState
        {
            get { return session.Connection.State; }
        }

        public bool OpenConnection()
        {
            try
            {
                if (!session.IsOpen)
                    session.Connection.Open();
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error opening the cloud DB:", ex.Message);
                return false;
            }

            if (session.IsOpen)
                return true;
            else
                return false;
        }

        public bool CloseConnection()
        {
            if (session.IsOpen)
            {
                try
                {
                    session.Close();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error closing the cloud DB connection:", ex.Message);
                    return false;
                }
            }

            if (!session.IsOpen)
                return true;
            else
                return false;
        }

        public bool WriteActionLog(ActionLog ActionLog)
        {
            throw new NotImplementedException();
        }

        public bool WriteStatusLog(StatusLog ActionLog)
        {
            throw new NotImplementedException();
        }

        public bool WriteErrorLog(ErrorLog ErrorLog)
        {
            throw new NotImplementedException();
        }

        public int WriteConnectionLog()
        {
            try
            {
                //using (ISession session = NHibernateHelper.OpenLocalSession())
                Domain.Cloud.Logs.ConnectionLogs connection_log = new Domain.Cloud.Logs.ConnectionLogs 
                {
                    application = session.Get<TUFStatus.Domain.Cloud.App.Applications>(3),
                    installation = session.Get<TUFStatus.Domain.Cloud.App.Installations>(Program.TufmanInstallationID),
                    connection_time = DateTime.Now 
                };

                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Cloud.Logs.ConnectionLogs>(session);


                using (var xa = session.BeginTransaction())
                {
                    repository.Add(connection_log);
                    xa.Commit();
                    //Assert.NotNull(fad);
                    //Assert.AreNotEqual(0, fad.Id);
                }

            
                if (Program.RunMode == 0)
                {
                    int x = connection_log.connection_id;
                    System.Windows.Forms.MessageBox.Show(x.ToString() + ":" + connection_log.installation.installation_id.ToString());    //.description.ToString());
                }

                connectionID = connection_log.connection_id;
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error writing the connection log:", ex.Message);
                return 0;
            }

            return connectionID;
        }

        public bool WriteDisconnectionTime()
        {
            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Cloud.Logs.ConnectionLogs>(session);
                //Domain.Cloud.Logs.Connection_Logs connection_log = session.Get<Domain.Cloud.Logs.Connection_Logs>(connectionID);

                using (var xa = session.BeginTransaction())
                {
                    Domain.Cloud.Logs.ConnectionLogs connection_log = repository.FindById(connectionID);
                    connection_log.disconnection_time = DateTime.Now;
                    repository.Update(connection_log);
                    xa.Commit();
                    //Assert.NotNull(fad);
                    //Assert.AreNotEqual(0, fad.Id);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error writing the disconnection time:", ex.Message);
                return false;
            }
            return true;
        }

        public object GetInstallationDetails()
        {
            Domain.Cloud.App.Installations installation;

            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Cloud.App.Installations>(session);

                installation = repository.FindById(Program.TufmanInstallationID);
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error retrieving the installation details from the cloud:", ex.Message);
                return null;
            }
            return installation;
        }

        public int TransferActionLogs(IStatusDB DestDB)
        {
            throw new NotImplementedException();
        }

        public int AppendActionLogs(List<TUFStatus.Domain.Local.Logs.ActionLogs> actionLogs, ISession localSession)
        {
            int transferCount = 0;

            if (actionLogs.Count > 0)
            {
                try
                {
                    var localRepository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ActionLogs>(localSession);
                    var cloudRepository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Cloud.Logs.ActionLogs>(session);

                    for (int i = 0; i < actionLogs.Count; i++)
                    {
                        TUFStatus.Domain.Cloud.Logs.ActionLogs newActionLog = new TUFStatus.Domain.Cloud.Logs.ActionLogs();

                        newActionLog.installation = session.Get<TUFStatus.Domain.Cloud.App.Installations>(actionLogs[i].installation.installation_id);
                        newActionLog.application = session.Get<TUFStatus.Domain.Cloud.App.Applications>(actionLogs[i].application_id);
                        newActionLog.action_type_id = actionLogs[i].action_type_id;
                        newActionLog.gear_code = actionLogs[i].gear_code;
                        newActionLog.action_time = actionLogs[i].action_time;
                        newActionLog.action_messages = actionLogs[i].action_messages;
                        newActionLog.action_result = actionLogs[i].action_result;
                        newActionLog.had_error = actionLogs[i].had_error;

                        using (var xa = session.BeginTransaction())
                        {
                            cloudRepository.Add(newActionLog);
                            xa.Commit();
                            //Assert.NotNull(fad);
                            //Assert.AreNotEqual(0, fad.Id);
                        }

                        using (var xb = localSession.BeginTransaction())
                        {
                            actionLogs[i].is_transferred = 1;
                            localRepository.Update(actionLogs[i]);
                            xb.Commit();
                        }
                        transferCount += 1;
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.LogTransfer, "", "There was an error appending the action logs:", ex.Message);
                    return 0;
                }
            }
            return transferCount;
        }

        public int TransferErrorLogs(IStatusDB DestDB)
        {
            throw new NotImplementedException();
        }

        public int AppendErrorLogs(List<TUFStatus.Domain.Local.Logs.ErrorLogs> errorLogs, ISession localSession)
        {
            int transferCount = 0;

            if (errorLogs.Count > 0)
            {
                try
                {
                    var localRepository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ErrorLogs>(localSession);
                    var cloudRepository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Cloud.Logs.ErrorLogs>(session);

                    for (int i = 0; i < errorLogs.Count; i++)
                    {
                        TUFStatus.Domain.Cloud.Logs.ErrorLogs newErrorLog = new TUFStatus.Domain.Cloud.Logs.ErrorLogs();

                        newErrorLog.installation = session.Get<TUFStatus.Domain.Cloud.App.Installations>(errorLogs[i].installation.installation_id);
                        newErrorLog.application = session.Get<TUFStatus.Domain.Cloud.App.Applications>(errorLogs[i].application_id);
                        newErrorLog.action_type_id = errorLogs[i].action_type_id;
                        newErrorLog.gear_code = errorLogs[i].gear_code;
                        newErrorLog.error_time = errorLogs[i].error_time;
                        newErrorLog.error_message = errorLogs[i].error_message;
                        newErrorLog.error_info = errorLogs[i].error_info;

                        using (var xa = session.BeginTransaction())
                        {
                            cloudRepository.Add(newErrorLog);
                            xa.Commit();
                            //Assert.NotNull(fad);
                            //Assert.AreNotEqual(0, fad.Id);
                        }

                        using (var xb = localSession.BeginTransaction())
                        {
                            errorLogs[i].is_transferred = 1;
                            localRepository.Update(errorLogs[i]);
                            xb.Commit();
                        }
                        transferCount += 1;
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.LogTransfer, "", "There was an error appending the error logs:", ex.Message);
                    return 0;
                }
            }
            return transferCount;
        }

        public bool SaveInstallationDetails(TUFStatus.Domain.Cloud.App.Installations installation)
        {
            throw new NotImplementedException();
        }

        public int ClearErrorLogs()
        {
            throw new NotImplementedException();
        }

        public int ClearActionLogs()
        {
            throw new NotImplementedException();
        }


        public List<Domain.Local.Logs.ActionLogs> GetLocalActionLogs()
        {
            throw new NotImplementedException();
        }


        public List<Domain.Local.Logs.ErrorLogs> GetLocalErrorLogs()
        {
            throw new NotImplementedException();
        }
    }
}
