using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ShopDatEditor
{
    public partial class NewItem : Form
    {
        public int ItemID { get; private set; }
        public string ItemName { get; private set; }
        public int Gold { get; private set; }
        public int EP { get; private set; }
        public string Category { get; private set; }
        public string Desc { get; private set; }
        public bool IsHot { get; private set; }
        public bool IsNew { get; private set; }

        private DataTable dtItems = new DataTable();
        private bool isVIPShop;
        private int shopCurrency; // 0=Gold, 5=EP

        // Constructor: pass list itemtype, isVIPShop flag, currency type
        public NewItem(List<ItemInfo> itemList, bool isVIPShop, int shopCurrency)
        {
            InitializeComponent();
            this.isVIPShop = isVIPShop;
            this.shopCurrency = shopCurrency;

            // Setup datagrid: tunjuk ID & Name je
            dtItems.Columns.Add("ID", typeof(int));
            dtItems.Columns.Add("Name", typeof(string));
            foreach (var it in itemList)
                dtItems.Rows.Add(it.ID, it.Name);
            dataItemList.DataSource = dtItems;
            dataItemList.Columns["ID"].Width = 60;
            dataItemList.Columns["Name"].Width = 120;
            dataItemList.Columns["ID"].ReadOnly = true;
            dataItemList.Columns["Name"].ReadOnly = true;

            // disable sort
            foreach (DataGridViewColumn col in dataItemList.Columns) col.SortMode = DataGridViewColumnSortMode.NotSortable;

            // event pilih row
            dataItemList.SelectionChanged += DataItemList_SelectionChanged;

            // cari bila tekan enter/search btn
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoSearch(); };
            btnSearch.Click += (s, e) => DoSearch();

            if (isVIPShop)
            {
                txtGold.Enabled = false;
                txtEP.Enabled = true;

                cbCategory.Enabled = chkHot.Enabled = chkNew.Enabled = true;
                cbCategory.SelectedItem = "item"; // default
                txtDesc.Enabled = false;
                txtDesc.Text = "null";
                // Enable desc hanya bila recommend
                cbCategory.SelectedIndexChanged += (s, e) =>
                {
                    if (cbCategory.Text == "recommend")
                    {
                        txtDesc.Enabled = true;
                        txtDesc.Text = "This is recommend item!";
                    }
                    else
                    {
                        txtDesc.Text = "null";
                        txtDesc.Enabled = false;
                    }
                };

            }
            else
            {
                // Biasa
                cbCategory.Enabled = txtDesc.Enabled = chkHot.Enabled = chkNew.Enabled = false;
                txtDesc.Text = "null";

                if (shopCurrency == 5) // EP Shop
                {
                    txtGold.Enabled = false;
                    txtEP.Enabled = true;
                }
                else // Gold Shop
                {
                    txtGold.Enabled = true;
                    txtEP.Enabled = false;
                }
            }

            // auto tick 1 sahaja (VIP shop)
            chkHot.CheckedChanged += (s, e) => { if (chkHot.Checked) chkNew.Checked = false; };
            chkNew.CheckedChanged += (s, e) => { if (chkNew.Checked) chkHot.Checked = false; };

            btnOK.Click += BtnOK_Click;
        }


        // Bila pilih item grid
        private void DataItemList_SelectionChanged(object sender, EventArgs e)
        {
            if (dataItemList.SelectedRows.Count == 0) return;
            var row = dataItemList.SelectedRows[0];
            txtSelectedItemID.Text = row.Cells["ID"].Value.ToString();
            txtSelectedItemName.Text = row.Cells["Name"].Value.ToString();
        }

        // Search logic
        // Letak variable untuk simpan last search index
        private int lastSearchRow = -1;

        private void DoSearch()
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword)) return;

            int startRow = lastSearchRow + 1;
            bool found = false;

            // Cari dari row selepas current
            for (int i = startRow; i < dataItemList.Rows.Count; i++)
            {
                var row = dataItemList.Rows[i];
                if (row.Cells["ID"].Value.ToString().Equals(keyword, StringComparison.OrdinalIgnoreCase) ||
                    row.Cells["Name"].Value.ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dataItemList.CurrentCell = row.Cells[0];
                    dataItemList.FirstDisplayedScrollingRowIndex = row.Index;
                    lastSearchRow = i;
                    found = true;
                    return;
                }
            }
            // Jika tak jumpa, ulang dari atas (circular search)
            for (int i = 0; i < startRow; i++)
            {
                var row = dataItemList.Rows[i];
                if (row.Cells["ID"].Value.ToString().Equals(keyword, StringComparison.OrdinalIgnoreCase) ||
                    row.Cells["Name"].Value.ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Selected = true;
                    dataItemList.CurrentCell = row.Cells[0];
                    dataItemList.FirstDisplayedScrollingRowIndex = row.Index;
                    lastSearchRow = i;
                    found = true;
                    return;
                }
            }
            // Tak jumpa langsung
            MessageBox.Show("Item not found!");
            lastSearchRow = -1;
        }

        // Reset lastSearchRow bila tukar keyword
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lastSearchRow = -1;
        }


        // Button OK click
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSelectedItemID.Text))
            {
                MessageBox.Show("Please select an item!");
                return;
            }
            ItemID = int.Parse(txtSelectedItemID.Text);
            ItemName = txtSelectedItemName.Text;

            if (isVIPShop)
            {
                // EP mesti diisi
                if (string.IsNullOrWhiteSpace(txtEP.Text) || !int.TryParse(txtEP.Text.Trim(), out var ep) || ep < 0)
                {
                    MessageBox.Show("EP must be filled (and must be a number)!");
                    txtEP.Focus();
                    return;
                }
                EP = ep;

                Category = cbCategory.Text;
                if (Category == "recommend")
                {
                    if (string.IsNullOrWhiteSpace(txtDesc.Text))
                    {
                        MessageBox.Show("Description is required for recommend category.");
                        txtDesc.Focus();
                        return;
                    }
                    Desc = txtDesc.Text;
                }
                else
                {
                    Desc = "null";
                    txtDesc.Text = "null";
                }
                IsHot = chkHot.Checked;
                IsNew = chkNew.Checked;
            }
            else
            {
                // Shop biasa: ikut currency
                if (shopCurrency == 5)
                {
                    // EP mesti diisi
                    if (string.IsNullOrWhiteSpace(txtEP.Text) || !int.TryParse(txtEP.Text.Trim(), out var ep) || ep < 0)
                    {
                        MessageBox.Show("EP must be filled (and must be a number)!");
                        txtEP.Focus();
                        return;
                    }
                    EP = ep;
                    Gold = 0;
                }
                else
                {
                    // Gold mesti diisi
                    if (string.IsNullOrWhiteSpace(txtGold.Text) || !int.TryParse(txtGold.Text.Trim(), out var gold) || gold < 0)
                    {
                        MessageBox.Show("Gold must be filled (and must be a number)!");
                        txtGold.Focus();
                        return;
                    }
                    Gold = gold;
                    EP = 0;
                }
                Desc = "null";
                Category = null;
                IsHot = false;
                IsNew = false;
                txtDesc.Text = "null";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


    }

    // Kelas simple untuk list item
    public class ItemInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
