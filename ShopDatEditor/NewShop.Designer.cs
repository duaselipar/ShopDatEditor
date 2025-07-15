namespace ShopDatEditor
{
    partial class NewShop
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
        private System.Windows.Forms.Label lblShopID;
        private System.Windows.Forms.Label lblShopName;
        private System.Windows.Forms.Label lblCurrencyType;
        private System.Windows.Forms.TextBox txtShopID;
        private System.Windows.Forms.TextBox txtShopName;
        private System.Windows.Forms.ComboBox cbCurrencyType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;

        private void InitializeComponent()
        {
            lblShopID = new Label();
            lblShopName = new Label();
            lblCurrencyType = new Label();
            txtShopID = new TextBox();
            txtShopName = new TextBox();
            cbCurrencyType = new ComboBox();
            btnOK = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblShopID
            // 
            lblShopID.AutoSize = true;
            lblShopID.Location = new Point(38, 15);
            lblShopID.Name = "lblShopID";
            lblShopID.Size = new Size(54, 15);
            lblShopID.TabIndex = 0;
            lblShopID.Text = "Shop ID :";
            // 
            // lblShopName
            // 
            lblShopName.AutoSize = true;
            lblShopName.Location = new Point(17, 50);
            lblShopName.Name = "lblShopName";
            lblShopName.Size = new Size(75, 15);
            lblShopName.TabIndex = 2;
            lblShopName.Text = "Shop Name :";
            // 
            // lblCurrencyType
            // 
            lblCurrencyType.AutoSize = true;
            lblCurrencyType.Location = new Point(3, 85);
            lblCurrencyType.Name = "lblCurrencyType";
            lblCurrencyType.Size = new Size(89, 15);
            lblCurrencyType.TabIndex = 4;
            lblCurrencyType.Text = "Currency Type :";
            // 
            // txtShopID
            // 
            txtShopID.Location = new Point(95, 12);
            txtShopID.Name = "txtShopID";
            txtShopID.Size = new Size(120, 23);
            txtShopID.TabIndex = 1;
            // 
            // txtShopName
            // 
            txtShopName.Location = new Point(95, 47);
            txtShopName.MaxLength = 15;
            txtShopName.Name = "txtShopName";
            txtShopName.Size = new Size(120, 23);
            txtShopName.TabIndex = 3;
            // 
            // cbCurrencyType
            // 
            cbCurrencyType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCurrencyType.Items.AddRange(new object[] { "Gold Shop", "EP Shop" });
            cbCurrencyType.Location = new Point(95, 82);
            cbCurrencyType.Name = "cbCurrencyType";
            cbCurrencyType.Size = new Size(120, 23);
            cbCurrencyType.TabIndex = 5;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(17, 125);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(80, 28);
            btnOK.TabIndex = 6;
            btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(122, 125);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 28);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            // 
            // NewShop
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(228, 166);
            Controls.Add(lblShopID);
            Controls.Add(txtShopID);
            Controls.Add(lblShopName);
            Controls.Add(txtShopName);
            Controls.Add(lblCurrencyType);
            Controls.Add(cbCurrencyType);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewShop";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add New Shop";
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion
    }
}