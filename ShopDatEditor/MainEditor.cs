using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ShopDatEditor
{
    public partial class MainEditor : Form
    {
        MySqlConnection conn;
        bool isConnected = false;
        string lastShopPath = "", lastItemtypePath = "";

        List<ShopEntry> shopList = new List<ShopEntry>();
        byte[] shopDatBytes = Array.Empty<byte>(); // Initialize to an empty array
        byte[] itemtypeDatBytes = Array.Empty<byte>(); // Initialize to an empty array
        int currentShopIndex = -1;
        Dictionary<int, int> itemIdToIndex = new Dictionary<int, int>();
        ContextMenuStrip itemMenu = new ContextMenuStrip();
        ContextMenuStrip shopMenu = new ContextMenuStrip();

        public MainEditor()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            dataItem.EditMode = DataGridViewEditMode.EditOnEnter;


            dataShop.MultiSelect = false; // <-- Tambah sini

            dataShop.RowHeadersVisible = false;
            dataItem.RowHeadersVisible = false;
            dataShop.AllowUserToResizeRows = false;
            dataShop.AllowUserToResizeColumns = false;
            dataItem.AllowUserToResizeRows = false;
            dataItem.AllowUserToResizeColumns = false;

            InitGrid(0);
            dataShop.SelectionChanged += dataShop_SelectionChanged;
            dataShop.CellEndEdit += dataShop_CellEndEdit;
            dataItem.CellEndEdit += dataItem_CellEndEdit;
            dataItem.UserDeletingRow += dataItem_UserDeletingRow;

            // Setup context menu
            itemMenu.Items.Add("Move to Top", null, ItemMoveTop_Click);
            itemMenu.Items.Add("Move to Bottom", null, ItemMoveBottom_Click);
            itemMenu.Items.Add("Delete", null, ItemDelete_Click);
            dataItem.ContextMenuStrip = itemMenu;

            // Event supaya klik kanan pilih row
            dataItem.MouseDown += DataItem_MouseDown;


            dataItem.AllowDrop = true;
            dataItem.MouseDown += DataItem_MouseDown_Drag;
            dataItem.MouseMove += DataItem_MouseMove_Drag;
            dataItem.DragOver += DataItem_DragOver;
            dataItem.DragDrop += DataItem_DragDrop;

            // Tick hanya satu (Hot atau New sahaja!)
            dataItem.CellValueChanged += DataItem_CellValueChanged_HotNew;
            dataItem.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataItem.IsCurrentCellDirty)
                    dataItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            dataShop.EditingControlShowing += DataShop_EditingControlShowing;
            dataItem.EditingControlShowing += DataItem_EditingControlShowing;

            dataItem.KeyDown += DataItem_KeyDown;

            shopMenu.Items.Add("Delete Shop", null, ShopDelete_Click);
            dataShop.ContextMenuStrip = shopMenu;

            // Right click will auto-select row
            dataShop.MouseDown += DataShop_MouseDown;

            // Enable Delete key shortcut
            dataShop.KeyDown += DataShop_KeyDown;

            txtHostname.Text = "localhost";
            txtPort.Text = "3306";
            txtUser.Text = "root";
            txtPass.Text = "test";
            txtDb.Text = "classicdb";

            SetButtonsEnabled(false);
            btnSelect.Enabled = true;
            btnLoad.Enabled = true;
        }

        int dragRowIndex = -1;


        private void SetButtonsEnabled(bool enabled)
        {
            btnNewshop.Enabled = enabled;
            btnNewitem.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnDbconnect.Enabled = enabled;
            btnNameupdate.Enabled = enabled;
            btnUpdateshop.Enabled = enabled;
        }
        private void InitGrid(int type)
        {
            dataItem.Columns.Clear();
            if (type == 1)
            {
                dataItem.Columns.Add("ItemID", "ItemID");
                dataItem.Columns.Add("Name", "Name");
                dataItem.Columns.Add("Stone", "Stone");
                dataItem.Columns.Add("Desc", "Desc");

                // Category ComboBox (egg, item, equipment, recommend)
                var catCol = new DataGridViewComboBoxColumn();
                catCol.Name = "Category";
                catCol.HeaderText = "Category";
                catCol.Items.AddRange("egg", "item", "equipment", "recommend");
                dataItem.Columns.Add(catCol);

                // Checkbox Hot/New
                var hotCol = new DataGridViewCheckBoxColumn();
                hotCol.Name = "Hot";
                hotCol.HeaderText = "Hot";
                dataItem.Columns.Add(hotCol);

                var newCol = new DataGridViewCheckBoxColumn();
                newCol.Name = "New";
                newCol.HeaderText = "New";
                dataItem.Columns.Add(newCol);

                dataItem.Columns["ItemID"].ReadOnly = dataItem.Columns["Name"].ReadOnly = false;
                dataItem.Columns["Stone"].ReadOnly = false;
                dataItem.Columns["Name"].Width = 140; // <-- Lebar grid nama item

                dataItem.Columns["ItemID"].ReadOnly = true;
                dataItem.Columns["Name"].ReadOnly = true;

            }
            else if (type == 2)
            {
                dataItem.Columns.Add("ItemID", "ItemID");
                dataItem.Columns.Add("Name", "Name");
                dataItem.Columns.Add("Gold", "Gold");
                dataItem.Columns.Add("EP", "EP"); // <-- tukar sini
                dataItem.Columns["ItemID"].ReadOnly = dataItem.Columns["Name"].ReadOnly = false;
                dataItem.Columns["Gold"].ReadOnly = false;
                dataItem.Columns["EP"].ReadOnly = false; // <-- tukar sini
                dataItem.Columns["Name"].Width = 140; // <-- Lebar grid nama item

                dataItem.Columns["ItemID"].ReadOnly = true;
                dataItem.Columns["Name"].ReadOnly = true;

            }

            foreach (DataGridViewColumn col in dataItem.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

        }


        private byte[] GetShopNameBytes(string name)
        {
            byte[] b = Encoding.GetEncoding("GB2312").GetBytes(name ?? "");
            var ret = new byte[16];
            Array.Copy(b, ret, Math.Min(b.Length, 16));
            return ret;
        }

        private string GetShopNameFromBytes(byte[] bytes, int offset)
        {
            var str = Encoding.GetEncoding("GB2312").GetString(bytes, offset, 16);
            int nullIdx = str.IndexOf('\0');
            if (nullIdx >= 0) str = str.Substring(0, nullIdx);
            return str.TrimEnd('?');
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string clientPath = txtClientpath.Text.Trim();
            if (string.IsNullOrWhiteSpace(clientPath) || !Directory.Exists(clientPath))
            {
                MessageBox.Show("Please select the client folder first.");
                return;
            }
            string iniFolder = Path.Combine(clientPath, "ini");
            string shopPath = Path.Combine(iniFolder, "Shop.dat");
            string itemtypePath = Path.Combine(iniFolder, "itemtype.dat");

            if (!File.Exists(shopPath) || !File.Exists(itemtypePath))
            {
                MessageBox.Show("Shop.dat or itemtype.dat file not found in ini folder.");
                return;
            }

            // Simpan path untuk kegunaan btnSave
            lastShopPath = shopPath;
            lastItemtypePath = itemtypePath;

            shopDatBytes = File.ReadAllBytes(shopPath);
            itemtypeDatBytes = File.ReadAllBytes(itemtypePath);
            shopList.Clear(); itemIdToIndex.Clear();

            int count = BitConverter.ToInt32(itemtypeDatBytes, 0);
            int recordLen = 244, recordTableOffset = itemtypeDatBytes.Length - count * recordLen;
            for (int i = 0; i < count; i++)
            {
                int id = BitConverter.ToInt32(itemtypeDatBytes, 4 + i * 4);
                itemIdToIndex[id] = i;
            }

            int idx = 4, shopCount = BitConverter.ToInt32(shopDatBytes, 0);
            while (idx < shopDatBytes.Length)
            {
                int shopID = BitConverter.ToInt32(shopDatBytes, idx);
                string shopName = GetShopNameFromBytes(shopDatBytes, idx + 4);
                int shopType = BitConverter.ToInt32(shopDatBytes, idx + 20);
                int itemCount = BitConverter.ToInt32(shopDatBytes, idx + 24);
                var shopEntry = new ShopEntry { ShopID = shopID, ShopName = shopName, Offset = idx, ShopType = shopType };

                if (shopID == 1207)
                {
                    for (int i = 0, b = idx + 28; i < itemCount; i++, b += 452)
                    {
                        int itemID = BitConverter.ToInt32(shopDatBytes, b);
                        int stone = 0;
                        if (itemIdToIndex.TryGetValue(itemID, out int itIdx))
                        {
                            int dataOffset = recordTableOffset + itIdx * recordLen;
                            stone = BitConverter.ToInt32(itemtypeDatBytes, dataOffset + 84);
                        }
                        shopEntry.Items.Add(new ShopItem
                        {
                            ItemID = itemID,
                            Stone = stone,
                            Category = Encoding.GetEncoding("GB2312").GetString(shopDatBytes, b + 4, 64).Split('\0')[0],
                            Hot = Encoding.GetEncoding("GB2312").GetString(shopDatBytes, b + 68, 64).Split('\0')[0],
                            New = Encoding.GetEncoding("GB2312").GetString(shopDatBytes, b + 132, 64).Split('\0')[0],
                            Desc = Encoding.GetEncoding("GB2312").GetString(shopDatBytes, b + 196, 128).Split('\0')[0]
                        });
                    }
                    idx += 28 + itemCount * 452;
                }
                else
                {
                    for (int i = 0, b = idx + 28; i < itemCount; i++, b += 4)
                    {
                        int itemID = BitConverter.ToInt32(shopDatBytes, b);
                        int gold = 0, stone = 0;
                        if (itemIdToIndex.TryGetValue(itemID, out int itIdx))
                        {
                            int dataOffset = recordTableOffset + itIdx * recordLen;
                            gold = BitConverter.ToInt32(itemtypeDatBytes, dataOffset + 36);
                            stone = BitConverter.ToInt32(itemtypeDatBytes, dataOffset + 84);
                        }
                        shopEntry.Items.Add(new ShopItem { ItemID = itemID, Gold = gold, Stone = stone });
                    }
                    idx += 28 + itemCount * 4;
                }
                shopList.Add(shopEntry);
            }
            shopList.Sort((a, b) => a.ShopID.CompareTo(b.ShopID));

            dataShop.Rows.Clear();
            dataShop.Columns.Clear();
            dataShop.Columns.Add("ShopID", "ID");
            dataShop.Columns.Add("ShopName", "Name");
            dataShop.Columns.Add("ItemCount", "ItemCount");
            dataShop.Columns["ItemCount"].ReadOnly = true;

            // Add currency type column (ComboBox)
            var currencyCol = new DataGridViewComboBoxColumn();
            currencyCol.Name = "CurrencyType";
            currencyCol.HeaderText = "Currency";
            currencyCol.Items.AddRange("Gold Shop", "EP Shop");
            dataShop.Columns.Add(currencyCol);
            currencyCol.DisplayIndex = 3;

            foreach (DataGridViewColumn col in dataShop.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (var s in shopList)
            {
                string currency = (s.ShopType == 5) ? "EP Shop" : "Gold Shop";
                dataShop.Rows.Add(s.ShopID, s.ShopName, s.Items.Count, currency);
            }

            SetButtonsEnabled(true);

            dataShop.ClearSelection();
            dataItem.Rows.Clear();
            InitGrid(0);
            if (dataShop.Rows.Count > 0)
            {
                dataShop.Rows[0].Selected = true;
                dataShop.CurrentCell = dataShop.Rows[0].Cells[0];
            }

            UpdateFormTitle();

        }




        private string GetItemName(int itemID)
        {
            if (itemtypeDatBytes == null || itemtypeDatBytes.Length < 8) return "";
            int count = BitConverter.ToInt32(itemtypeDatBytes, 0);
            int idTableOffset = 4, recordLen = 244, recordTableOffset = itemtypeDatBytes.Length - count * recordLen;
            for (int i = 0; i < count; i++)
                if (BitConverter.ToInt32(itemtypeDatBytes, idTableOffset + i * 4) == itemID)
                    return Encoding.GetEncoding("GB2312").GetString(itemtypeDatBytes, recordTableOffset + i * recordLen + 4, 16).Split('\0')[0];
            return "";
        }

        private void dataShop_SelectionChanged(object? sender, EventArgs? e)
        {
            if (dataShop.SelectedRows.Count == 0) return;
            int idx = dataShop.SelectedRows[0].Index; if (idx < 0 || idx >= shopList.Count) return;
            currentShopIndex = idx; var shop = shopList[idx];
            dataItem.Rows.Clear(); InitGrid(shop.ShopID == 1207 ? 1 : 2);

            foreach (var item in shop.Items)
            {
                string name = GetItemName(item.ItemID);
                if (shop.ShopID == 1207)
                {
                    bool isHot = !string.IsNullOrEmpty(item.Hot) && item.Hot.ToLower() == "hot";
                    bool isNew = !string.IsNullOrEmpty(item.New) && item.New.ToLower() == "new";
                    dataItem.Rows.Add(item.ItemID, name, item.Stone, item.Desc, item.Category, isHot, isNew);
                }
                else
                {
                    dataItem.Rows.Add(item.ItemID, name, item.Gold, item.Stone);
                }
            }
        }

        private void dataShop_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= shopList.Count) return;
            if (e.ColumnIndex == dataShop.Columns["ShopID"].Index)
            {
                int newID;
                if (int.TryParse(dataShop.Rows[e.RowIndex].Cells["ShopID"].Value?.ToString(), out newID))
                    shopList[e.RowIndex].ShopID = newID;
            }
            else if (e.ColumnIndex == dataShop.Columns["ShopName"].Index)
            {
                shopList[e.RowIndex].ShopName = dataShop.Rows[e.RowIndex].Cells["ShopName"].Value?.ToString() ?? "";
            }
            else if (e.ColumnIndex == dataShop.Columns["CurrencyType"].Index)
            {
                string? val = dataShop.Rows[e.RowIndex].Cells["CurrencyType"].Value?.ToString();
                shopList[e.RowIndex].ShopType = (val == "EP Shop") ? 5 : 0;
            }
        }

        private void dataItem_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (currentShopIndex < 0 || currentShopIndex >= shopList.Count) return;
            var shop = shopList[currentShopIndex];
            int row = e.RowIndex;
            if (row < 0 || row >= shop.Items.Count) return;
            var item = shop.Items[row];

            if (shop.ShopID == 1207)
            {
                int stone = 0;
                int.TryParse(dataItem.Rows[row].Cells["Stone"].Value?.ToString(), out stone);

                bool isHot = false, isNew = false;
                if (dataItem.Rows[row].Cells["Hot"].Value != null)
                    bool.TryParse(dataItem.Rows[row].Cells["Hot"].Value.ToString(), out isHot);
                if (dataItem.Rows[row].Cells["New"].Value != null)
                    bool.TryParse(dataItem.Rows[row].Cells["New"].Value.ToString(), out isNew);

                item.Hot = isHot ? "hot" : "null";
                item.New = isNew ? "new" : "null";

                item.Stone = stone;
                item.Desc = (dataItem.Rows[row].Cells["Desc"].Value?.ToString() ?? string.Empty);
                item.Category = (dataItem.Rows[row].Cells["Category"].Value?.ToString() ?? string.Empty);
            }
            else
            {
                int gold = 0, ep = 0;
                int.TryParse(dataItem.Rows[row].Cells["Gold"].Value?.ToString(), out gold);
                int.TryParse(dataItem.Rows[row].Cells["EP"].Value?.ToString(), out ep);

                foreach (var sh in shopList)
                    foreach (var it in sh.Items)
                        if (it.ItemID == item.ItemID)
                        {
                            it.Gold = gold;
                            it.Stone = ep; // Kekal Stone untuk field backend
                        }
            }
        }

        private void dataItem_UserDeletingRow(object? sender, DataGridViewRowCancelEventArgs e)
        {
            if (currentShopIndex < 0 || currentShopIndex >= shopList.Count) return;
            var shop = shopList[currentShopIndex];
            if (shop == null) return; // Ensure shop is not null
            int rowIdx = e.Row?.Index ?? -1; // Safely handle null reference
            if (rowIdx < 0 || rowIdx >= shop.Items.Count) return;
            shop.Items.RemoveAt(rowIdx);
            dataShop.Rows[currentShopIndex].Cells["ItemCount"].Value = shop.Items.Count;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // --- Update itemtype.dat ikut SEMUA shop ---
            int count = BitConverter.ToInt32(itemtypeDatBytes, 0);
            int recordLen = 244, recordTableOffset = itemtypeDatBytes.Length - count * recordLen;
            var itemIdDict = BuildItemIdIndexDict();

            foreach (var shop in shopList)
            {
                foreach (var item in shop.Items)
                {
                    if (itemIdDict.TryGetValue(item.ItemID, out int idx))
                    {
                        int dataOffset = recordTableOffset + idx * recordLen;
                        if (shop.ShopID == 1207)
                        {
                            BitConverter.GetBytes(item.Stone).CopyTo(itemtypeDatBytes, dataOffset + 84);
                        }
                        else
                        {
                            BitConverter.GetBytes(item.Gold).CopyTo(itemtypeDatBytes, dataOffset + 36);
                            BitConverter.GetBytes(item.Stone).CopyTo(itemtypeDatBytes, dataOffset + 84);
                        }
                    }
                }
            }
            // --- SIMPAN IKUT PATH YANG DI-LOAD TADI ---
            File.WriteAllBytes(lastItemtypePath, itemtypeDatBytes);

            shopList.Sort((a, b) => a.ShopID.CompareTo(b.ShopID));
            using (var fs = new FileStream(lastShopPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(BitConverter.GetBytes(shopList.Count), 0, 4);
                foreach (var shop in shopList)
                {
                    fs.Write(BitConverter.GetBytes(shop.ShopID), 0, 4);
                    fs.Write(GetShopNameBytes(shop.ShopName), 0, 16);
                    fs.Write(BitConverter.GetBytes(shop.ShopType), 0, 4);
                    fs.Write(BitConverter.GetBytes(shop.Items.Count), 0, 4);

                    if (shop.ShopID == 1207)
                    {
                        foreach (var item in shop.Items)
                        {
                            byte[] buf = new byte[452];
                            BitConverter.GetBytes(item.ItemID).CopyTo(buf, 0);

                            byte[] catBytes = new byte[64];
                            Encoding.GetEncoding("GB2312").GetBytes(item.Category ?? "", 0, Math.Min((item.Category ?? "").Length, 64), catBytes, 0);
                            catBytes.CopyTo(buf, 4);

                            byte[] hotBytes = new byte[64];
                            Encoding.GetEncoding("GB2312").GetBytes(item.Hot ?? "", 0, Math.Min((item.Hot ?? "").Length, 64), hotBytes, 0);
                            hotBytes.CopyTo(buf, 68);

                            byte[] newBytes = new byte[64];
                            Encoding.GetEncoding("GB2312").GetBytes(item.New ?? "", 0, Math.Min((item.New ?? "").Length, 64), newBytes, 0);
                            newBytes.CopyTo(buf, 132);

                            byte[] descBytes = new byte[128];
                            Encoding.GetEncoding("GB2312").GetBytes(item.Desc ?? "", 0, Math.Min((item.Desc ?? "").Length, 128), descBytes, 0);
                            descBytes.CopyTo(buf, 196);

                            fs.Write(buf, 0, 452);
                        }
                    }
                    else
                    {
                        foreach (var item in shop.Items)
                        {
                            fs.Write(BitConverter.GetBytes(item.ItemID), 0, 4);
                        }
                    }
                }
            }
            MessageBox.Show("Successfully saved Shop.dat & itemtype.dat");
        }




        private Dictionary<int, int> BuildItemIdIndexDict()
        {
            int count = BitConverter.ToInt32(itemtypeDatBytes, 0);
            var dict = new Dictionary<int, int>();
            for (int i = 0; i < count; i++)
            {
                int id = BitConverter.ToInt32(itemtypeDatBytes, 4 + i * 4);
                dict[id] = i;
            }
            return dict;
        }


        private void DataItem_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataItem.HitTest(e.X, e.Y);
                if (hti.RowIndex >= 0)
                {
                    dataItem.ClearSelection();
                    dataItem.Rows[hti.RowIndex].Selected = true;
                    dataItem.CurrentCell = dataItem.Rows[hti.RowIndex].Cells[0];
                }
            }
        }

        // Delete
        private void ItemDelete_Click(object? sender, EventArgs e)
        {
            if (dataItem.SelectedRows.Count == 0) return;
            int row = dataItem.SelectedRows[0].Index;
            if (row >= 0 && row < shopList[currentShopIndex].Items.Count)
            {
                shopList[currentShopIndex].Items.RemoveAt(row);
                dataItem.Rows.RemoveAt(row);
                dataShop.Rows[currentShopIndex].Cells["ItemCount"].Value = shopList[currentShopIndex].Items.Count;
            }

            UpdateFormTitle();
        }

        // Move to Top
        private void ItemMoveTop_Click(object? sender, EventArgs e)
        {
            if (dataItem.SelectedRows.Count == 0) return;
            int row = dataItem.SelectedRows[0].Index;
            if (row > 0)
            {
                var item = shopList[currentShopIndex].Items[row];
                shopList[currentShopIndex].Items.RemoveAt(row);
                shopList[currentShopIndex].Items.Insert(0, item);
                dataShop_SelectionChanged(null, null); // Refresh view
                dataItem.Rows[0].Selected = true;
            }
        }

        // Move to Bottom
        private void ItemMoveBottom_Click(object? sender, EventArgs e)
        {
            if (dataItem.SelectedRows.Count == 0) return;
            int row = dataItem.SelectedRows[0].Index;
            var list = shopList[currentShopIndex].Items;
            if (row < list.Count - 1)
            {
                var item = list[row];
                list.RemoveAt(row);
                list.Add(item);
                dataShop_SelectionChanged(null, null); // Refresh view
                dataItem.Rows[list.Count - 1].Selected = true;
            }
        }

        private void DataItem_MouseDown_Drag(object? sender, MouseEventArgs e)
        {
            var ht = dataItem.HitTest(e.X, e.Y);
            dragRowIndex = -1;

            // Hanya boleh drag kalau klik column "ItemID" atau "Name"
            if (ht.RowIndex >= 0 && (ht.ColumnIndex == dataItem.Columns["ItemID"].Index || ht.ColumnIndex == dataItem.Columns["Name"].Index))
            {
                dragRowIndex = ht.RowIndex;
            }
        }

        private void DataItem_MouseMove_Drag(object? sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && dragRowIndex >= 0)
            {
                dataItem.DoDragDrop(dataItem.Rows[dragRowIndex], DragDropEffects.Move);
            }
        }

        private void DataItem_DragOver(object? sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void DataItem_DragDrop(object? sender, DragEventArgs e)
        {
            // Simpan scroll index sekarang
            int scrollIndex = dataItem.FirstDisplayedScrollingRowIndex;
            Point clientPoint = dataItem.PointToClient(new Point(e.X, e.Y));
            int targetRowIndex = dataItem.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (dragRowIndex >= 0 && targetRowIndex >= 0 && dragRowIndex != targetRowIndex)
            {
                // Tukar dalam list
                var items = shopList[currentShopIndex].Items;
                var movedItem = items[dragRowIndex];
                items.RemoveAt(dragRowIndex);
                items.Insert(targetRowIndex, movedItem);

                // Refresh grid
                dataShop_SelectionChanged(null, null);

                // **Restore scroll ke posisi asal**
                if (scrollIndex >= 0 && scrollIndex < dataItem.Rows.Count)
                    dataItem.FirstDisplayedScrollingRowIndex = scrollIndex;

                // Select baris yang baru (optional)
                if (targetRowIndex >= 0 && targetRowIndex < dataItem.Rows.Count)
                {
                    dataItem.ClearSelection();
                    dataItem.Rows[targetRowIndex].Selected = true;
                }
            }
            dragRowIndex = -1;
        }

        private void DataItem_CellValueChanged_HotNew(object? sender, DataGridViewCellEventArgs e)
        {
            if (currentShopIndex < 0) return;
            if (dataItem.Columns[e.ColumnIndex].Name == "Hot" && e.RowIndex >= 0)
            {
                bool isHot = false;
                if (dataItem.Rows[e.RowIndex].Cells["Hot"].Value != null)
                    bool.TryParse(dataItem.Rows[e.RowIndex].Cells["Hot"].Value.ToString(), out isHot);

                if (isHot)
                {
                    // Jika tick Hot, untick New
                    dataItem.Rows[e.RowIndex].Cells["New"].Value = false;
                }
            }
            else if (dataItem.Columns[e.ColumnIndex].Name == "New" && e.RowIndex >= 0)
            {
                bool isNew = false;
                if (dataItem.Rows[e.RowIndex].Cells["New"].Value != null)
                    bool.TryParse(dataItem.Rows[e.RowIndex].Cells["New"].Value.ToString(), out isNew);

                if (isNew)
                {
                    // Jika tick New, untick Hot
                    dataItem.Rows[e.RowIndex].Cells["Hot"].Value = false;
                }
            }
        }

        private void DataShop_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataShop.CurrentCell.ColumnIndex == dataShop.Columns["ShopName"].Index)
            {
                var tb = e.Control as TextBox;
                if (tb != null) tb.MaxLength = 15;
            }
        }

        // Untuk desc di VIP shop
        private void DataItem_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (currentShopIndex >= 0 && shopList[currentShopIndex].ShopID == 1207 &&
                dataItem.CurrentCell.ColumnIndex == dataItem.Columns["Desc"].Index)
            {
                var tb = e.Control as TextBox;
                if (tb != null) tb.MaxLength = 127;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Pilih folder client EO anda";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtClientpath.Text = dlg.SelectedPath;
                }
            }
        }


        private void DataShop_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataShop.HitTest(e.X, e.Y);
                if (hti.RowIndex >= 0)
                {
                    dataShop.ClearSelection();
                    dataShop.Rows[hti.RowIndex].Selected = true;
                    dataShop.CurrentCell = dataShop.Rows[hti.RowIndex].Cells[0];
                }
            }
        }
        private void ShopDelete_Click(object? sender, EventArgs e)
        {
            DeleteSelectedShop();
        }
        private void DataShop_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedShop();
                e.Handled = true;
            }
            UpdateFormTitle();
        }
        private void DeleteSelectedShop()
        {
            if (dataShop.SelectedRows.Count == 0) return;
            int row = dataShop.SelectedRows[0].Index;
            if (row < 0 || row >= shopList.Count) return;

            // Check if this shop still has items
            if (shopList[row].Items.Count > 0)
            {
                MessageBox.Show("This shop still contains items. Please remove all items from this shop before deleting.");
                return;
            }

            // Confirm delete
            if (MessageBox.Show("Are you sure you want to delete this shop?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                shopList.RemoveAt(row);
                dataShop.Rows.RemoveAt(row);
                // Refresh item list and grid
                dataItem.Rows.Clear();
                InitGrid(0);

                // Auto-select next shop if any
                if (dataShop.Rows.Count > 0)
                {
                    int next = Math.Min(row, dataShop.Rows.Count - 1);
                    dataShop.Rows[next].Selected = true;
                    dataShop.CurrentCell = dataShop.Rows[next].Cells[0];
                }
            }

            UpdateFormTitle();
        }

        // Dalam Form1, contoh bila tekan btnNewshop:
        private void btnNewshop_Click(object sender, EventArgs e)
        {
            // Get existing Shop IDs
            var existingIDs = new HashSet<int>(shopList.Select(s => s.ShopID));
            using (var frm = new NewShop(existingIDs))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Add new shop to shopList
                    shopList.Add(new ShopEntry
                    {
                        ShopID = frm.ShopID,
                        ShopName = frm.ShopName,
                        ShopType = frm.ShopType,
                        Items = new List<ShopItem>()
                    });

                    // Sort & refresh dataShop grid
                    shopList.Sort((a, b) => a.ShopID.CompareTo(b.ShopID));
                    dataShop.Rows.Clear();
                    foreach (var s in shopList)
                    {
                        string currency = (s.ShopType == 5) ? "EP Shop" : "Gold Shop";
                        dataShop.Rows.Add(s.ShopID, s.ShopName, s.Items.Count, currency);
                    }

                    // Select new shop row automatically
                    for (int i = 0; i < shopList.Count; i++)
                    {
                        if (shopList[i].ShopID == frm.ShopID)
                        {
                            dataShop.Rows[i].Selected = true;
                            dataShop.CurrentCell = dataShop.Rows[i].Cells[0];
                            break;
                        }
                    }
                    dataItem.Rows.Clear();
                }
            }

            UpdateFormTitle();

        }

        private void btnNewitem_Click(object sender, EventArgs e)
        {
            if (currentShopIndex < 0 || currentShopIndex >= shopList.Count)
            {
                MessageBox.Show("Please select a shop first.");
                return;
            }

            // Dapatkan senarai item dari itemtype.dat
            List<ItemInfo> itemList = new List<ItemInfo>();
            if (itemtypeDatBytes != null && itemtypeDatBytes.Length > 8)
            {
                int count = BitConverter.ToInt32(itemtypeDatBytes, 0);
                int idTableOffset = 4, recordLen = 244, recordTableOffset = itemtypeDatBytes.Length - count * recordLen;
                for (int i = 0; i < count; i++)
                {
                    int itemID = BitConverter.ToInt32(itemtypeDatBytes, idTableOffset + i * 4);
                    string name = Encoding.GetEncoding("GB2312").GetString(itemtypeDatBytes, recordTableOffset + i * recordLen + 4, 16).Split('\0')[0];
                    itemList.Add(new ItemInfo { ID = itemID, Name = name });
                }
            }

            var shop = shopList[currentShopIndex];
            bool isVIPShop = shop.ShopID == 1207;
            int shopCurrency = shop.ShopType; // 0 = Gold, 5 = EP

            var dlg = new NewItem(itemList, isVIPShop, shopCurrency);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var newItem = new ShopItem
                {
                    ItemID = dlg.ItemID,
                    Desc = dlg.Desc,
                    Category = dlg.Category,
                    Hot = dlg.IsHot ? "hot" : "null",
                    New = dlg.IsNew ? "new" : "null",
                    Gold = dlg.Gold,
                    Stone = dlg.EP // dua-dua guna EP field, untuk VIP dan biasa
                };
                shop.Items.Add(newItem);

                foreach (var shopEntry in shopList)
                {
                    foreach (var itm in shopEntry.Items)
                    {
                        if (itm.ItemID == newItem.ItemID)
                        {
                            itm.Gold = newItem.Gold;
                            itm.Stone = newItem.Stone; // Field Stone tu memang digunakan untuk EP juga
                        }
                    }
                }

                dataShop.Rows[currentShopIndex].Cells["ItemCount"].Value = shop.Items.Count;
                dataShop_SelectionChanged(null, null);
                if (dataItem.Rows.Count > 0)
                {
                    dataItem.ClearSelection();
                    dataItem.Rows[dataItem.Rows.Count - 1].Selected = true;
                }
            }

            UpdateFormTitle();
        }

        private void btnDbconnect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                // Build connection string
                string connStr = $"server={txtHostname.Text.Trim()};port={txtPort.Text.Trim()};user={txtUser.Text.Trim()};password={txtPass.Text.Trim()};database={txtDb.Text.Trim()};";
                try
                {
                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    isConnected = true;
                    btnDbconnect.Text = "Disconnect";
                    MessageBox.Show("Connected to database!");
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    MessageBox.Show("Failed to connect!\n" + ex.Message);
                }
            }
            else
            {
                // Disconnect
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                isConnected = false;
                btnDbconnect.Text = "Connect";
                MessageBox.Show("Disconnected!");
            }
        }

        private void btnNameupdate_Click(object sender, EventArgs e)
        {
            if (!isConnected || conn == null)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }
            if (shopList.Count == 0)
            {
                MessageBox.Show("No shop data loaded!");
                return;
            }

            // SQL query - ikut struktur table cq_npc (tukar field/column kalau lain)
            string sql = "SELECT id, name FROM cq_npc";
            var npcNames = new Dictionary<int, string>();
            try
            {
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        npcNames[id] = name;
                    }
                }

                int updateCount = 0;
                foreach (var shop in shopList)
                {
                    if (npcNames.TryGetValue(shop.ShopID, out var newName))
                    {
                        if (!string.IsNullOrWhiteSpace(newName) && shop.ShopName != newName)
                        {
                            shop.ShopName = newName;
                            updateCount++;
                        }
                    }
                }

                // Refresh grid
                dataShop.Rows.Clear();
                foreach (var s in shopList)
                {
                    string currency = (s.ShopType == 5) ? "EP Shop" : "Gold Shop";
                    dataShop.Rows.Add(s.ShopID, s.ShopName, s.Items.Count, currency);
                }
                MessageBox.Show($"Updated {updateCount} shop name(s) from cq_npc!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdateshop_Click(object sender, EventArgs e)
        {
            if (!isConnected || conn == null)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }
            if (shopList.Count == 0)
            {
                MessageBox.Show("No shop data loaded!");
                return;
            }

            try
            {
                // TRUNCATE table
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("TRUNCATE TABLE cq_goods", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                int inserted = 0;
                int nextId = 1;
                foreach (var shop in shopList)
                {
                    foreach (var item in shop.Items)
                    {
                        // INSERT ke cq_goods
                        string sql = "INSERT INTO cq_goods (id, ownerid, itemtype) VALUES (@id, @ownerid, @itemtype)";
                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", nextId++);
                            cmd.Parameters.AddWithValue("@ownerid", shop.ShopID);
                            cmd.Parameters.AddWithValue("@itemtype", item.ItemID);
                            inserted += cmd.ExecuteNonQuery();
                        }

                        // UPDATE cq_itemtype ikut harga (Gold/EP)
                        string sqlUpdate = "UPDATE cq_itemtype SET price=@price, emoney=@emoney WHERE id=@itemid";
                        using (var cmd2 = new MySql.Data.MySqlClient.MySqlCommand(sqlUpdate, conn))
                        {
                            cmd2.Parameters.AddWithValue("@itemid", item.ItemID);
                            cmd2.Parameters.AddWithValue("@price", item.Gold);
                            cmd2.Parameters.AddWithValue("@emoney", item.Stone);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show($"Truncated and updated {inserted} item(s) to cq_goods, and updated price/emoney in cq_itemtype!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void UpdateFormTitle()
        {
            int totalShop = shopList.Count;
            int totalItem = shopList.Sum(s => s.Items.Count);
            this.Text = $"Shop Editor by DuaSelipar - Total Shop: {totalShop} | Total Item: {totalItem}";
        }

        private void DataItem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dataItem.SelectedRows.Count > 0 && currentShopIndex >= 0)
            {
                int idx = dataItem.SelectedRows[0].Index;
                if (idx >= 0 && idx < shopList[currentShopIndex].Items.Count)
                {
                    shopList[currentShopIndex].Items.RemoveAt(idx);
                    dataItem.Rows.RemoveAt(idx);
                    dataShop.Rows[currentShopIndex].Cells["ItemCount"].Value = shopList[currentShopIndex].Items.Count;
                    UpdateFormTitle();
                }
                e.Handled = true;
            }
        }
    }

    public class ShopEntry
    {
        public int ShopID { get; set; }
        public string ShopName { get; set; } = string.Empty; // Initialize with a default value
        public int Offset { get; set; }
        public int ShopType { get; set; }
        public List<ShopItem> Items { get; set; } = new List<ShopItem>(); // Initialize with an empty list
    }
    public class ShopItem
    {
        public int ItemID { get; set; }
        public string Desc { get; set; } = string.Empty; // Initialize with a default value
        public string Category { get; set; } = string.Empty; // Initialize with a default value
        public string Hot { get; set; } = string.Empty; // Initialize with a default value
        public string New { get; set; } = string.Empty; // Initialize with a default value
        public int Gold { get; set; }
        public int Stone { get; set; }
    }
}