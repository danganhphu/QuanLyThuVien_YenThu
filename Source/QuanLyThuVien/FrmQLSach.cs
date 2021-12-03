using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVien
{
    public partial class FrmQLSach : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmQLSach()
        {
            InitializeComponent();
            this.ActiveControl = txtTenSach;
            txtTenSach.Focus();
        }

        private AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void BindingData()
        {
            var listSach = db.Saches.Where(n => n.TenSach.Contains(txtTimKiem.Text));
            foreach (var item in listSach)
            {
                collection.Add(item.TenSach);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }

        private void fillGrid()
        {
            var load = from a in db.Saches
                       where a.TenSach.Contains(txtTimKiem.Text)
                       select new
                       {
                           a.MaSach,
                           a.TenSach,
                           a.TacGia,
                           a.MaTheLoai,
                           a.MaNXB,
                           a.DonGia,
                           a.SoLuongTon,
                           a.SoLanMuon,
                           a.TinhTrang
                       };
            dataGridViewSach.DataSource = load;
        }

        private void changeFontSize()
        {
            dataGridViewSach.ColumnHeadersDefaultCellStyle.Font = new Font("arial", 10);
            foreach (DataGridViewColumn dr in dataGridViewSach.Columns)
            {
                dr.DefaultCellStyle.Font = new Font("arial", 10);
            }
        }

        private void LoadComboboxTheLoai()
        {
            var load = from a in db.TheLoais select a.TenLoai;
            cbTenTL.DataSource = load;
            cbTenTL.Font = new Font("arial", 10);
            cbTenTL.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadComboboxTenNXB()
        {
            var load = from a in db.NhaXuatBans select a.TenNXB;
            cbTenNXB.DataSource = load;
            cbTenNXB.Font = new Font("arial", 10);
            cbTenNXB.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadDataTheLoai()
        {
            var load = from a in db.TheLoais
                       select new
                       {
                           a.MaTheLoai,
                           a.TenLoai
                       };
            dataGridViewTheLoai.DataSource = load;
            btnSuaTheLoai.Enabled = false;
            btnXoaTheLoai.Enabled = false;
            btnThemTheLoai.Enabled = true;
        }

        private void LoadDataNXB()
        {
            var load = from a in db.NhaXuatBans
                       select new
                       {
                           a.MaNXB,
                           a.TenNXB,
                           a.DiaChi,
                           a.NgayThanhLap
                       };
            dataGridViewNXB.DataSource = load;
        }

        private void ClearTxt()
        {
            txtMaSach.Clear();
            txtMaTL.Clear();
            txtMaNXB.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtDonGia.Clear();
            txtTinhTrang.Clear();
            txtSoLuongTon.Clear();
            txtTenSach.Focus();
            txtTenTheLoai.Clear();
            txtTenNXB.Clear();
            txtDiaChi.Clear();
        }

        private void LoadDataSach()
        {
            var load = from a in db.Saches
                       select new
                       {
                           a.MaSach,
                           a.TenSach,
                           a.TacGia,
                           a.MaTheLoai,
                           a.MaNXB,
                           a.DonGia,
                           a.SoLuongTon,
                           a.SoLanMuon,
                           a.TinhTrang
                       };
            dataGridViewSach.DataSource = load;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtTenSach.Text.Trim().Length.Equals(0) || txtTacGia.Text.Trim().Length.Equals(0) || txtDonGia.Text.Trim().Length.Equals(0) || txtSoLuongTon.Text.Trim().Length.Equals(0) || txtTinhTrang.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (Model.checkValid(txtTenSach.Text))
                {
                    MessageBox.Show("Tên sách không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenSach.Focus();
                }
                else if (Model.checkValid(txtTacGia.Text))
                {
                    MessageBox.Show("Tên tác giả không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTacGia.Focus();
                }
                else if (!Model.checkValid(txtDonGia.Text))
                {
                    MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                }
                else if (!Model.checkValid(txtSoLuongTon.Text))
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTon.Focus();
                }
                else if (int.Parse(txtDonGia.Text.ToString()) <= 0)
                {
                    MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                }
                else if (int.Parse(txtSoLuongTon.Text.ToString()) <= 0)
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTon.Focus();
                }
                else if (Model.checkValid(txtTinhTrang.Text))
                {
                    MessageBox.Show("Tình trạng không được ghi số. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTinhTrang.Focus();
                }
                else
                {
                    Sach checkTenSach = db.Saches.SingleOrDefault(n => n.TenSach.Equals(txtTenSach.Text));
                    if (checkTenSach != null)
                    {
                        MessageBox.Show("Đã có sách " + txtTenSach.Text.ToString().ToLower() + " trong thư viện. Vui lòng chọn sách khác!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Sach s = new Sach();
                        s.TenSach = txtTenSach.Text.Trim();
                        s.TacGia = txtTacGia.Text.Trim();
                        s.DonGia = int.Parse(txtDonGia.Text.Trim().ToString());
                        s.SoLuongTon = int.Parse(txtSoLuongTon.Text.Trim().ToString());
                        s.SoLanMuon = 0;
                        var query1 = db.TheLoais.SingleOrDefault(n => n.TenLoai.Equals(cbTenTL.Text)).MaTheLoai;
                        s.MaTheLoai = query1.ToString();
                        var query2 = db.NhaXuatBans.SingleOrDefault(n => n.TenNXB.Equals(cbTenNXB.Text)).MaNXB;
                        s.MaNXB = int.Parse(query2.ToString());
                        s.TinhTrang = txtTinhTrang.Text.Trim();
                        db.Saches.InsertOnSubmit(s);
                        db.SubmitChanges();
                        MessageBox.Show("Thêm sách thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataSach();
                        ClearTxt();
                    }
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaSach.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn sách cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá sách này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = int.Parse(dataGridViewSach.SelectedCells[0].Value.ToString());
                    Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(id));
                    if (s == null)
                    {
                        MessageBox.Show("Vui lòng chọn sách cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.Saches.DeleteOnSubmit(s);
                        db.SubmitChanges();
                        MessageBox.Show("Đã xoá 1 cuốn sách thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataSach();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaSach.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtTenSach.Text.Trim().Length.Equals(0) || txtTacGia.Text.Trim().Length.Equals(0) || txtDonGia.Text.Trim().Length.Equals(0) || txtSoLuongTon.Text.Trim().Length.Equals(0) || txtTinhTrang.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin sách cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (Model.checkValid(txtTenSach.Text))
                {
                    MessageBox.Show("Tên sách không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenSach.Focus();
                }
                else if (Model.checkValid(txtTacGia.Text))
                {
                    MessageBox.Show("Tên tác giả không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTacGia.Focus();
                }
                else if (!Model.checkValid(txtDonGia.Text))
                {
                    MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                }
                else if (!Model.checkValid(txtSoLuongTon.Text))
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTon.Focus();
                }
                else if (int.Parse(txtDonGia.Text.ToString()) <= 0)
                {
                    MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                }
                else if (int.Parse(txtSoLuongTon.Text.ToString()) <= 0)
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTon.Focus();
                }
                else if (Model.checkValid(txtTinhTrang.Text))
                {
                    MessageBox.Show("Tình trạng không được ghi số. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTinhTrang.Focus();
                }
                else
                {
                    Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(txtMaSach.Text.Trim()));
                    s.TenSach = txtTenSach.Text.Trim();
                    s.TacGia = txtTacGia.Text.Trim();
                    s.DonGia = int.Parse(txtDonGia.Text.Trim().ToString());
                    s.SoLuongTon = int.Parse(txtSoLuongTon.Text.Trim().ToString());
                    s.TinhTrang = txtTinhTrang.Text.Trim();
                    var query1 = db.TheLoais.SingleOrDefault(n => n.TenLoai.Equals(cbTenTL.Text)).MaTheLoai;
                    var query2 = db.NhaXuatBans.SingleOrDefault(n => n.TenNXB.Equals(cbTenNXB.Text)).MaNXB;
                    s.MaTheLoai = query1.ToString();
                    s.MaNXB = int.Parse(query2.ToString());
                    db.SubmitChanges();
                    MessageBox.Show("Sửa thông tin sách thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataSach();
                    ClearTxt();
                }
            }
        }

        private void FrmQLSach_Load(object sender, EventArgs e)
        {
            changeFontSize();
            LoadComboboxTheLoai();
            LoadComboboxTenNXB();
            LoadDataSach();
            LoadDataTheLoai();
            LoadDataNXB();
            BindingData();
        }

        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSoLuongTon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTinhTrang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTenSach_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTacGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridViewSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewSach.SelectedRows)
            {
                txtMaSach.Text = dr.Cells[0].Value.ToString();
                txtTenSach.Text = dr.Cells[1].Value.ToString();
                txtTacGia.Text = dr.Cells[2].Value.ToString();
                txtDonGia.Text = dr.Cells[5].Value.ToString();
                txtSoLuongTon.Text = dr.Cells[6].Value.ToString();
                txtSoLanMuon.Text = dr.Cells[7].Value.ToString();
                txtTinhTrang.Text = dr.Cells[8].Value.ToString();
                var query1 = db.TheLoais.SingleOrDefault(n => n.MaTheLoai.Equals(dr.Cells[3].Value.ToString())).TenLoai;
                var query2 = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB.Equals(int.Parse(dr.Cells[4].Value.ToString()))).TenNXB;
                cbTenTL.Text = query1.ToString();
                cbTenNXB.Text = query2.ToString();
            }
            btnThem.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void dataGridViewTheLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewTheLoai.SelectedRows)
            {
                txtMaTL.Text = dr.Cells[0].Value.ToString();
                txtTenTheLoai.Text = dr.Cells[1].Value.ToString();
            }
            btnThemTheLoai.Enabled = false;
            btnSuaTheLoai.Enabled = true;
            btnXoaTheLoai.Enabled = true;
            txtMaTL.Enabled = false;
        }

        private void dataGridViewNXB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewNXB.SelectedRows)
            {
                txtMaNXB.Text = dr.Cells[0].Value.ToString();
                txtTenNXB.Text = dr.Cells[1].Value.ToString();
                txtDiaChi.Text = dr.Cells[2].Value.ToString();
                dateTimeThanhLap.Text = dr.Cells[3].Value.ToString();
            }
            btnThemNXB.Enabled = false;
            btnSuaNXB.Enabled = true;
            btnXoaNXB.Enabled = true;
        }

        private void btnThemTheLoai_Click(object sender, EventArgs e)
        {
            if (txtMaTL.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập mã thể loại sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtTenTheLoai.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (Model.checkValid(txtTenTheLoai.Text))
            {
                MessageBox.Show("Tên thể loại không hợp lệ. Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTheLoai.Focus();
            }
            else
            {
                TheLoai checkMaTL = db.TheLoais.SingleOrDefault(n => n.MaTheLoai.Equals(txtMaTL.Text));
                TheLoai checkTenTL = db.TheLoais.SingleOrDefault(n => n.TenLoai.Equals(txtTenTheLoai.Text));
                if (checkMaTL != null)
                {
                    MessageBox.Show("Mã này đã tồn tại trong danh mục! Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (checkTenTL != null)
                {
                    MessageBox.Show("Tên thẻ loại này đã tồn tại trong danh mục. Vui lòng kiểm tra lại!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TheLoai tl = new TheLoai();
                    tl.MaTheLoai = txtMaTL.Text.Trim();
                    tl.TenLoai = txtTenTheLoai.Text.Trim();
                    db.TheLoais.InsertOnSubmit(tl);
                    db.SubmitChanges();
                    MessageBox.Show("Đã thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataTheLoai();
                    LoadComboboxTheLoai();
                    LoadDataSach();
                    txtMaTL.Clear();
                    txtTenTheLoai.Text = "";
                    txtMaTL.Focus();
                }
            }
        }

        private void btnXoaTheLoai_Click(object sender, EventArgs e)
        {
            if (txtMaTL.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn thể loại cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá tên thể loại này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TheLoai tl = db.TheLoais.SingleOrDefault(n => n.MaTheLoai.Equals(txtMaTL.Text));
                    if (tl == null)
                    {
                        MessageBox.Show("Vui lòng chọn thể loại cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        PhieuMuon pm = db.PhieuMuons.SingleOrDefault(n => n.MaTheLoai.Equals(txtMaTL.Text.Trim()));
                        if (pm != null)
                        {
                            db.PhieuMuons.DeleteOnSubmit(pm);
                            db.SubmitChanges();
                            db.TheLoais.DeleteOnSubmit(tl);
                            db.SubmitChanges();
                            MessageBox.Show("Đã xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTheLoai();
                            LoadDataSach();
                            LoadComboboxTheLoai();
                            ClearTxt();
                        }
                        else
                        {
                            db.TheLoais.DeleteOnSubmit(tl);
                            db.SubmitChanges();
                            MessageBox.Show("Đã xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTheLoai();
                            LoadDataSach();
                            LoadComboboxTheLoai();
                            ClearTxt();
                            txtMaTL.Enabled = true;
                            txtMaTL.Focus();
                        }
                    }
                }
            }
        }

        private void btnSuaTheLoai_Click(object sender, EventArgs e)
        {
            if (txtMaTL.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn tên thể loại cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (txtTenTheLoai.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại sách cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (Model.checkValid(txtTenTheLoai.Text))
            {
                MessageBox.Show("Tên thể loại không hợp lệ. Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTheLoai.Focus();
            }
            else
            {
                TheLoai tl = db.TheLoais.SingleOrDefault(n => n.MaTheLoai.Equals(txtMaTL.Text));
                if (tl == null)
                {
                    MessageBox.Show("Mã thể loại không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    tl.TenLoai = txtTenTheLoai.Text.Trim();
                    db.SubmitChanges();
                    MessageBox.Show("Sửa thông tin thể loại thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataTheLoai();
                    LoadDataSach();
                    LoadComboboxTheLoai();
                    ClearTxt();
                    txtMaTL.Enabled = true;
                    txtMaTL.Focus();
                }
            }
        }

        private void btnThemNXB_Click(object sender, EventArgs e)
        {
            NhaXuatBan checkTenNXB = db.NhaXuatBans.SingleOrDefault(n => n.TenNXB.Equals(txtTenNXB.Text));
            if (txtTenNXB.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà xuất bản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Model.checkValid(txtTenNXB.Text))
            {
                MessageBox.Show("Tên nhà xuất bản không hợp lệ. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNXB.Focus();
            }
            else if (checkTenNXB != null)
            {
                MessageBox.Show("Đã tồn tại nhà xuất bản này trong danh mục. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dateTimeThanhLap.Value.Date >= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày thành lập không thể là ngày tương lai. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                NhaXuatBan nxb = new NhaXuatBan();
                nxb.TenNXB = txtTenNXB.Text.Trim();
                nxb.DiaChi = txtDiaChi.Text.Trim();
                nxb.NgayThanhLap = DateTime.Parse(dateTimeThanhLap.Value.ToShortDateString());
                db.NhaXuatBans.InsertOnSubmit(nxb);
                db.SubmitChanges();
                MessageBox.Show("Thêm nhà xuất bản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataNXB();
                LoadDataSach();
                LoadComboboxTenNXB();
                ClearTxt();
                txtTenNXB.Focus();
            }
        }

        private void btnXoaNXB_Click(object sender, EventArgs e)
        {
            if (txtMaNXB.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn nhà sản xuất cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá nhà xuất bản này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    NhaXuatBan nxb = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB.Equals(int.Parse(txtMaNXB.Text.ToString())));
                    if (nxb == null)
                    {
                        MessageBox.Show("Vui lòng chọn nhà xuất bản cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.NhaXuatBans.DeleteOnSubmit(nxb);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá nhà xuất bản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataNXB();
                        LoadDataSach();
                        LoadComboboxTenNXB();
                        ClearTxt();
                        btnThemNXB.Enabled = true;
                        btnSuaNXB.Enabled = false;
                        btnXoaNXB.Enabled = false;
                        txtTenNXB.Focus();
                    }
                }
            }
        }

        private void btnSuaNXB_Click(object sender, EventArgs e)
        {
            NhaXuatBan checkTenNXB = db.NhaXuatBans.SingleOrDefault(n => n.TenNXB.Equals(txtTenNXB.Text));
            if (txtTenNXB.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà xuất bản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Model.checkValid(txtTenNXB.Text))
            {
                MessageBox.Show("Tên nhà xuất bản không hợp lệ. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNXB.Focus();
            }
            //else if (checkTenNXB != null)
            //{
            //    MessageBox.Show("Đã tồn tại nhà xuất bản này trong danh mục. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            else if (dateTimeThanhLap.Value.Date >= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày thành lập không thể là ngày tương lai. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                NhaXuatBan nxb = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB.Equals(int.Parse(txtMaNXB.Text.Trim())));
                nxb.TenNXB = txtTenNXB.Text.Trim();
                nxb.DiaChi = txtDiaChi.Text.Trim();
                nxb.NgayThanhLap = DateTime.Parse(dateTimeThanhLap.Value.ToString());
                db.SubmitChanges();
                MessageBox.Show("Sửa thông tin nhà xuất bản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataNXB();
                LoadDataSach();
                LoadComboboxTenNXB();
                ClearTxt();
                btnThemNXB.Enabled = true;
                btnSuaNXB.Enabled = false;
                btnXoaNXB.Enabled = false;
                txtTenNXB.Focus();
            }
        }

        private void txtTenTheLoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTenNXB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            fillGrid();
        }

        private void txtMaTL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThemTheLoai.Enabled = true;
            btnSuaTheLoai.Enabled = false;
            txtMaTL.Enabled = true;
            btnXoaTheLoai.Enabled = false;
            txtMaTL.Clear();
            txtTenTheLoai.Clear();
            txtMaTL.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnThemNXB.Enabled = true;
            btnSuaNXB.Enabled = false;
            btnXoaNXB.Enabled = false;
            txtMaNXB.Clear();
            txtTenNXB.Clear();
            txtDiaChi.Clear();
            txtTenNXB.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            ClearTxt();
            txtSoLanMuon.Clear();
            txtTenSach.Focus();
        }
    }
}