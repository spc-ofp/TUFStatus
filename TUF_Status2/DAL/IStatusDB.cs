using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NHibernate;

namespace TUFStatus.DAL
{
    public interface IStatusDB
    {
        Boolean IsCloud {get;}
        Boolean IsLocal {get;}
        Boolean IsConnected {get;}
        System.Data.ConnectionState ConnectionState {get;}

        Boolean OpenConnection();
        Boolean CloseConnection();

        Boolean WriteActionLog(ActionLog ActionLog);
        Boolean WriteStatusLog(StatusLog ActionLog);
        Boolean WriteErrorLog(ErrorLog ErrorLog);
        int WriteConnectionLog();
        Boolean WriteDisconnectionTime();

        object GetInstallationDetails();
        Boolean SaveInstallationDetails(TUFStatus.Domain.Cloud.App.Installations installation);

        int TransferActionLogs(IStatusDB destDB);
        int AppendActionLogs(List<TUFStatus.Domain.Local.Logs.ActionLogs> actionLogs, ISession localSession);
        int TransferErrorLogs(IStatusDB destDB);
        int AppendErrorLogs(List<TUFStatus.Domain.Local.Logs.ErrorLogs> errorLogs, ISession localSession);

        List<TUFStatus.Domain.Local.Logs.ActionLogs> GetLocalActionLogs();
        List<TUFStatus.Domain.Local.Logs.ErrorLogs> GetLocalErrorLogs();

        int ClearErrorLogs();
        int ClearActionLogs();

    }
}
