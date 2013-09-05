using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TUFStatus
{
    public partial class FormLocalLogs : Form
    {
        private BindingSource bindingSource1 = new BindingSource();

        public FormLocalLogs()
        {
            InitializeComponent();

            //actionLogsBindingSource.
        }

        private void buttonActionLogs_Click(object sender, EventArgs e)
        {
            dataGridViewLogs.AutoGenerateColumns = true;

            bindingSource1.DataSource = Program.localStatusDB.GetLocalActionLogs();
            dataGridViewLogs.DataSource = bindingSource1;

            // Automatically resize the visible rows.
            dataGridViewLogs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            // Set the DataGridView control's border.
            dataGridViewLogs.BorderStyle = BorderStyle.Fixed3D;
        }

        private void buttonErrorLogs_Click(object sender, EventArgs e)
        {
            dataGridViewLogs.AutoGenerateColumns = true;

            bindingSource1.DataSource = Program.localStatusDB.GetLocalErrorLogs();
            dataGridViewLogs.DataSource = bindingSource1;

            // Automatically resize the visible rows.
            dataGridViewLogs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            // Set the DataGridView control's border.
            dataGridViewLogs.BorderStyle = BorderStyle.Fixed3D;
        }
    }
}
