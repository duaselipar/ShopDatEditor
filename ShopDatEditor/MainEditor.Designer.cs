namespace ShopDatEditor
{
    partial class MainEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainEditor));
            btnLoad = new Button();
            btnSave = new Button();
            dataShop = new DataGridView();
            dataItem = new DataGridView();
            txtClientpath = new TextBox();
            btnSelect = new Button();
            btnNewshop = new Button();
            btnNewitem = new Button();
            groupBox1 = new GroupBox();
            btnUpdateshop = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            btnNameupdate = new Button();
            btnDbconnect = new Button();
            txtDb = new TextBox();
            txtPass = new TextBox();
            txtUser = new TextBox();
            txtPort = new TextBox();
            txtHostname = new TextBox();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataShop).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataItem).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(637, 11);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 25);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(1088, 14);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(113, 48);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // dataShop
            // 
            dataShop.AllowUserToAddRows = false;
            dataShop.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataShop.Location = new Point(12, 83);
            dataShop.Name = "dataShop";
            dataShop.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataShop.Size = new Size(421, 532);
            dataShop.TabIndex = 2;
            // 
            // dataItem
            // 
            dataItem.AllowUserToAddRows = false;
            dataItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataItem.Location = new Point(439, 83);
            dataItem.Name = "dataItem";
            dataItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataItem.Size = new Size(762, 532);
            dataItem.TabIndex = 3;
            // 
            // txtClientpath
            // 
            txtClientpath.Location = new Point(116, 11);
            txtClientpath.Name = "txtClientpath";
            txtClientpath.Size = new Size(429, 23);
            txtClientpath.TabIndex = 4;
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(556, 11);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(75, 26);
            btnSelect.TabIndex = 5;
            btnSelect.Text = "Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnNewshop
            // 
            btnNewshop.Location = new Point(12, 45);
            btnNewshop.Name = "btnNewshop";
            btnNewshop.Size = new Size(117, 32);
            btnNewshop.TabIndex = 6;
            btnNewshop.Text = "New Shop";
            btnNewshop.UseVisualStyleBackColor = true;
            btnNewshop.Click += btnNewshop_Click;
            // 
            // btnNewitem
            // 
            btnNewitem.Location = new Point(439, 45);
            btnNewitem.Name = "btnNewitem";
            btnNewitem.Size = new Size(117, 32);
            btnNewitem.TabIndex = 7;
            btnNewitem.Text = "New Item";
            btnNewitem.UseVisualStyleBackColor = true;
            btnNewitem.Click += btnNewitem_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnUpdateshop);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnNameupdate);
            groupBox1.Controls.Add(btnDbconnect);
            groupBox1.Controls.Add(txtDb);
            groupBox1.Controls.Add(txtPass);
            groupBox1.Controls.Add(txtUser);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtHostname);
            groupBox1.Location = new Point(12, 631);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1189, 100);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "MySQL Connection";
            // 
            // btnUpdateshop
            // 
            btnUpdateshop.Location = new Point(794, 59);
            btnUpdateshop.Name = "btnUpdateshop";
            btnUpdateshop.Size = new Size(276, 23);
            btnUpdateshop.TabIndex = 12;
            btnUpdateshop.Text = "Update All Item Shop";
            btnUpdateshop.UseVisualStyleBackColor = true;
            btnUpdateshop.Click += btnUpdateshop_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(387, 35);
            label5.Name = "label5";
            label5.Size = new Size(61, 15);
            label5.TabIndex = 11;
            label5.Text = "Database :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(54, 63);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 10;
            label4.Text = "Port :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(236, 34);
            label3.Name = "label3";
            label3.Size = new Size(36, 15);
            label3.TabIndex = 9;
            label3.Text = "User :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(209, 63);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 8;
            label2.Text = "Password :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 35);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 7;
            label1.Text = "Hostname :";
            // 
            // btnNameupdate
            // 
            btnNameupdate.Location = new Point(794, 30);
            btnNameupdate.Name = "btnNameupdate";
            btnNameupdate.Size = new Size(276, 23);
            btnNameupdate.TabIndex = 6;
            btnNameupdate.Text = "Update Shop Name From Database";
            btnNameupdate.UseVisualStyleBackColor = true;
            btnNameupdate.Click += btnNameupdate_Click;
            // 
            // btnDbconnect
            // 
            btnDbconnect.Location = new Point(590, 32);
            btnDbconnect.Name = "btnDbconnect";
            btnDbconnect.Size = new Size(118, 46);
            btnDbconnect.TabIndex = 5;
            btnDbconnect.Text = "Connect";
            btnDbconnect.UseVisualStyleBackColor = true;
            btnDbconnect.Click += btnDbconnect_Click;
            // 
            // txtDb
            // 
            txtDb.Location = new Point(454, 31);
            txtDb.Name = "txtDb";
            txtDb.Size = new Size(100, 23);
            txtDb.TabIndex = 4;
            // 
            // txtPass
            // 
            txtPass.Location = new Point(278, 60);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(100, 23);
            txtPass.TabIndex = 3;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(278, 32);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(100, 23);
            txtUser.TabIndex = 2;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(95, 60);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 23);
            txtPort.TabIndex = 1;
            // 
            // txtHostname
            // 
            txtHostname.Location = new Point(95, 31);
            txtHostname.Name = "txtHostname";
            txtHostname.Size = new Size(100, 23);
            txtHostname.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 14);
            label6.Name = "label6";
            label6.Size = new Size(98, 15);
            label6.TabIndex = 9;
            label6.Text = "Your Client Path :";
            // 
            // MainEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1213, 740);
            Controls.Add(label6);
            Controls.Add(txtClientpath);
            Controls.Add(btnLoad);
            Controls.Add(btnSelect);
            Controls.Add(groupBox1);
            Controls.Add(btnNewitem);
            Controls.Add(btnNewshop);
            Controls.Add(dataItem);
            Controls.Add(dataShop);
            Controls.Add(btnSave);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Shop Editor By DuaSelipar";
            ((System.ComponentModel.ISupportInitialize)dataShop).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataItem).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLoad;
        private Button btnSave;
        private DataGridView dataShop;
        private DataGridView dataItem;
        private TextBox txtClientpath;
        private Button btnSelect;
        private Button btnNewshop;
        private Button btnNewitem;
        private GroupBox groupBox1;
        private TextBox txtUser;
        private TextBox txtPort;
        private TextBox txtHostname;
        private Button btnNameupdate;
        private Button btnDbconnect;
        private TextBox txtDb;
        private TextBox txtPass;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnUpdateshop;
        private Label label6;
    }
}
