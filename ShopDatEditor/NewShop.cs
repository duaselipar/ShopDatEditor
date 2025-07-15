using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ShopDatEditor
{
    public partial class NewShop : Form
    {
        // Senarai ID sedia ada (wajib pass in dari Form1)
        private HashSet<int> existingShopIDs;

        public int ShopID { get; private set; }
        public string ShopName { get; private set; }
        public int ShopType { get; private set; } // 0 = Gold, 5 = EP

        public NewShop(HashSet<int> existingShopIDs)
        {
            InitializeComponent();
            this.existingShopIDs = existingShopIDs;

            // --- Set default value ---
            txtShopID.Text = "1000";
            txtShopName.Text = "NewShop";

            // Initialize non-nullable properties with default values
            ShopName = "NewShop";
            ShopID = 1000;
            ShopType = 0;

            // Allow only numbers in ShopID textbox
            txtShopID.KeyPress += TxtShopID_KeyPress;
            btnOK.Click += BtnOK_Click;
        }

        private void TxtShopID_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Only allow digits and control (e.g. backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cbCurrencyType.SelectedIndex = 0;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            // Validate Shop ID
            if (!int.TryParse(txtShopID.Text.Trim(), out int id))
            {
                MessageBox.Show("Shop ID must be a number.");
                txtShopID.Focus();
                return;
            }
            if (id < 1000 || id > 9999)
            {
                MessageBox.Show("Shop ID must be between 1000 and 9999.");
                txtShopID.Focus();
                return;
            }
            if (existingShopIDs.Contains(id))
            {
                MessageBox.Show("This Shop ID already exists. Please use a different ID.");
                txtShopID.Focus();
                return;
            }

            // Validate Shop Name
            string name = txtShopName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Shop Name cannot be empty.");
                txtShopName.Focus();
                return;
            }
            if (name.Length > 15)
            {
                MessageBox.Show("Shop Name too long (max 15 char).");
                txtShopName.Focus();
                return;
            }

            ShopID = id;
            ShopName = name;
            ShopType = (cbCurrencyType.SelectedIndex == 1) ? 5 : 0; // 0=Gold, 5=EP

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
