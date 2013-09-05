using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TUFStatus
{
    public class ErrorHandler
    {
        private static ErrorHandler instance;
        private DAL.IStatusDB localStatusDB;
        private bool useMessageBox;
        private int installationID;
        private TUFMANInstallation.ApplicationList applicationID;
        private TextLog textLog;

        private ErrorHandler() {}


        public static ErrorHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ErrorHandler();
                }
                return instance;
            }
        }

        public void Assign(DAL.IStatusDB statusDB, TextLog textlog, bool useMessage, int installID, TUFMANInstallation.ApplicationList appID)
        {
            if (localStatusDB != null)
                localStatusDB = null;

            if (textLog != null)
                textLog = null;

            localStatusDB = statusDB;
            useMessageBox = useMessage;
            installationID = installID;
            applicationID = appID;
            textLog = textlog;
        }

        public void HandleError(ActionLog.ActionTypes actionID, string gearcode, string info, string errMessage, bool writeToDB = true)
        {
            if (useMessageBox)
            {
                string message;

                message = info + Environment.NewLine + Environment.NewLine + errMessage;

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // save error to the local DB
            if (writeToDB)
            {
                ErrorLog errorLog = new ErrorLog(Program.TufmanInstallationID, TUFStatus.TUFMANInstallation.ApplicationList.TUFStatus,actionID, gearcode, info, errMessage);

                if (localStatusDB != null)
                    localStatusDB.WriteErrorLog(errorLog);
            }
            // write to the local log file
            textLog.WriteErrorLog(gearcode, info, errMessage);
        }
    }
}
