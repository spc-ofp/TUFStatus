using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TUFStatus
{
    public class TUFMANInstallation
    {
        private int installation_id;
        private string country_code;
        private int install_no;
        private string description;
        private string tufman_driver;
        private string tufman_server;
        private string tufman_database;
        private Int16 tufman_userlogin;
        private string tufman_username;
        private string tufman_password;
        private Int16 run_backup;
        private string backup_folder;
        private string backup_copy_folder;
        private string portal_driver;
        private string portal_server;
        private string portal_database;
        private Int16 portal_userlogin;
        private string portal_username;
        private string portal_password;
        private Int16 run_sync;

        public enum ApplicationList
        {
            IMSPortal=1,
            TUFMAN=2,
            TUFStatus=3,
            TUFART=4
        }

        public bool SetInstallationDetails(TUFStatus.Domain.Cloud.App.Installations installation)
        {
            try
            {
                if (installation != null)
                {
                    installation_id = installation.installation_id;
                    country_code = Program.GetDBString(installation.country_code);
                    install_no = installation.install_no;
                    description = Program.GetDBString(installation.description);
                    tufman_driver = Program.GetDBString(installation.tufman_driver);
                    tufman_server = Program.GetDBString(installation.tufman_server);
                    tufman_database = Program.GetDBString(installation.tufman_database);
                    tufman_userlogin = installation.tufman_userlogin;
                    tufman_username = Program.GetDBString(installation.tufman_username);
                    tufman_password = Program.GetDBString(installation.tufman_password);
                    run_backup = installation.run_backup;
                    backup_folder = Program.GetDBString(installation.backup_folder);
                    backup_copy_folder = Program.GetDBString(installation.backup_copy_folder);
                    portal_driver = Program.GetDBString(installation.portal_driver);
                    portal_server = Program.GetDBString(installation.portal_server);
                    portal_database = Program.GetDBString(installation.portal_database);
                    portal_userlogin = installation.portal_userlogin;
                    portal_username = Program.GetDBString(installation.portal_username);
                    portal_password = Program.GetDBString(installation.portal_password);
                    run_sync = installation.run_sync;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error setting the installation details from the cloud:", ex.Message);
                return false;
            }

            return true;
        }

        public bool SetInstallationDetails(TUFStatus.Domain.Local.App.Installations installation)
        {
            try
            {
                if (installation != null)
                {
                    installation_id = installation.installation_id;
                    country_code = Program.GetDBString(installation.country_code);
                    install_no = installation.install_no;
                    description = Program.GetDBString(installation.description);
                    tufman_driver = Program.GetDBString(installation.tufman_driver);
                    tufman_server = Program.GetDBString(installation.tufman_server);
                    tufman_database = Program.GetDBString(installation.tufman_database);
                    tufman_userlogin = installation.tufman_userlogin;
                    tufman_username = Program.GetDBString(installation.tufman_username);
                    tufman_password = Program.GetDBString(installation.tufman_password);
                    run_backup = installation.run_backup;
                    backup_folder = Program.GetDBString(installation.backup_folder);
                    backup_copy_folder = Program.GetDBString(installation.backup_copy_folder);
                    portal_driver = Program.GetDBString(installation.portal_driver);
                    portal_server = Program.GetDBString(installation.portal_server);
                    portal_database = Program.GetDBString(installation.portal_database);
                    portal_userlogin = installation.portal_userlogin;
                    portal_username = Program.GetDBString(installation.portal_username);
                    portal_password = Program.GetDBString(installation.portal_password);
                    run_sync = installation.run_sync;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "There was an error setting the installation details from local:", ex.Message);
                return false;
            }

            return true;
        }

        public ActionLog RunRecon(string gearcode, int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANRecon;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = recon.run_recon '" + gearcode + "'," + mode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = gearcode + " Recon successful, mode=" + mode;
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, gearcode, "There was an error running the Recon, mode=" + mode, ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, gearcode, result, message, error);

            return actionLog;
        }

        public ActionLog RunSync(int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.Syncronisation;

            if (run_sync > 0)
            {
                try
                {
                    Classes.Synchroniser synchroniser = new Classes.Synchroniser();
                    result = synchroniser.Synchronise(Program.TufmanInstallationID, (CloudStatusDB)Program.cloudStatusDB, 0);
                    message = "Syncronisation successful";
                }
                catch (Exception ex)
                {
                    error = true;
                    message = ex.Message;
                    ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error running the Sync, mode=" + mode, ex.Message);
                }

                actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, "", result, message, error);
            }
            return actionLog;
        }

        public ActionLog RunLicenseLinking(int mode)
        {
            ActionLog actionLog;
            int result = 0;
            bool error = false;
            string message;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANLicenseLinking;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = tufman.update_all_lic_links " + mode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = "License link successful, mode=" + mode;
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error running the license linking, mode=" + mode, ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            // Setup Action log
            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, "", result, message, error);

            return actionLog;
        }

        public ActionLog RunNationalFleetLinking(int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANNationalFleetLinking;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = tufman.update_all_nat_fleet_links " + mode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = "National fleet link successful, mode=" + mode;
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error running the national fleet linking, mode=" + mode, ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, "", result, message, error);

            return actionLog;
        }

        public ActionLog RunUpdateCatchFlagCodes(int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANUpdateCatchFlagCodes;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = tufman.update_all_log_catch_flag_codes " + mode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = "Update catch flag codes successful, mode=" + mode;
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error updating the catch flag codes, mode=" + mode, ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, "", result, message, error);

            return actionLog;
        }

        public ActionLog RunUpdateLogsheetLinking(string gearcode, int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANRecon;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = tufman.linklogs_" + gearcode + " " + mode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = "Logsheet link for " + gearcode + " successful, mode=" + mode;
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, gearcode, "There was an error linking logsheets, mode=" + mode, ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, gearcode, result, message, error);

            return actionLog;
        }

        public ActionLog RunEstimateCatch(string gearcode, int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANRecon;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = tufman.estimate_catch_" + gearcode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = "Estimate catch for " + gearcode + " successful";
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, gearcode, "There was an error estimating catch", ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, gearcode, result, message, error);

            return actionLog;
        }

        public ActionLog RunEstimateHooks(string gearcode, int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANRecon;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = tufman.estimate_hooks_" + gearcode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = "Estimate hooks for " + gearcode + " successful";
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, gearcode, "There was an error estimating hooks", ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, gearcode, result, message, error);

            return actionLog;
        }

        public ActionLog RunRaiseCatch(string gearcode, int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANRecon;

            try
            {
                sqlConnection.ConnectionString = GetConnectionString();
                sqlConnection.Open();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 600;
                sqlCommand.CommandText = "DECLARE @nResult int; exec @nResult = tufman.raise_catches_from_unloadings_" + gearcode + "; SELECT @nResult";
                result = (int)sqlCommand.ExecuteScalar();

                // report success to the log
                message = "Raise catch for " + gearcode + " successful";
            }
            catch (Exception ex)
            {
                error = true;
                message = ex.Message;
                ErrorHandler.Instance.HandleError(actionTypeID, gearcode, "There was an error raising catch", ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();
            }

            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, gearcode, result, message, error);

            return actionLog;
        }

        public ActionLog RunDBBackup(int mode)
        {
            ActionLog actionLog = null;
            int result = 0;
            string message = "";
            bool error = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = new SqlParameter();
            ActionLog.ActionTypes actionTypeID = ActionLog.ActionTypes.TUFMANBackup;
            string strSQL;
            string filename;
            string compressedFilename;
            string backupPath; 
            string backupCopyPath;

            filename = tufman_database + "_day_" + ((System.DateTime.Now.Day) % 7).ToString();

            backupPath = backup_folder;
            backupCopyPath = backup_copy_folder;

            if ((backupPath !=null) &&  (backupPath.Length > 0))
            {
                strSQL = "BACKUP DATABASE " + tufman_database  
                       + " TO DISK = N'" + backupPath + "\\" + filename + ".bak" + "' " 
                       + " WITH RETAINDAYS = 21, NAME = N'Full Database Backup', " 
                       + " NOFORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10,CONTINUE_AFTER_ERROR";

                try
                {
                    sqlConnection.ConnectionString = GetConnectionString();
                    sqlConnection.Open();

                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 0;
                    sqlCommand.CommandText = strSQL;
                    result = (int)sqlCommand.ExecuteNonQuery();

                    // report success to the log
                    message = "Backup successful to " + backupPath + "\\" + filename + ".bak" + ", mode=" + mode;

                    try
                    {
                        // compress file and create weekly and monthly backup files
                        if (System.IO.File.Exists(backupPath + "\\7za.exe"))
                        {
                            compressedFilename = filename + ".zip";

                            // delete file if it exists
                            if (System.IO.File.Exists(backupPath + "\\" + compressedFilename))
                                System.IO.File.Delete(backupPath + "\\" + compressedFilename);

                            // compress
                            string strCommand;

                            strCommand = "a -tzip " + filename + ".zip " + filename + ".bak";
                            //MessageBox.Show(strCommand)

                            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
                            processStartInfo.FileName = "7za.exe";
                            processStartInfo.Arguments = strCommand;
                            processStartInfo.WorkingDirectory = backupPath;
                            System.Diagnostics.Process process = System.Diagnostics.Process.Start(processStartInfo);
                            process.WaitForExit();

                            // delete original backup file if compressed file successful
                            if (System.IO.File.Exists(backupPath + "\\" + compressedFilename))
                                System.IO.File.Delete(backupPath + "\\" + filename + ".bak");

                            // copy to weekly and monthly, and to backup_copy if setup
                            string fileCopyName;
                            if (System.IO.Directory.Exists(backupCopyPath))
                                System.IO.File.Copy(backupPath + "\\" + compressedFilename, backupCopyPath + "\\" + compressedFilename, true);

                            // weekly
                            fileCopyName = tufman_database + "_wk_" + (((int)(System.DateTime.Now.Day + 1) / 7) + 1).ToString() + ".zip";
                            System.IO.File.Copy(backupPath + "\\" + compressedFilename, backupPath + "\\" + fileCopyName, true);
                            System.IO.File.Copy(backupPath + "\\" + fileCopyName, backupCopyPath + "\\" + fileCopyName, true);

                            // monthly
                            fileCopyName = tufman_database + "_mon_" + System.DateTime.Now.Month.ToString("00") + ".zip";
                            System.IO.File.Copy(backupPath + "\\" + compressedFilename, backupPath + "\\" + fileCopyName, true);
                            System.IO.File.Copy(backupPath + "\\" + fileCopyName, backupCopyPath + "\\" + fileCopyName, true);

                        }
                        else
                            // report that 7za doesn't exist
                            ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error compressing or copying the backups", "7za.exe not found");
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error compressing or copying the backups", ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    error = true;
                    message = ex.Message;
                    ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error backing up the DB, mode==" + mode, ex.Message);
                    ErrorHandler.Instance.HandleError(actionTypeID, "", "There was an error backing up the DB, mode==" + mode, strSQL);
                }
                finally
                {
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
            else
            {
                error = true;
                message = "There is no backup folder set";
            }
            actionLog = new ActionLog(installation_id, ApplicationList.TUFStatus, actionTypeID, "", result, message, error);

            return actionLog;
        }

        public string GetConnectionString()
        {
            if (tufman_userlogin == 1)
                return "Server=" + tufman_server.Trim() + ";Database=" + tufman_database.Trim() + ";Connection Timeout=10;User Id=" + tufman_username.Trim() + ";Password=" + Decodare(tufman_password.Trim()) + ";";
            else
                return "Server=" + tufman_server.Trim() + ";Database=" + tufman_database.Trim() + ";Connection Timeout=10;Trusted_Connection=Yes;";
        }

        public string PortalDatabase()
        {
            return portal_database;
        }

        public string CountryCode()
        {
            return country_code;
        }

        private string Decodare(string strWord)
        {
            string result="";
            int n;

            for (n = 0; n <= strWord.Length - 1; n++)
                result = result + (char)((int)(strWord.ElementAt(n)) - 5);

            return result;

        }


    }
}
