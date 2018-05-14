namespace LicenseManager.Client
{
    partial class OCGLicenseTool
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
            this.gbLicInfo = new System.Windows.Forms.GroupBox();
            this.txtClientName = new System.Windows.Forms.TextBox();
            this.lblClientName = new System.Windows.Forms.Label();
            this.cmbApplicationName = new System.Windows.Forms.ComboBox();
            this.btnDeleteComputer = new System.Windows.Forms.Button();
            this.btnAddComputer = new System.Windows.Forms.Button();
            this.dtExpireDate = new System.Windows.Forms.DateTimePicker();
            this.lblExpiredate = new System.Windows.Forms.Label();
            this.cbLicenseCount = new System.Windows.Forms.CheckedListBox();
            this.lblLicenseCount = new System.Windows.Forms.Label();
            this.lvComputername = new System.Windows.Forms.ListView();
            this.lblComputerName = new System.Windows.Forms.Label();
            this.lblAppName = new System.Windows.Forms.Label();
            this.btnCreateLicense = new System.Windows.Forms.Button();
            this.chkServer = new System.Windows.Forms.CheckBox();
            this.gbLicInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLicInfo
            // 
            this.gbLicInfo.Controls.Add(this.chkServer);
            this.gbLicInfo.Controls.Add(this.txtClientName);
            this.gbLicInfo.Controls.Add(this.lblClientName);
            this.gbLicInfo.Controls.Add(this.cmbApplicationName);
            this.gbLicInfo.Controls.Add(this.btnDeleteComputer);
            this.gbLicInfo.Controls.Add(this.btnAddComputer);
            this.gbLicInfo.Controls.Add(this.dtExpireDate);
            this.gbLicInfo.Controls.Add(this.lblExpiredate);
            this.gbLicInfo.Controls.Add(this.cbLicenseCount);
            this.gbLicInfo.Controls.Add(this.lblLicenseCount);
            this.gbLicInfo.Controls.Add(this.lvComputername);
            this.gbLicInfo.Controls.Add(this.lblComputerName);
            this.gbLicInfo.Controls.Add(this.lblAppName);
            this.gbLicInfo.Location = new System.Drawing.Point(5, 8);
            this.gbLicInfo.Name = "gbLicInfo";
            this.gbLicInfo.Size = new System.Drawing.Size(450, 307);
            this.gbLicInfo.TabIndex = 0;
            this.gbLicInfo.TabStop = false;
            this.gbLicInfo.Text = "License Information";
            // 
            // txtClientName
            // 
            this.txtClientName.Location = new System.Drawing.Point(19, 37);
            this.txtClientName.Name = "txtClientName";
            this.txtClientName.Size = new System.Drawing.Size(243, 20);
            this.txtClientName.TabIndex = 2;
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(19, 20);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(64, 13);
            this.lblClientName.TabIndex = 1;
            this.lblClientName.Text = "Client Name";
            // 
            // cmbApplicationName
            // 
            this.cmbApplicationName.FormattingEnabled = true;
            this.cmbApplicationName.Location = new System.Drawing.Point(19, 81);
            this.cmbApplicationName.Name = "cmbApplicationName";
            this.cmbApplicationName.Size = new System.Drawing.Size(246, 21);
            this.cmbApplicationName.TabIndex = 4;
            // 
            // btnDeleteComputer
            // 
            this.btnDeleteComputer.Location = new System.Drawing.Point(269, 215);
            this.btnDeleteComputer.Name = "btnDeleteComputer";
            this.btnDeleteComputer.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteComputer.TabIndex = 10;
            this.btnDeleteComputer.Text = "Delete";
            this.btnDeleteComputer.UseVisualStyleBackColor = true;
            this.btnDeleteComputer.Click += new System.EventHandler(this.btnDeleteComputer_Click);
            // 
            // btnAddComputer
            // 
            this.btnAddComputer.Location = new System.Drawing.Point(269, 186);
            this.btnAddComputer.Name = "btnAddComputer";
            this.btnAddComputer.Size = new System.Drawing.Size(75, 23);
            this.btnAddComputer.TabIndex = 9;
            this.btnAddComputer.Text = "Add";
            this.btnAddComputer.UseVisualStyleBackColor = true;
            this.btnAddComputer.Click += new System.EventHandler(this.btnAddComputer_Click);
            // 
            // dtExpireDate
            // 
            this.dtExpireDate.Location = new System.Drawing.Point(19, 127);
            this.dtExpireDate.Name = "dtExpireDate";
            this.dtExpireDate.Size = new System.Drawing.Size(246, 20);
            this.dtExpireDate.TabIndex = 6;
            // 
            // lblExpiredate
            // 
            this.lblExpiredate.AutoSize = true;
            this.lblExpiredate.Location = new System.Drawing.Point(16, 110);
            this.lblExpiredate.Name = "lblExpiredate";
            this.lblExpiredate.Size = new System.Drawing.Size(62, 13);
            this.lblExpiredate.TabIndex = 5;
            this.lblExpiredate.Text = "Expire Date";
            // 
            // cbLicenseCount
            // 
            this.cbLicenseCount.CheckOnClick = true;
            this.cbLicenseCount.FormattingEnabled = true;
            this.cbLicenseCount.Items.AddRange(new object[] {
            "1000",
            "10000",
            "100000",
            "1000000"});
            this.cbLicenseCount.Location = new System.Drawing.Point(286, 37);
            this.cbLicenseCount.Name = "cbLicenseCount";
            this.cbLicenseCount.Size = new System.Drawing.Size(131, 79);
            this.cbLicenseCount.TabIndex = 12;
            this.cbLicenseCount.SelectedIndexChanged += new System.EventHandler(this.cbLicenseCount_SelectedIndexChanged);
            this.cbLicenseCount.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbLicenseCount_ItemCheck);
            // 
            // lblLicenseCount
            // 
            this.lblLicenseCount.AutoSize = true;
            this.lblLicenseCount.Location = new System.Drawing.Point(286, 20);
            this.lblLicenseCount.Name = "lblLicenseCount";
            this.lblLicenseCount.Size = new System.Drawing.Size(49, 13);
            this.lblLicenseCount.TabIndex = 11;
            this.lblLicenseCount.Text = "Licenses";
            // 
            // lvComputername
            // 
            this.lvComputername.LabelEdit = true;
            this.lvComputername.Location = new System.Drawing.Point(19, 184);
            this.lvComputername.Name = "lvComputername";
            this.lvComputername.Size = new System.Drawing.Size(246, 97);
            this.lvComputername.TabIndex = 8;
            this.lvComputername.UseCompatibleStateImageBehavior = false;
            this.lvComputername.View = System.Windows.Forms.View.List;
            // 
            // lblComputerName
            // 
            this.lblComputerName.AutoSize = true;
            this.lblComputerName.Location = new System.Drawing.Point(19, 167);
            this.lblComputerName.Name = "lblComputerName";
            this.lblComputerName.Size = new System.Drawing.Size(103, 13);
            this.lblComputerName.TabIndex = 7;
            this.lblComputerName.Text = "Licensed Computers";
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Location = new System.Drawing.Point(19, 65);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(90, 13);
            this.lblAppName.TabIndex = 3;
            this.lblAppName.Text = "Application Name";
            // 
            // btnCreateLicense
            // 
            this.btnCreateLicense.Location = new System.Drawing.Point(5, 321);
            this.btnCreateLicense.Name = "btnCreateLicense";
            this.btnCreateLicense.Size = new System.Drawing.Size(75, 23);
            this.btnCreateLicense.TabIndex = 13;
            this.btnCreateLicense.Text = "Create License";
            this.btnCreateLicense.UseVisualStyleBackColor = true;
            this.btnCreateLicense.Click += new System.EventHandler(this.btnCreateLicense_Click);
            // 
            // chkServer
            // 
            this.chkServer.AutoSize = true;
            this.chkServer.Location = new System.Drawing.Point(19, 284);
            this.chkServer.Name = "chkServer";
            this.chkServer.Size = new System.Drawing.Size(123, 17);
            this.chkServer.TabIndex = 13;
            this.chkServer.Text = "do not check Server";
            this.chkServer.UseVisualStyleBackColor = true;
            // 
            // OCGLicenseTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 352);
            this.Controls.Add(this.btnCreateLicense);
            this.Controls.Add(this.gbLicInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OCGLicenseTool";
            this.ShowIcon = false;
            this.Text = "OCG License Tool";
            this.gbLicInfo.ResumeLayout(false);
            this.gbLicInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLicInfo;
        private System.Windows.Forms.CheckedListBox cbLicenseCount;
        private System.Windows.Forms.Label lblLicenseCount;
        private System.Windows.Forms.ListView lvComputername;
        private System.Windows.Forms.Label lblComputerName;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblExpiredate;
        private System.Windows.Forms.DateTimePicker dtExpireDate;
        private System.Windows.Forms.Button btnDeleteComputer;
        private System.Windows.Forms.Button btnAddComputer;
        private System.Windows.Forms.Button btnCreateLicense;
        private System.Windows.Forms.ComboBox cmbApplicationName;
        private System.Windows.Forms.TextBox txtClientName;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.CheckBox chkServer;
    }
}

