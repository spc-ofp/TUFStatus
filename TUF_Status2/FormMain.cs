using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FluentNHibernate;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Criterion;
using TUFStatus;
//using TUFStatus.Repositories;
using System.Diagnostics;

namespace TUFStatus
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            // test local
            //using (ISession session = NHibernateHelper.OpenLocalSession())
            //{
            //    int x;

            //    //write to connection log and return connection id
            //    Domain.Local.Logs.Action_Logs action_log = session.Get<Domain.Local.Logs.Action_Logs>(193);

            //    x = action_log.application_id;
            //    MessageBox.Show(x.ToString() + ":" + action_log.installation.description.ToString());

            //    //test_add_new_connection_log(session);
            //}

            // test cloud
            //using (ISession session = NHibernateHelper.OpenCloudSession())
            //{
            //    int x;

            //    //write to connection log and return connection id
            //    Domain.Cloud.Logs.Connection_Logs connection_log = session.Get<Domain.Cloud.Logs.Connection_Logs>(78);

            //    x = connection_log.connection_id;

            //    test_add_new_connection_log(session);
            //}

        }

        public void test_add_new_connection_log(ISession session)
        {
            var connection_log = new Domain.Cloud.Logs.ConnectionLogs { application = session.Get<TUFStatus.Domain.Cloud.App.Applications>(3), installation = session.Get<TUFStatus.Domain.Cloud.App.Installations>(1), connection_time = DateTime.Now };
            //if connection_log.disconnection_time.HasValue
            
            //IConnectionLogRepository repository = new ConnectionLogRepository();
            var repository = new TUFStatus.DAL.Repositories.Repository<ISession, Domain.Cloud.Logs.ConnectionLogs>(session);

            using (var xa = session.BeginTransaction())
            {
                repository.Add(connection_log);
                //frepo.Add(fad);
                xa.Commit();
                //frepo.Reload(fad);
                //Assert.NotNull(fad);
                //Assert.AreNotEqual(0, fad.Id);
            }
        }

        private void buttonRunRecon_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (Program.RunRecon(1))
                MessageBox.Show("Recon run successful");
            else
                MessageBox.Show("Recon finished with errors");

            Cursor.Current = Cursors.Default;
        }

        private void buttonRunPostEntryProcessing_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (Program.RunPostEntryProcessing(1))
                MessageBox.Show("Postentry run successful");
            else
                MessageBox.Show("Postentry finished with errors");

            Cursor.Current = Cursors.Default;
        }

        private void buttonTransferLogs_Click(object sender, EventArgs e)
        {
            MessageBox.Show("transferred " + Program.localStatusDB.TransferActionLogs(Program.cloudStatusDB) + " action logs");
            MessageBox.Show("transferred " + Program.localStatusDB.TransferErrorLogs(Program.cloudStatusDB) + " error logs");
        }

        private void buttonRunBackup_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Program.RunBackup(1);
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Finished");
        }

        private void buttonClearLocalLogs_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cleared " + Program.localStatusDB.ClearActionLogs().ToString() + " action logs");
            MessageBox.Show("Cleared " + Program.localStatusDB.ClearErrorLogs().ToString() + " error logs");
        }

        private void buttonViewLocalLogs_Click(object sender, EventArgs e)
        {
            FormLocalLogs frmlocalLogs = new FormLocalLogs();

            frmlocalLogs.Show();
        }

        private void buttonRunSync_Click(object sender, EventArgs e)
        {
            Classes.Synchroniser synchroniser = new Classes.Synchroniser();
            int result = 0;

            Cursor.Current = Cursors.WaitCursor;
            result = synchroniser.Synchronise(Program.TufmanInstallationID, (CloudStatusDB)Program.cloudStatusDB,0);
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Finished");
        }
    }
}
