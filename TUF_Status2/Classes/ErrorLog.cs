using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUFStatus
{
    public class ErrorLog
    {
        private int _InstallationID;
        private TUFMANInstallation.ApplicationList _ApplicationID;
        private ActionLog.ActionTypes _ActionTypeID;
        private string _GearCode;
        private DateTime _ErrorTime;
        private string _ErrorInfo;
        private string _ErrorMessage;

        public ErrorLog(int InstID, TUFMANInstallation.ApplicationList AppID, ActionLog.ActionTypes ActionTypeID, string GearCode, string ErrorInfo, string ErrorMSG)
        {
            _InstallationID = InstID;
            _ApplicationID = AppID;
            _ActionTypeID = ActionTypeID;
            _GearCode = GearCode;
            _ErrorTime = System.DateTime.Now;
            _ErrorInfo = ErrorInfo;
            _ErrorMessage = ErrorMSG;
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

        public DateTime ErrorTime
        {
            get
            {
                return _ErrorTime;
            }
        }

        public string ErrorInfo
        {
            get
            {
                return _ErrorInfo;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
        }
    }
}
