using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUFStatus.DAL;
using System.Data.SQLite;
using NHibernate;

namespace TUFStatus
{
    public class LocalStatusDB : IStatusDB
    {
        public ISession session;
        //private SQLiteConnection connection;

        //public LocalStatusDB(string filepath)
        //{
        //    connection = new SQLiteConnection("Data Source=" + filepath + ";Version=3;");
        //}

        public LocalStatusDB(ISession isession)
        {
            session = isession;
        }

        public bool IsCloud
        {
            get { return false; }
        }

        public bool IsLocal
        {
            get { return true; }
        }

        public bool IsConnected
        {
            get { return session.IsConnected; }
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
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error opening the local DB", ex.Message);
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
                    ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error closing the local DB connection:", ex.Message);
                    return false;
                }
            }

            if (!session.IsOpen)
                return true;
            else
                return false;
        }

        public bool WriteActionLog(ActionLog actionLog)
        {
            try
            {
                short hadError;

                if (actionLog.HadError) hadError = 1 ;
                else hadError = 0;

                Domain.Local.Logs.ActionLogs newActionLog = new Domain.Local.Logs.ActionLogs
                {
                    installation = session.Get <TUFStatus.Domain.Local.App.Installations> (actionLog.InstallationID),
                    application_id = (int)actionLog.ApplicationID,
                    action_type_id = (int)actionLog.ActionTypeID,
                    gear_code = actionLog.GearCode,
                    action_time = actionLog.ActionTime,
                    action_result = actionLog.ActionResult,
                    action_messages = actionLog.ActionMessage,
                    had_error = hadError,
                    is_transferred = 0
                };

                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ActionLogs>(session);


                using (var xa = session.BeginTransaction())
                {
                    repository.Add(newActionLog);
                    xa.Commit();
                    //Assert.NotNull(fad);
                    //Assert.AreNotEqual(0, fad.Id);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error writing the local action log:", ex.Message);
                return false;
            }

            return true;
        }

        public bool WriteStatusLog(StatusLog ActionLog)
        {
            throw new NotImplementedException();
        }

        public bool WriteErrorLog(ErrorLog errorLog)
        {
            try
            {
                Domain.Local.Logs.ErrorLogs newErrorLog = new Domain.Local.Logs.ErrorLogs
                {
                    installation = session.Get<TUFStatus.Domain.Local.App.Installations>(errorLog.InstallationID),
                    application_id = (int)errorLog.ApplicationID,
                    action_type_id = (int)errorLog.ActionTypeID,
                    gear_code = errorLog.GearCode,
                    error_time = errorLog.ErrorTime,
                    error_message = errorLog.ErrorMessage,
                    error_info = errorLog.ErrorInfo,
                    is_transferred = 0
                };

                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ErrorLogs>(session);


                using (var xa = session.BeginTransaction())
                {
                    repository.Add(newErrorLog);
                    xa.Commit();
                    //Assert.NotNull(fad);
                    //Assert.AreNotEqual(0, fad.Id);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error writing the local action log:", ex.Message);
                return false;
            }

            return true;
        }

        public int WriteConnectionLog()
        {
            return 0;
        }

        public bool WriteDisconnectionTime()
        {
            return false;
        }

        public object GetInstallationDetails()
        {
            Domain.Local.App.Installations installation;

            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.App.Installations>(session);

                installation = repository.FindById(Program.TufmanInstallationID);
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error retrieving the installation details from the local DB:", ex.Message);
                return null;
            }
            return installation;
        }

        public int TransferActionLogs(IStatusDB destDB)
        {
            // transfers action logs with 'is_transferred' of false to the destination (cloud) db
            int result = 0;
            List<TUFStatus.Domain.Local.Logs.ActionLogs> actionLogs;
            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ActionLogs>(session);
                actionLogs = repository.FilterBy(x => x.is_transferred == 0).ToList();

                result = destDB.AppendActionLogs(actionLogs,session);
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.LogTransfer, "", "There was an error transferring the local action logs to the cloud:", ex.Message);
                return -1;
            }

            return result;
        }

        public int AppendActionLogs(List<TUFStatus.Domain.Local.Logs.ActionLogs> actionLogs, ISession localSession)
        {
            return 0;
        }

        public int TransferErrorLogs(IStatusDB destDB)
        {
            // transfers error logs with 'is_transferred' of false to the destination (cloud) db
            int result = 0;
            List<TUFStatus.Domain.Local.Logs.ErrorLogs> errorLogs;
            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ErrorLogs>(session);
                errorLogs = repository.FilterBy(x => x.is_transferred == 0).ToList();

                result = destDB.AppendErrorLogs(errorLogs, session);
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.LogTransfer, "", "There was an error transferring the local action logs to the cloud:", ex.Message);
                return -1;
            }

            return result;
        }

        public int AppendErrorLogs(List<TUFStatus.Domain.Local.Logs.ErrorLogs> errorLogs, ISession localSession)
        {
            return 0;
        }

        public List<TUFStatus.Domain.Local.Logs.ActionLogs> GetLocalActionLogs()
        {
            List<TUFStatus.Domain.Local.Logs.ActionLogs> actionLogs;

            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ActionLogs>(session);
                actionLogs = repository.All().ToList();
                return actionLogs;
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error retrieving the local action logs:", ex.Message);
                return null;
            }
        }

        public List<Domain.Local.Logs.ErrorLogs> GetLocalErrorLogs()
        {
            List<TUFStatus.Domain.Local.Logs.ErrorLogs> errorLogs;

            try
            {
                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ErrorLogs>(session);
                errorLogs = repository.All().ToList();
                return errorLogs;
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error retrieving the local error logs:", ex.Message);
                return null;
            }
        }

        public int ClearActionLogs()
        {
            // clear the local log file of transferred data
            int count = 0;

            // error logs
            List<TUFStatus.Domain.Local.Logs.ActionLogs> actionLogs;
            var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ActionLogs>(session);
            actionLogs = repository.FilterBy(x => x.is_transferred == 1).ToList();

            var xa = session.BeginTransaction();
            for (int i = 0; i < actionLogs.Count; i++)
            {
                repository.Delete(actionLogs[i]);
                count += 1;
            }
            xa.Commit();

            return count;
        }

        public int ClearErrorLogs()
        {
            // clear the local log file of transferred data
            int count = 0;

            // error logs
            List<TUFStatus.Domain.Local.Logs.ErrorLogs> errorLogs;
            var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.Logs.ErrorLogs>(session);
            errorLogs = repository.FilterBy(x => x.is_transferred == 1).ToList();

            var xa = session.BeginTransaction();
            for (int i = 0; i < errorLogs.Count; i++)
            {
                repository.Delete(errorLogs[i]);
                count += 1;
            }
            xa.Commit();

            return count;
        }


        public bool SaveInstallationDetails(TUFStatus.Domain.Cloud.App.Installations installation)
        {
            try
            {
                // first delete existing ? no need because we use save or update

                Domain.Local.App.Installations newInstallation = new Domain.Local.App.Installations
                {
                    installation_id = installation.installation_id,
                    country_code = installation.country_code,
                    install_no = installation.install_no,
                    description = installation.description,
                    tufman_driver = installation.tufman_driver,
                    tufman_server = installation.tufman_server,
                    tufman_database = installation.tufman_database,
                    tufman_userlogin = installation.tufman_userlogin,
                    tufman_username = installation.tufman_username,
                    tufman_password = installation.tufman_password,
                    run_backup = installation.run_backup,
                    backup_folder = installation.backup_folder,
                    backup_copy_folder = installation.backup_copy_folder,
                    portal_driver = installation.portal_driver,
                    portal_server = installation.portal_server,
                    portal_database = installation.portal_database,
                    portal_userlogin = installation.portal_userlogin,
                    portal_username = installation.portal_username,
                    portal_password = installation.portal_password
                };

                var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Local.App.Installations>(session);


                using (var xa = session.BeginTransaction())
                {
                    repository.Add(newInstallation);
                    xa.Commit();
                    //Assert.NotNull(fad);
                    //Assert.AreNotEqual(0, fad.Id);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error writing the local installation copy:", ex.Message);
                return false;
            }

            return true;
        }
    }
}
