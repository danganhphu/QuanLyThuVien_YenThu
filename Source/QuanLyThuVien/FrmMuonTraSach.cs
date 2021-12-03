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
    public partial class FrmMuonTraSach : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmMuonTraSach()
        {
            InitializeComponent();
        }

        private AutoCompleteStringCollection collection1 = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection collection2 = new AutoCompleteStringCollection();

        private void BindingDataPM()
        {
            var listSach = db.Saches.Where(n => n.TenSach.Contains(txtTimKiemSach.Text));
            foreach (var item in listSach)
            {
                collection1.Add(item.TenSach);
            }
            txtTimKiemSach.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiemSach.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiemSach.AutoCompleteCustomSource = collection1;
        }

        private void BindingDataPT()
        {
            var listSach = db.Saches.Where(n => n.TenSach.Contains(txtTimKiemSach.Text));
            foreach (var item in listSach)
            {
                collection2.Add(item.TenSach);
            }
            txtTimKiemTheTra.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiemTheTra.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiemTheTra.AutoCompleteCustomSource = collection2;
        }

        private void fillGridPM()
        {
            var load = from a in db.PhieuMuons
                       where a.TenSach.Contains(txtTimKiemSach.Text)
                       select new
                       {
                           a.MaPhieuMuon,
                           a.MaSach,
                           a.TenSach,
                           a.MaThe,
                           a.SoLuongMuon,
                           a.NgayMuon,
                           a.NgayTra
                       };
            dataGridViewPM.DataSource = load;
        }

        private void fillGridPT()
        {
            var load = from a in db.PhieuTras
                       where a.Sach.TenSach.Contains(txtTimKiemTheTra.Text)
                       select new
                       {
                           a.MaPhieuTra,
                           a.MaDocGia,
                           a.MaThe,
                           a.MaSach,
                           a.SoLuongMuon,
                           a.SoLuongTra,
                           a.NgayTra,
                           a.TinhTrangSach
                       };
            dataGridViewPhieuTra.DataSource = load;
        }

        private void LoadDataPhieuMuon()
        {
            var load = from a in db.PhieuMuons
                       select new
                       {
                           a.MaPhieuMuon,
                           a.MaSach,
                           a.TenSach,
                           a.MaThe,
                           a.SoLuongMuon,
                           a.NgayMuon,
                           a.NgayTra,
                           a.MaTT
                       };
            dataGridViewPM.DataSource = load;
            cbMaSach.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTenSach.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTenTheLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaThe.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadComboMaSach()
        {
            var load = from a in db.Saches select a.MaSach;
            cbMaSach.DataSource = load;
        }

        private void LoadComboTenSach()
        {
            var load = from a in db.Saches select a.TenSach;
            cbTenSach.DataSource = load;
            cbTenSachTra.DataSource = load;
        }

        private void LoadComboTenTheLoai()
        {
            var load = db.TheLoais.OrderByDescending(n => n.MaTheLoai).Select(n => n.TenLoai);
            cbTenTheLoai.DataSource = load;
        }

        private void LoadComboMaThe()
        {
            var load = db.Thes.OrderBy(n => n.MaDocGia).Select(n => n.MaThe);
            cbMaThe.DataSource = load;
            cbMaTheTra.DataSource = load;
        }

        private void LoadPhieuTra()
        {
            var load = from a in db.PhieuTras select new { a.MaPhieuTra, a.MaDocGia, a.MaThe, a.MaSach, a.SoLuongMuon, a.SoLuongTra, a.NgayTra, a.TinhTrangSach, a.MaTT };
            dataGridViewPhieuTra.DataSource = load;
            cbMaDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMaTheTra.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTenSachTra.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTinhTrangSach.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTinhTrangSach.SelectedItem = "Tốt";
        }

        private void LoadComboMaDocGia()
        {
            var load = db.DocGias.OrderBy(n => n.MaDocGia).Select(n => n.MaDocGia);
            cbMaDocGia.DataSource = load;
        }

        private void FrmMuonTraSach_Load(object sender, EventArgs e)
        {
            LoadDataPhieuMuon();
            LoadComboMaSach();
            LoadComboTenSach();
            LoadComboTenTheLoai();
            LoadComboMaThe();
            LoadPhieuTra();
            LoadComboMaDocGia();
            BindingDataPM();
            BindingDataPT();
            changeFontSize();
        }

        private void changeFontSize()
        {
            dataGridViewPhieuTra.ColumnHeadersDefaultCellStyle.Font = new Font("arial", 9);
            foreach (DataGridViewColumn dr in dataGridViewPhieuTra.Columns)
            {
                dr.DefaultCellStyle.Font = new Font("arial", 9);
            }
        }

        private void btnChoMuon_Click(object sender, EventArgs e)
        {
            var ngayHHThe = db.Thes.SingleOrDefault(n => n.MaThe.Equals(cbMaThe.Text)).NgayHetHan;
            var checkSLMuon1 = db.PhieuMuons.Where(n => n.MaThe.Equals(cbMaThe.Text)).Count();
            var checkSLMuon2 = db.PhieuMuons.Where(n => n.MaThe.Equals(cbMaThe.Text)).Sum(n => n.SoLuongMuon);
            var slmuon = db.QuyDinhs.SingleOrDefault(n => n.MaQD.Equals(1)).SoLuongQD;
            if (txtSoLuongMuon.Text.Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập số lượng mượn hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (!Model.checkValid(txtSoLuongMuon.Text))
            {
                MessageBox.Show("Số lượng mượn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongMuon.Focus();
            }
            else if (int.Parse(txtSoLuongMuon.Text.ToString()) > slmuon)
            {
                MessageBox.Show("Chỉ được mượn tối đa " + slmuon + " quyển sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (checkSLMuon2 + int.Parse(txtSoLuongMuon.Text.ToString()) > slmuon)
            {
                if (checkSLMuon2 >= slmuon)
                {
                    MessageBox.Show("Độc giả đã mượn " + checkSLMuon2.ToString() + " quyển sách. Đã đạt số lượng mượn tối đa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Chỉ được mượn tối đa " + slmuon + " quyển sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //if (checkSLMuon2 == 2)
                    //{
                    //    MessageBox.Show("Đọc giả đã mượn " + checkSLMuon2.ToString() + " quyển sách. Bây giờ chỉ được mượn tối đa 1 quyển sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Đọc giả đã mượn " + checkSLMuon2.ToString() + " quyển sách. Bây giờ chỉ được mượn tối đa " + (int.Parse(txtSoLuongMuon.Text.ToString()) - checkSLMuon2).ToString() + " quyển sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //}
                }
            }
            else
            {
                int slm = int.Parse(txtSoLuongMuon.Text.ToString());
                int checkSLTon = int.Parse(db.Saches.SingleOrDefault(n => n.MaSach.Equals(cbMaSach.Text)).SoLuongTon.ToString());
                if (slm == 0)
                {
                    MessageBox.Show("Số lượng mượn không hợp lệ. Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (checkSLTon == 0)
                {
                    MessageBox.Show("Hiện tại sách " + cbTenSach.Text.ToString().ToLower() + " trong thư viện đã hết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (checkSLTon < slm)
                {
                    MessageBox.Show("Hiện tại sách " + cbTenSach.Text.ToString().ToLower() + " trong thư viện chỉ còn " + checkSLTon.ToString() + " quyển", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    PhieuMuon checkTenSachMuon = db.PhieuMuons.SingleOrDefault(n => n.MaThe.Equals(cbMaThe.Text) && n.MaSach.Equals(cbMaSach.Text));
                    if (checkTenSachMuon != null)
                    {
                        MessageBox.Show("Độc giả đã mượn sách " + cbTenSach.Text.ToString() + ". Nếu muốn sửa số lượng mượn của sách này vui lòng xoá phiếu mượn cũ và cập nhật lại số lượng sách muốn mượn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dateTimeNM.Value.Date != DateTime.Now.Date)
                    {
                        MessageBox.Show("Ngày mượn phải là ngày hiện hành", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (dateTimeNM.Value.Date >= dateTimeNT.Value.Date)
                    {
                        MessageBox.Show("Ngày trả phải lớn hơn ngày mượn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else if (ngayHHThe.Value.Date < DateTime.Now.Date)
                    {
                        MessageBox.Show("Thẻ độc giả đã hết hạn. Vui lòng cấp thẻ mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else if (checkSLMuon1 >= slmuon || checkSLMuon2 >= slmuon)
                    {
                        MessageBox.Show("Độc giả đã mượn quá " + slmuon + " cuốn sách chưa trả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        PhieuMuon pm = new PhieuMuon();
                        pm.MaSach = int.Parse(cbMaSach.Text.ToString());
                        pm.TenSach = cbTenSach.Text;
                        pm.MaThe = cbMaThe.Text.Trim();
                        var maTL = db.TheLoais.SingleOrDefault(n => n.TenLoai.Equals(cbTenTheLoai.Text)).MaTheLoai;
                        pm.MaTheLoai = maTL;
                        pm.NgayMuon = DateTime.Parse(dateTimeNM.Value.ToShortDateString());
                        pm.NgayTra = DateTime.Parse(dateTimeNT.Value.ToShortDateString());
                        pm.SoLuongMuon = int.Parse(txtSoLuongMuon.Text.ToString());
                        pm.MaTT = Model.maTT;
                        db.PhieuMuons.InsertOnSubmit(pm);
                        db.SubmitChanges();
                        Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(int.Parse(cbMaSach.Text.ToString())));
                        s.SoLuongTon -= int.Parse(txtSoLuongMuon.Text.ToString());
                        s.SoLanMuon += int.Parse(txtSoLuongMuon.Text.ToString());
                        db.SubmitChanges();
                        MessageBox.Show("Đã cho mượn " + txtSoLuongMuon.Text.Trim() + " quyển sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataPhieuMuon();
                        txtMaPhieuMuon.Clear();
                        txtSoLuongMuon.Clear();
                    }
                }
            }
        }

        private void txtSoLuongMuon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbTenSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSoLuongMuon.Clear();
            var loadMaSach = db.Saches.SingleOrDefault(n => n.TenSach.Equals(cbTenSach.Text)).MaSach;
            cbMaSach.Text = loadMaSach.ToString();
            var loadTenLoai = db.Saches.SingleOrDefault(n => n.TenSach.Equals(cbTenSach.Text)).TheLoai.TenLoai;
            cbTenTheLoai.Text = loadTenLoai.ToString();
        }

        private void cbMaSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSoLuongMuon.Clear();
            var load = db.Saches.SingleOrDefault(n => n.MaSach.Equals(cbMaSach.Text)).TenSach;
            cbTenSach.Text = load.ToString();
            var loadTenLoai = db.Saches.SingleOrDefault(n => n.MaSach.Equals(cbMaSach.Text)).TheLoai.TenLoai;
            cbTenTheLoai.Text = loadTenLoai.ToString();
        }

        private void btnXoaPhieuMuon_Click(object sender, EventArgs e)
        {
            if (txtMaPhieuMuon.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn 1 phiếu mượn cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn huỷ phiếu này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PhieuMuon pm = db.PhieuMuons.SingleOrDefault(n => n.MaPhieuMuon.Equals(txtMaPhieuMuon.Text));
                    if (pm == null)
                    {
                        MessageBox.Show("Vui lòng chọn phiếu mượn cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.PhieuMuons.DeleteOnSubmit(pm);
                        db.SubmitChanges();
                        MessageBox.Show("Đã xoá thành công 1 phiếu mượn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataPhieuMuon();
                        Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(int.Parse(cbMaSach.Text.ToString())));
                        s.SoLuongTon += int.Parse(txtSoLuongMuon.Text.ToString());
                        s.SoLanMuon -= int.Parse(txtSoLuongMuon.Text.ToString());
                        db.SubmitChanges();
                        txtMaPhieuMuon.Clear();
                        txtSoLuongMuon.Clear();
                    }
                }
            }
        }

        private void dataGridViewPM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewPM.SelectedRows)
            {
                txtMaPhieuMuon.Text = dr.Cells[0].Value.ToString();
                cbMaSach.Text = dr.Cells[1].Value.ToString();
                cbTenSach.Text = dr.Cells[2].Value.ToString();
                cbMaThe.Text = dr.Cells[3].Value.ToString();
                txtSoLuongMuon.Text = dr.Cells[4].Value.ToString();
                dateTimeNM.Text = dr.Cells[5].Value.ToString();
                dateTimeNT.Text = dr.Cells[6].Value.ToString();
                var query = db.Saches.SingleOrDefault(n => n.MaSach.Equals(cbMaSach.Text)).TheLoai.TenLoai;
                cbTenTheLoai.Text = query.ToString();
            }
        }

        private void cbMaDocGia_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSoLuongDaMuon.Clear();
            The loadMaThe = db.Thes.SingleOrDefault(n => n.MaDocGia.Equals(cbMaDocGia.Text));
            var tenDG = db.DocGias.SingleOrDefault(n => n.MaDocGia.Equals(cbMaDocGia.Text)).Hoten;
            if (loadMaThe == null)
            {
                MessageBox.Show("Độc giả " + tenDG + " chưa được cấp thẻ!. Vui lòng cấp thẻ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                cbMaTheTra.Text = loadMaThe.MaThe.ToString();
            }
            var slMuon = db.PhieuMuons.Where(n => n.MaThe.Equals(cbMaTheTra.Text) && n.Sach.TenSach.Equals(cbTenSachTra.Text)).Sum(n => n.SoLuongMuon);
            txtSoLuongDaMuon.Text = slMuon.ToString();
        }

        private void cbMaTheTra_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSoLuongDaMuon.Clear();
            var loadMaDocGia = db.Thes.SingleOrDefault(n => n.MaThe.Equals(cbMaTheTra.Text)).DocGia.MaDocGia;
            cbMaDocGia.Text = loadMaDocGia.ToString();
            var slMuon = db.PhieuMuons.Where(n => n.MaThe.Equals(cbMaTheTra.Text) && n.Sach.TenSach.Equals(cbTenSachTra.Text)).Sum(n => n.SoLuongMuon);
            txtSoLuongDaMuon.Text = slMuon.ToString();
        }

        private void txtSoLuongTra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSoLuongDaMuon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbTenSachTra_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSoLuongDaMuon.Clear();
            var getMaSach = db.Saches.SingleOrDefault(n => n.TenSach.Equals(cbTenSachTra.Text)).MaSach;
            var getSoLuongMuon = db.PhieuMuons.Where(n => n.MaThe.Equals(cbMaTheTra.Text) && n.MaSach.Equals(getMaSach));
            foreach (var item in getSoLuongMuon)
            {
                if (item.MaSach.Equals(getMaSach))
                {
                    txtSoLuongDaMuon.Text = getSoLuongMuon.Sum(n => n.SoLuongMuon).ToString();
                }
                else
                {
                    txtSoLuongDaMuon.Text += item.SoLuongMuon.ToString();
                }
            }
        }

        private void btnDong1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void btnXoaPhieuTra_Click(object sender, EventArgs e)
        {
            if (txtMaPhieuTra.Text.Length.Equals(0) && txtMaPhieuMuon.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng chọn 1 phiếu trả cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                PhieuTra pt = db.PhieuTras.SingleOrDefault(n => n.MaPhieuTra.Equals(int.Parse(txtMaPhieuTra.Text.ToString())));
                if (pt == null)
                {
                    MessageBox.Show("Vui lòng chọn 1 phiếu trả cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    db.PhieuTras.DeleteOnSubmit(pt);
                    db.SubmitChanges();
                    MessageBox.Show("Đã xoá thành công 1 phiếu trả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSoLuongDaMuon.Clear();
                    txtSoLuongTra.Clear();
                    txtMaPhieuTra.Clear();
                    LoadPhieuTra();
                }
            }
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            The checkMaThe = db.Thes.SingleOrDefault(n => n.MaDocGia.Equals(cbMaDocGia.Text));
            var tenDG = db.DocGias.SingleOrDefault(n => n.MaDocGia.Equals(cbMaDocGia.Text)).Hoten;
            if (checkMaThe == null)
            {
                MessageBox.Show("Độc giả " + tenDG + " chưa được cấp thẻ!. Vui lòng cấp thẻ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (txtSoLuongDaMuon.Text.Length.Equals(0))
            {
                MessageBox.Show("Độc giả không mượn sách " + cbTenSachTra.Text.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtSoLuongTra.Text.Length.Equals(0) || !Model.checkValid(txtSoLuongTra.Text))
                {
                    MessageBox.Show("Số lượng trả không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTra.Focus();
                }
                else
                {
                    int slTra = int.Parse(txtSoLuongTra.Text.ToString());
                    if (slTra <= 0)
                    {
                        MessageBox.Show("Vui lòng nhập số lượng sách mà đọc giả muốn trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuongTra.Focus();
                    }
                    else
                    {
                        // Lấy ra sách muốn trả
                        var sachMuonTra = db.PhieuMuons.SingleOrDefault(n => n.MaThe.Equals(cbMaTheTra.Text) && n.TenSach.Equals(cbTenSachTra.Text));
                        if (sachMuonTra == null)
                        {
                            MessageBox.Show("Độc giả không mượn sách hoặc đã trả sách " + cbTenSachTra.Text.ToString() + ". Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtSoLuongTra.Clear();
                            txtSoLuongDaMuon.Clear();
                        }
                        else
                        {
                            int getMaSachTra = db.Saches.SingleOrDefault(n => n.TenSach.Equals(cbTenSachTra.Text)).MaSach;
                            if (sachMuonTra.NgayMuon.Value.Date > dateTimeNgayTra.Value.Date)
                            {
                                MessageBox.Show("Ngày độc giả mượn sách là ngày " + sachMuonTra.NgayMuon.Value.Date.ToShortDateString() + " Vui lòng kiểm tra lại ngày độc giả trả sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if (slTra <= int.Parse(txtSoLuongDaMuon.Text.ToString()))
                            {
                                if (sachMuonTra.NgayTra.Value.Date >= dateTimeNgayTra.Value.Date && cbTinhTrangSach.Text.Equals("Tốt"))
                                {
                                    PhieuTra pt = new PhieuTra();
                                    pt.MaDocGia = int.Parse(cbMaDocGia.Text.ToString());
                                    pt.MaThe = cbMaTheTra.Text;
                                    pt.MaSach = getMaSachTra;
                                    pt.SoLuongMuon = int.Parse(txtSoLuongDaMuon.Text.ToString());

                                    pt.SoLuongTra = slTra;
                                    pt.NgayTra = DateTime.Parse(dateTimeNgayTra.Value.ToShortDateString());
                                    pt.TinhTrangSach = cbTinhTrangSach.Text;
                                    pt.MaTT = Model.maTT;
                                    db.PhieuTras.InsertOnSubmit(pt);
                                    db.SubmitChanges();
                                    MessageBox.Show("Đã trả sách thành công và tình trạng sách " + cbTinhTrangSach.Text.ToString().ToLower(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadPhieuTra();
                                    Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(getMaSachTra));
                                    s.SoLuongTon += slTra;
                                    PhieuMuon pm = db.PhieuMuons.SingleOrDefault(n => n.MaThe.Equals(cbMaTheTra.Text) && n.MaSach.Equals(getMaSachTra));
                                    if (slTra < int.Parse(txtSoLuongDaMuon.Text.ToString()))
                                    {
                                        pm.SoLuongMuon -= slTra;
                                        db.SubmitChanges();
                                    }
                                    int checkSLMuon = int.Parse(pm.SoLuongMuon.ToString());
                                    if (txtSoLuongTra.Text == txtSoLuongDaMuon.Text || checkSLMuon == 0)
                                    {
                                        db.PhieuMuons.DeleteOnSubmit(pm);
                                    }
                                    db.SubmitChanges();
                                    txtSoLuongDaMuon.Clear();
                                    txtSoLuongTra.Clear();
                                    txtSoLuongTra.Focus();
                                    LoadDataPhieuMuon();
                                }
                                else if (sachMuonTra.NgayTra.Value.Date <= dateTimeNgayTra.Value.Date && cbTinhTrangSach.Text.Equals("Xấu"))
                                {
                                    TimeSpan soNgay = dateTimeNgayTra.Value.Date - sachMuonTra.NgayTra.Value.Date;
                                    int soNgayTraTre = int.Parse(soNgay.TotalDays.ToString());
                                    int soTienPhat = (int)(soNgayTraTre * 10000 + sachMuonTra.Sach.DonGia * 0.3);
                                    PhieuTra pt = new PhieuTra();
                                    pt.MaDocGia = int.Parse(cbMaDocGia.Text.ToString());
                                    pt.MaThe = cbMaTheTra.Text;
                                    pt.MaSach = getMaSachTra;
                                    pt.SoLuongMuon = int.Parse(txtSoLuongDaMuon.Text.ToString());
                                    pt.SoLuongTra = slTra;
                                    pt.NgayTra = DateTime.Parse(dateTimeNgayTra.Value.ToShortDateString());
                                    pt.TinhTrangSach = cbTinhTrangSach.Text;
                                    pt.MaTT = Model.maTT;
                                    db.PhieuTras.InsertOnSubmit(pt);
                                    db.SubmitChanges();
                                    MessageBox.Show("Đã trả sách thành công và tình trạng sách " + cbTinhTrangSach.Text.ToString().ToLower() + ". Độc giả đã trả muộn " + soNgayTraTre.ToString() + " ngày và số tiền bị phạt là " + String.Format("{0:0,0}", soTienPhat) + " VNĐ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadPhieuTra();
                                    Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(getMaSachTra));
                                    s.SoLuongTon += slTra;
                                    PhieuMuon pm = db.PhieuMuons.SingleOrDefault(n => n.MaThe.Equals(cbMaTheTra.Text) && n.MaSach.Equals(getMaSachTra));
                                    if (slTra < int.Parse(txtSoLuongDaMuon.Text.ToString()))
                                    {
                                        pm.SoLuongMuon -= slTra;
                                        db.SubmitChanges();
                                    }
                                    int checkSLMuon = int.Parse(pm.SoLuongMuon.ToString());
                                    if (txtSoLuongTra.Text == txtSoLuongDaMuon.Text || checkSLMuon == 0)
                                    {
                                        db.PhieuMuons.DeleteOnSubmit(pm);
                                    }
                                    db.SubmitChanges();
                                    txtSoLuongDaMuon.Clear();
                                    txtSoLuongTra.Clear();
                                    txtSoLuongTra.Focus();
                                    LoadDataPhieuMuon();
                                }
                                else if (cbTinhTrangSach.Text.Equals("Xấu") && sachMuonTra.NgayTra.Value.Date >= dateTimeNgayTra.Value.Date)
                                {
                                    int soTienPhat = (int)(sachMuonTra.Sach.DonGia * 0.3);
                                    PhieuTra pt = new PhieuTra();
                                    pt.MaDocGia = int.Parse(cbMaDocGia.Text.ToString());
                                    pt.MaThe = cbMaTheTra.Text;
                                    pt.MaSach = getMaSachTra;
                                    pt.SoLuongMuon = int.Parse(txtSoLuongDaMuon.Text.ToString());
                                    pt.SoLuongTra = slTra;
                                    pt.NgayTra = DateTime.Parse(dateTimeNgayTra.Value.ToShortDateString());
                                    pt.TinhTrangSach = cbTinhTrangSach.Text;
                                    pt.MaTT = Model.maTT;
                                    db.PhieuTras.InsertOnSubmit(pt);
                                    db.SubmitChanges();
                                    MessageBox.Show("Đã trả sách thành công và tình trạng sách " + cbTinhTrangSach.Text.ToString().ToLower() + " số tiền bị phạt là " + String.Format("{0:0,0}", soTienPhat) + " VNĐ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadPhieuTra();
                                    Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(getMaSachTra));
                                    s.SoLuongTon += slTra;
                                    PhieuMuon pm = db.PhieuMuons.SingleOrDefault(n => n.MaThe.Equals(cbMaTheTra.Text) && n.MaSach.Equals(getMaSachTra));
                                    if (slTra < int.Parse(txtSoLuongDaMuon.Text.ToString()))
                                    {
                                        pm.SoLuongMuon -= slTra;
                                        db.SubmitChanges();
                                    }
                                    int checkSLMuon = int.Parse(pm.SoLuongMuon.ToString());
                                    if (txtSoLuongTra.Text == txtSoLuongDaMuon.Text || checkSLMuon == 0)
                                    {
                                        db.PhieuMuons.DeleteOnSubmit(pm);
                                    }
                                    db.SubmitChanges();
                                    txtSoLuongDaMuon.Clear();
                                    txtSoLuongTra.Clear();
                                    txtSoLuongTra.Focus();
                                    LoadDataPhieuMuon();
                                }
                                else if (cbTinhTrangSach.Text.Equals("Tốt") && sachMuonTra.NgayTra.Value.Date <= dateTimeNgayTra.Value.Date)
                                {
                                    TimeSpan soNgay = dateTimeNgayTra.Value.Date - sachMuonTra.NgayTra.Value.Date;
                                    int soNgayTraTre = int.Parse(soNgay.TotalDays.ToString());
                                    int soTienPhat = (int)(soNgayTraTre * 10000);
                                    PhieuTra pt = new PhieuTra();
                                    pt.MaDocGia = int.Parse(cbMaDocGia.Text.ToString());
                                    pt.MaThe = cbMaTheTra.Text;
                                    pt.MaSach = getMaSachTra;
                                    pt.SoLuongMuon = int.Parse(txtSoLuongDaMuon.Text.ToString());
                                    pt.SoLuongTra = slTra;
                                    pt.NgayTra = DateTime.Parse(dateTimeNgayTra.Value.ToShortDateString());
                                    pt.TinhTrangSach = cbTinhTrangSach.Text;
                                    pt.MaTT = Model.maTT;
                                    db.PhieuTras.InsertOnSubmit(pt);
                                    db.SubmitChanges();
                                    MessageBox.Show("Đã trả sách thành công và tình trạng sách " + cbTinhTrangSach.Text.ToString().ToLower() + ". Độc giả đã trả muộn " + soNgayTraTre.ToString() + " ngày và số tiền bị phạt là " + String.Format("{0:0,0}", soTienPhat) + " VNĐ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadPhieuTra();
                                    Sach s = db.Saches.SingleOrDefault(n => n.MaSach.Equals(getMaSachTra));
                                    s.SoLuongTon += slTra;
                                    PhieuMuon pm = db.PhieuMuons.SingleOrDefault(n => n.MaThe.Equals(cbMaTheTra.Text) && n.MaSach.Equals(getMaSachTra));
                                    if (slTra < int.Parse(txtSoLuongDaMuon.Text.ToString()))
                                    {
                                        pm.SoLuongMuon -= slTra;
                                        db.SubmitChanges();
                                    }
                                    int checkSLMuon = int.Parse(pm.SoLuongMuon.ToString());
                                    if (txtSoLuongTra.Text == txtSoLuongDaMuon.Text || checkSLMuon == 0)
                                    {
                                        db.PhieuMuons.DeleteOnSubmit(pm);
                                    }
                                    db.SubmitChanges();
                                    txtSoLuongDaMuon.Clear();
                                    txtSoLuongTra.Clear();
                                    txtSoLuongTra.Focus();
                                    LoadDataPhieuMuon();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Độc giả chỉ mượn có " + txtSoLuongDaMuon.Text.ToString() + " cuốn sách!. Vui lòng kiểm tra lại số lượng trả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtSoLuongTra.Clear();
                                txtSoLuongTra.Focus();
                            }
                        }
                    }
                }
            }
        }

        private void dataGridViewPhieuTra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewPhieuTra.SelectedRows)
            {
                txtMaPhieuTra.Text = dr.Cells[0].Value.ToString();
                cbMaDocGia.Text = dr.Cells[1].Value.ToString();
                cbMaTheTra.Text = dr.Cells[2].Value.ToString();
                cbTenSachTra.Text = db.Saches.SingleOrDefault(n => n.MaSach.Equals(int.Parse(dr.Cells[3].Value.ToString()))).TenSach;
                PhieuMuon checkSLMuon = db.PhieuMuons.SingleOrDefault(n => n.MaThe.Equals(cbMaTheTra.Text) && n.TenSach.Equals(cbTenSachTra.Text));
                if (checkSLMuon == null)
                {
                    txtSoLuongDaMuon.Text = "0";
                }
                else
                {
                    txtSoLuongDaMuon.Text = checkSLMuon.SoLuongMuon.ToString();
                }
                txtSoLuongTra.Text = dr.Cells[5].Value.ToString();
                dateTimeNgayTra.Text = dr.Cells[6].Value.ToString();
                cbTinhTrangSach.Text = dr.Cells[7].Value.ToString();
            }
        }

        private void txtTimKiemThe_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTimKiemThe_TextChanged(object sender, EventArgs e)
        {
            fillGridPM();
        }

        private void txtTimKiemTheTra_TextChanged(object sender, EventArgs e)
        {
            fillGridPT();
        }

        private void txtTimKiemTheTra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }
    }
}