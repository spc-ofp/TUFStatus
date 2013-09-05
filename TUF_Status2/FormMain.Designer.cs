namespace TUFStatus
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRunRecon = new System.Windows.Forms.Button();
            this.buttonRunPostEntryProcessing = new System.Windows.Forms.Button();
            this.buttonTransferLogs = new System.Windows.Forms.Button();
            this.buttonRunBackup = new System.Windows.Forms.Button();
            this.buttonClearLocalLogs = new System.Windows.Forms.Button();
            this.buttonViewLocalLogs = new System.Windows.Forms.Button();
            this.buttonRunSync = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRunRecon
            // 
            this.buttonRunRecon.Location = new System.Drawing.Point(152, 68);
            this.buttonRunRecon.Name = "buttonRunRecon";
            this.buttonRunRecon.Size = new System.Drawing.Size(158, 30);
            this.buttonRunRecon.TabIndex = 0;
            this.buttonRunRecon.Text = "Run Recon";
            this.buttonRunRecon.UseVisualStyleBackColor = true;
            this.buttonRunRecon.Click += new System.EventHandler(this.buttonRunRecon_Click);
            // 
            // buttonRunPostEntryProcessing
            // 
            this.buttonRunPostEntryProcessing.Location = new System.Drawing.Point(152, 104);
            this.buttonRunPostEntryProcessing.Name = "buttonRunPostEntryProcessing";
            this.buttonRunPostEntryProcessing.Size = new System.Drawing.Size(158, 30);
            this.buttonRunPostEntryProcessing.TabIndex = 1;
            this.buttonRunPostEntryProcessing.Text = "Run Post-Entry Processing";
            this.buttonRunPostEntryProcessing.UseVisualStyleBackColor = true;
            this.buttonRunPostEntryProcessing.Click += new System.EventHandler(this.buttonRunPostEntryProcessing_Click);
            // 
            // buttonTransferLogs
            // 
            this.buttonTransferLogs.Location = new System.Drawing.Point(152, 140);
            this.buttonTransferLogs.Name = "buttonTransferLogs";
            this.buttonTransferLogs.Size = new System.Drawing.Size(158, 30);
            this.buttonTransferLogs.TabIndex = 2;
            this.buttonTransferLogs.Text = "Transfer Logs";
            this.buttonTransferLogs.UseVisualStyleBackColor = true;
            this.buttonTransferLogs.Click += new System.EventHandler(this.buttonTransferLogs_Click);
            // 
            // buttonRunBackup
            // 
            this.buttonRunBackup.Location = new System.Drawing.Point(152, 176);
            this.buttonRunBackup.Name = "buttonRunBackup";
            this.buttonRunBackup.Size = new System.Drawing.Size(158, 30);
            this.buttonRunBackup.TabIndex = 3;
            this.buttonRunBackup.Text = "Run Backup";
            this.buttonRunBackup.UseVisualStyleBackColor = true;
            this.buttonRunBackup.Click += new System.EventHandler(this.buttonRunBackup_Click);
            // 
            // buttonClearLocalLogs
            // 
            this.buttonClearLocalLogs.Location = new System.Drawing.Point(152, 248);
            this.buttonClearLocalLogs.Name = "buttonClearLocalLogs";
            this.buttonClearLocalLogs.Size = new System.Drawing.Size(158, 30);
            this.buttonClearLocalLogs.TabIndex = 4;
            this.buttonClearLocalLogs.Text = "Clear Local Logs";
            this.buttonClearLocalLogs.UseVisualStyleBackColor = true;
            this.buttonClearLocalLogs.Click += new System.EventHandler(this.buttonClearLocalLogs_Click);
            // 
            // buttonViewLocalLogs
            // 
            this.buttonViewLocalLogs.Location = new System.Drawing.Point(152, 351);
            this.buttonViewLocalLogs.Name = "buttonViewLocalLogs";
            this.buttonViewLocalLogs.Size = new System.Drawing.Size(158, 30);
            this.buttonViewLocalLogs.TabIndex = 5;
            this.buttonViewLocalLogs.Text = "View Local Logs";
            this.buttonViewLocalLogs.UseVisualStyleBackColor = true;
            this.buttonViewLocalLogs.Click += new System.EventHandler(this.buttonViewLocalLogs_Click);
            // 
            // buttonRunSync
            // 
            this.buttonRunSync.Location = new System.Drawing.Point(152, 212);
            this.buttonRunSync.Name = "buttonRunSync";
            this.buttonRunSync.Size = new System.Drawing.Size(158, 30);
            this.buttonRunSync.TabIndex = 6;
            this.buttonRunSync.Text = "Run Sync";
            this.buttonRunSync.UseVisualStyleBackColor = true;
            this.buttonRunSync.Click += new System.EventHandler(this.buttonRunSync_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 429);
            this.Controls.Add(this.buttonRunSync);
            this.Controls.Add(this.buttonViewLocalLogs);
            this.Controls.Add(this.buttonClearLocalLogs);
            this.Controls.Add(this.buttonRunBackup);
            this.Controls.Add(this.buttonTransferLogs);
            this.Controls.Add(this.buttonRunPostEntryProcessing);
            this.Controls.Add(this.buttonRunRecon);
            this.Name = "FormMain";
            this.Text = "TUFStatus";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRunRecon;
        private System.Windows.Forms.Button buttonRunPostEntryProcessing;
        private System.Windows.Forms.Button buttonTransferLogs;
        private System.Windows.Forms.Button buttonRunBackup;
        private System.Windows.Forms.Button buttonClearLocalLogs;
        private System.Windows.Forms.Button buttonViewLocalLogs;
        private System.Windows.Forms.Button buttonRunSync;
    }
}

