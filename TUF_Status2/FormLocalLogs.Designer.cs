namespace TUFStatus
{
    partial class FormLocalLogs
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
            this.buttonActionLogs = new System.Windows.Forms.Button();
            this.buttonErrorLogs = new System.Windows.Forms.Button();
            this.dataGridViewLogs = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonActionLogs
            // 
            this.buttonActionLogs.Location = new System.Drawing.Point(12, 12);
            this.buttonActionLogs.Name = "buttonActionLogs";
            this.buttonActionLogs.Size = new System.Drawing.Size(109, 23);
            this.buttonActionLogs.TabIndex = 0;
            this.buttonActionLogs.Text = "Action logs";
            this.buttonActionLogs.UseVisualStyleBackColor = true;
            this.buttonActionLogs.Click += new System.EventHandler(this.buttonActionLogs_Click);
            // 
            // buttonErrorLogs
            // 
            this.buttonErrorLogs.Location = new System.Drawing.Point(127, 12);
            this.buttonErrorLogs.Name = "buttonErrorLogs";
            this.buttonErrorLogs.Size = new System.Drawing.Size(109, 23);
            this.buttonErrorLogs.TabIndex = 1;
            this.buttonErrorLogs.Text = "Error logs";
            this.buttonErrorLogs.UseVisualStyleBackColor = true;
            this.buttonErrorLogs.Click += new System.EventHandler(this.buttonErrorLogs_Click);
            // 
            // dataGridViewLogs
            // 
            this.dataGridViewLogs.AllowUserToAddRows = false;
            this.dataGridViewLogs.AllowUserToDeleteRows = false;
            this.dataGridViewLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLogs.Location = new System.Drawing.Point(12, 41);
            this.dataGridViewLogs.Name = "dataGridViewLogs";
            this.dataGridViewLogs.ReadOnly = true;
            this.dataGridViewLogs.Size = new System.Drawing.Size(1006, 479);
            this.dataGridViewLogs.TabIndex = 2;
            // 
            // FormLocalLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 532);
            this.Controls.Add(this.dataGridViewLogs);
            this.Controls.Add(this.buttonErrorLogs);
            this.Controls.Add(this.buttonActionLogs);
            this.Name = "FormLocalLogs";
            this.Text = "local log Browser";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLogs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonActionLogs;
        private System.Windows.Forms.Button buttonErrorLogs;
        private System.Windows.Forms.DataGridView dataGridViewLogs;
    }
}