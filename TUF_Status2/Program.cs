using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TUFStatus.DAL.Configuration;

namespace TUFStatus
{
    public static class Program
    {
        public static int RunMode;
        public static int TufmanInstallationID;
        public static TextLog TextLogFile = new TextLog();
        public static TUFMANInstallation tufmanInstallation = new TUFMANInstallation();

        public static DAL.IStatusDB cloudStatusDB;
        public static DAL.IStatusDB localStatusDB;

        //public static NHibernateHelper nHibernateHelper;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {//App_Start.NHibernateProfilerBootstrapper.PreStart();

            bool useCloud;

            //MessageBox.Show("1");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0) 
            {
                RunMode = Convert.ToInt16(args[0]);

                //MessageBox.Show("Start mode is " + RunMode.ToString());
            }
            else
            {
                RunMode = 0;
            }

            Program.TextLogFile.WriteLog("Application started : mode=" + RunMode.ToString());

            // ---------------------------------------------------------------------------------------
            // initialisation of parameters
            // ---------------------------------------------------------------------------------------            
            TufmanInstallationID = Properties.Settings.Default.tufman_installation_id;
            cloudStatusDB = new CloudStatusDB(NHibernateHelper.OpenCloudSession());

            localStatusDB = new LocalStatusDB(NHibernateHelper.OpenLocalSession());

            ErrorHandler.Instance.Assign(localStatusDB, TextLogFile, (RunMode == 0), TufmanInstallationID, TUFMANInstallation.ApplicationList.TUFStatus);

            // ---------------------------------------------------------------------------------------
            // check connection to the local database
            // ---------------------------------------------------------------------------------------
            if (Program.localStatusDB.IsConnected)
            {
                //MessageBox.Show("Connected to the local DB");
            }
            else
            {
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "Error:", "Not connected to local DB", false);
            }

            // ---------------------------------------------------------------------------------------
            // Check connection to the cloud
            // ---------------------------------------------------------------------------------------
            if (Program.cloudStatusDB.IsConnected)
                useCloud = true;
            else
            {
                useCloud = false;
                ErrorHandler.Instance.HandleError(ActionLog.ActionTypes.Application, "", "Error:", "Not connected to cloud DB", false);
            }

            // ---------------------------------------------------------------------------------------
            // get TUFMAN installation details, either from the cloud or local if no cloud connection
            // ---------------------------------------------------------------------------------------

            if (useCloud)
            {
                TUFStatus.Domain.Cloud.App.Installations installation;
                installation = (TUFStatus.Domain.Cloud.App.Installations)cloudStatusDB.GetInstallationDetails();

                if (installation == null)
                {
                    TUFStatus.Domain.Local.App.Installations installation2;
                    installation2 = (TUFStatus.Domain.Local.App.Installations)localStatusDB.GetInstallationDetails();
                    tufmanInstallation.SetInstallationDetails(installation2);
                }
                else
                {
                    tufmanInstallation.SetInstallationDetails(installation);
                    localStatusDB.SaveInstallationDetails(installation);
                }
            }
            else
            {
                TUFStatus.Domain.Local.App.Installations installation2;
                installation2 = (TUFStatus.Domain.Local.App.Installations)localStatusDB.GetInstallationDetails();
                tufmanInstallation.SetInstallationDetails(installation2);
            }


            // ---------------------------------------------------------------------------------------
            // Create connection log and get connection ID
            // ---------------------------------------------------------------------------------------

            if (useCloud)
                cloudStatusDB.WriteConnectionLog();

            // ---------------------------------------------------------------------------------------
            // run tasks
            // ---------------------------------------------------------------------------------------
            
            switch (RunMode)
            {
                case 0:    // manual
                    Application.Run(new FormMain());
                    break;
                case 1:     // partial
                    RunRecon(0);
                    RunPostEntryProcessing(0);
                    break;
                case 2:     // full
                    RunRecon(1);
                    RunPostEntryProcessing(1);
                    RunBackup(0);
                    RunSync(0);

                    break;
            }

            // transfer action and error logs if cloud connected
            if (useCloud)
            {
                localStatusDB.TransferActionLogs(cloudStatusDB);
                localStatusDB.TransferErrorLogs(cloudStatusDB);
            }

            // ---------------------------------------------------------------------------------------
            // tidy up
            // ---------------------------------------------------------------------------------------
            if (useCloud)
                cloudStatusDB.WriteDisconnectionTime();

            cloudStatusDB.CloseConnection();
            cloudStatusDB = null;

            localStatusDB.CloseConnection();
            localStatusDB = null;

            TextLogFile.WriteLog("Application ended");
            TextLogFile.Close();
        }

        public static bool RunRecon(int mode)
        {
            // runs the recon in TUFMAN
            // mode 0 = partial, 1 = full
            bool hadError = false;

            ActionLog actionLog;

            actionLog = tufmanInstallation.RunRecon("LL", mode);
            hadError = actionLog.HadError;
            localStatusDB.WriteActionLog(actionLog);

            actionLog = null;
            actionLog = tufmanInstallation.RunRecon("PL", mode);
            hadError = (hadError | actionLog.HadError);
            localStatusDB.WriteActionLog(actionLog);

            actionLog = null;
            actionLog = tufmanInstallation.RunRecon("PS", mode);
            hadError = (hadError | actionLog.HadError);
            localStatusDB.WriteActionLog(actionLog);

            return !hadError;
        }

        public static bool RunSync(int mode)
        {
            // runs the recon in TUFMAN
            // mode 0 = partial, 1 = full
            bool hadError = false;

            ActionLog actionLog;

            actionLog = tufmanInstallation.RunSync(mode);
            hadError = actionLog.HadError;
            if (actionLog != null)
                localStatusDB.WriteActionLog(actionLog);

            return !hadError;
        }

        public static bool RunPostEntryProcessing(int mode)
        {
            bool error = false;

            ActionLog actionLog;

            actionLog = tufmanInstallation.RunLicenseLinking(mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunNationalFleetLinking(mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunUpdateCatchFlagCodes(mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunUpdateLogsheetLinking("LL",mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunUpdateLogsheetLinking("PS",mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunUpdateLogsheetLinking("PL",mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunEstimateCatch("LL",mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunEstimateHooks("LL",mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            actionLog = tufmanInstallation.RunRaiseCatch("LL",mode);
            localStatusDB.WriteActionLog(actionLog);
            error = (error | actionLog.HadError);

            return !(error | actionLog.HadError);
        }

        public static bool RunBackup(int mode)
        {
            ActionLog actionLog;

            actionLog = tufmanInstallation.RunDBBackup(mode);

            return !actionLog.HadError;
        }

        public static string GetDBString(object dbString)
        {
            // function returns a string from a DB object, i.e. could be null, and trims the string
            if (dbString == null)
                return "";
            else
            {
                if (dbString.GetType()==typeof(string))
                    return dbString.ToString().Trim();
                else
                    return "";
            }
        }
    }
}

