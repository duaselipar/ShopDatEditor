namespace ShopDatEditor
{
    partial class NewItem
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataItemList;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblGold;
        private System.Windows.Forms.Label lblEP;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblHot;
        private System.Windows.Forms.Label lblNew;
        private System.Windows.Forms.TextBox txtGold;
        private System.Windows.Forms.TextBox txtEP;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.CheckBox chkHot;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSelectedItemID;
        private System.Windows.Forms.Label lblSelectedItemName;
        private System.Windows.Forms.TextBox txtSelectedItemID;
        private System.Windows.Forms.TextBox txtSelectedItemName;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataItemList = new DataGridView();
            txtSearch = new TextBox();
            btnSearch = new Button();
            lblGold = new Label();
            lblEP = new Label();
            lblDesc = new Label();
            lblCategory = new Label();
            lblHot = new Label();
            lblNew = new Label();
            txtGold = new TextBox();
            txtEP = new TextBox();
            txtDesc = new TextBox();
            cbCategory = new ComboBox();
            chkHot = new CheckBox();
            chkNew = new CheckBox();
            btnOK = new Button();
            btnCancel = new Button();
            lblSelectedItemID = new Label();
            lblSelectedItemName = new Label();
            txtSelectedItemID = new TextBox();
            txtSelectedItemName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataItemList).BeginInit();
            SuspendLayout();
            // 
            // dataItemList
            // 
            dataItemList.AllowUserToAddRows = false;
            dataItemList.AllowUserToDeleteRows = false;
            dataItemList.AllowUserToResizeRows = false;
            dataItemList.Location = new Point(10, 40);
            dataItemList.Name = "dataItemList";
            dataItemList.RowHeadersVisible = false;
            dataItemList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataItemList.Size = new Size(200, 273);
            dataItemList.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(10, 10);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(134, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(150, 10);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(60, 23);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Search";
            // 
            // lblGold
            // 
            lblGold.AutoSize = true;
            lblGold.Location = new Point(231, 115);
            lblGold.Name = "lblGold";
            lblGold.Size = new Size(35, 15);
            lblGold.TabIndex = 6;
            lblGold.Text = "Gold:";
            // 
            // lblEP
            // 
            lblEP.AutoSize = true;
            lblEP.Location = new Point(231, 150);
            lblEP.Name = "lblEP";
            lblEP.Size = new Size(23, 15);
            lblEP.TabIndex = 8;
            lblEP.Text = "EP:";
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(231, 220);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(35, 15);
            lblDesc.TabIndex = 12;
            lblDesc.Text = "Desc:";
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(231, 185);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(58, 15);
            lblCategory.TabIndex = 10;
            lblCategory.Text = "Category:";
            // 
            // lblHot
            // 
            lblHot.AutoSize = true;
            lblHot.Location = new Point(243, 255);
            lblHot.Name = "lblHot";
            lblHot.Size = new Size(30, 15);
            lblHot.TabIndex = 14;
            lblHot.Text = "Hot:";
            // 
            // lblNew
            // 
            lblNew.AutoSize = true;
            lblNew.Location = new Point(331, 255);
            lblNew.Name = "lblNew";
            lblNew.Size = new Size(34, 15);
            lblNew.TabIndex = 16;
            lblNew.Text = "New:";
            // 
            // txtGold
            // 
            txtGold.Location = new Point(301, 112);
            txtGold.Name = "txtGold";
            txtGold.Size = new Size(120, 23);
            txtGold.TabIndex = 7;
            // 
            // txtEP
            // 
            txtEP.Location = new Point(301, 147);
            txtEP.Name = "txtEP";
            txtEP.Size = new Size(120, 23);
            txtEP.TabIndex = 9;
            // 
            // txtDesc
            // 
            txtDesc.Location = new Point(301, 217);
            txtDesc.MaxLength = 127;
            txtDesc.Name = "txtDesc";
            txtDesc.Size = new Size(120, 23);
            txtDesc.TabIndex = 13;
            // 
            // cbCategory
            // 
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategory.Items.AddRange(new object[] { "egg", "item", "equipment", "recommend" });
            cbCategory.Location = new Point(301, 182);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(120, 23);
            cbCategory.TabIndex = 11;
            // 
            // chkHot
            // 
            chkHot.Location = new Point(284, 253);
            chkHot.Name = "chkHot";
            chkHot.Size = new Size(50, 20);
            chkHot.TabIndex = 15;
            // 
            // chkNew
            // 
            chkNew.Location = new Point(371, 253);
            chkNew.Name = "chkNew";
            chkNew.Size = new Size(50, 20);
            chkNew.TabIndex = 17;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(231, 285);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(80, 28);
            btnOK.TabIndex = 18;
            btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(331, 285);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 28);
            btnCancel.TabIndex = 19;
            btnCancel.Text = "Cancel";
            // 
            // lblSelectedItemID
            // 
            lblSelectedItemID.AutoSize = true;
            lblSelectedItemID.Location = new Point(231, 45);
            lblSelectedItemID.Name = "lblSelectedItemID";
            lblSelectedItemID.Size = new Size(48, 15);
            lblSelectedItemID.TabIndex = 2;
            lblSelectedItemID.Text = "Item ID:";
            // 
            // lblSelectedItemName
            // 
            lblSelectedItemName.AutoSize = true;
            lblSelectedItemName.Location = new Point(231, 80);
            lblSelectedItemName.Name = "lblSelectedItemName";
            lblSelectedItemName.Size = new Size(42, 15);
            lblSelectedItemName.TabIndex = 4;
            lblSelectedItemName.Text = "Name:";
            // 
            // txtSelectedItemID
            // 
            txtSelectedItemID.Location = new Point(301, 42);
            txtSelectedItemID.Name = "txtSelectedItemID";
            txtSelectedItemID.ReadOnly = true;
            txtSelectedItemID.Size = new Size(120, 23);
            txtSelectedItemID.TabIndex = 3;
            // 
            // txtSelectedItemName
            // 
            txtSelectedItemName.Location = new Point(301, 77);
            txtSelectedItemName.Name = "txtSelectedItemName";
            txtSelectedItemName.ReadOnly = true;
            txtSelectedItemName.Size = new Size(120, 23);
            txtSelectedItemName.TabIndex = 5;
            // 
            // NewItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(437, 326);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(dataItemList);
            Controls.Add(lblSelectedItemID);
            Controls.Add(txtSelectedItemID);
            Controls.Add(lblSelectedItemName);
            Controls.Add(txtSelectedItemName);
            Controls.Add(lblGold);
            Controls.Add(txtGold);
            Controls.Add(lblEP);
            Controls.Add(txtEP);
            Controls.Add(lblCategory);
            Controls.Add(cbCategory);
            Controls.Add(lblDesc);
            Controls.Add(txtDesc);
            Controls.Add(lblHot);
            Controls.Add(chkHot);
            Controls.Add(lblNew);
            Controls.Add(chkNew);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewItem";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add New Item";
            ((System.ComponentModel.ISupportInitialize)dataItemList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
