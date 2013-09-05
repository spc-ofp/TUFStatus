using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUFStatus
{
    public class ActionLog
    {
        private int _InstallationID;
        private TUFMANInstallation.ApplicationList _ApplicationID;
        private ActionLog.ActionTypes _ActionTypeID;
        private string _GearCode;
        private DateTime _ActionTime;
        private int _ActionResult;
        private string _ActionMessage;
        private Boolean _HadError;

        public ActionLog(int InstID, TUFMANInstallation.ApplicationList AppID, ActionLog.ActionTypes ActionTypeID, string GearCode, int ActionResult, string ActionMSG, Boolean HadError)
         {
            _InstallationID = InstID;
            _ApplicationID = AppID;
            _ActionTypeID = ActionTypeID;
            _GearCode = GearCode;
            _ActionTime = System.DateTime.Now;
            _ActionResult = ActionResult;
            _ActionMessage = ActionMSG;
            _HadError = HadError;
        }

        public int InstallationID
        {
            get
            {
                return _InstallationID;
            }
        }

        public TUFMANInstallation.ApplicationList ApplicationID
        {
            get
            {
                return _ApplicationID;
            }
        }

        public ActionLog.ActionTypes ActionTypeID
        {
            get
            {
                return _ActionTypeID;
            }
        }

        public string GearCode
        {
            get
            {
                return _GearCode;
            }
        }

        public DateTime ActionTime
        {
            get
            {
                return _ActionTime;
            }
        }

        public int ActionResult
        {
            get
            {
                return _ActionResult;
            }
        }

        public string ActionMessage
        {
            get
            {
                return _ActionMessage;
            }
        }

        public Boolean HadError
        {
            get
            {
                return _HadError;
            }
        }

        //-------------------------------------------------------------------------------------------
        // ENUMERATIONS
        //-------------------------------------------------------------------------------------------

        public enum ActionTypes
        {
            Application = 0,
            TUFMANRecon = 1,
            TUFMANBackup = 2,
            TUFMANLicenseLinking = 3,
            TUFMANNationalFleetLinking = 4,
            LogTransfer = 5,
            TUFMANLogLinking = 6,
            TUFMANEstimateCatch = 7,
            TUFMANEstimateHooks = 8,
            TUFMANRaiseCatchesFromUnloadings = 9,
            TUFMANUpdateCatchFlagCodes = 10,
            Syncronisation = 11
        };
    }
}
