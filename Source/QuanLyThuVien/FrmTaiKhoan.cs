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
    public partial class FrmTaiKhoan : Form
    {
        DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();
        public FrmTaiKhoan()
        {
            InitializeComponent();
        }

        private void LoadDataTaiKhoan()
        {
            dataGridViewTaiKhoan.DataSource = from a in db.TaiKhoans select new { a.MaTK, a.TK, a.MK, a.MaTT, a.Quyen };
            cbtype.DropDownStyle = ComboBoxStyle.DropDownList;
            cbtype.SelectedItem = "User";
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnthem.Enabled = true;
            cbMaTT.Enabled = true;
            cbMaTT.SelectedIndex = -1;
        }

        private void LoadComboMaTT()
        {
            cbMaTT.DataSource = db.ThuThus.OrderBy(n => n.MaTT).Select(n => n.MaTT);
        }

        private void ClearTxT()
        {
            txtMaTK.Clear();
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text.Trim().Length.Equals(0) || txtMatKhau.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TaiKhoan checkTK = db.TaiKhoans.SingleOrDefault(n => n.TK.Equals(txtTaiKhoan.Text.Trim()));
                if (checkTK != null)
                {
                    MessageBox.Show("Tài khoản đã tồn tại. Vui lòng chọn tài khoản khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTaiKhoan.Focus();
                }
                else
                {
                    TaiKhoan checkMaTT = db.TaiKhoans.SingleOrDefault(n => n.MaTT.Equals(cbMaTT.Text.Trim()));
                    ThuThu checkTonTaiMa = db.ThuThus.SingleOrDefault(n => n.MaTT.Equals(cbMaTT.Text.Trim()));
                    if (checkMaTT != null)
                    {
                        MessageBox.Show("Mã thủ thư "+ cbMaTT.Text.ToString() +" đã được cấp tài khoản. Vui lòng chọn mã thủ thư khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (checkMaTT == null)
                    {
                        if (cbMaTT.Text.Equals("") && cbtype.Text.Equals("Admin"))
                        {
                            TaiKhoan tk = new TaiKhoan();
                            tk.TK = txtTaiKhoan.Text.Trim();
                            tk.MK = txtMatKhau.Text.Trim();
                            tk.MaTT = cbMaTT.Text.Trim().Equals("") ? null : cbMaTT.Text.Trim();
                            tk.Quyen = cbtype.Text.Trim();
                            db.TaiKhoans.InsertOnSubmit(tk);
                            db.SubmitChanges();
                            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearTxT();
                            LoadDataTaiKhoan();
                        }
                        else
                        {
                            if (cbMaTT.Text.Trim().Equals("") && cbtype.Text.Trim().Equals("User"))
                            {
                                MessageBox.Show("Vui lòng chọn mã thủ thư cần cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if (checkTonTaiMa == null)
                            {
                                MessageBox.Show("Thủ thư có mã " + cbMaTT.Text.ToString() + " hiện không tồn tại. Vui lòng kiểm tra lại trước khi cấp tài khoản cho thủ thư mã " + cbMaTT.Text.ToString() + "!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if (cbMaTT.Text.Trim().Length.Equals(0) && cbtype.Text.Equals("User"))
                            {
                                MessageBox.Show("Vui lòng chọn mã thủ thư cần cấp tài khoản!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if (cbMaTT.Text.Trim().Length > 0 && cbtype.Text.Equals("Admin"))
                            {
                                MessageBox.Show("Cấp tài khoản cho thủ thư vui lòng chọn loại User!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                TaiKhoan tk = new TaiKhoan();
                                tk.TK = txtTaiKhoan.Text.Trim();
                                tk.MK = txtMatKhau.Text.Trim();
                                tk.MaTT = cbMaTT.Text.Trim().Equals("") ? null : cbMaTT.Text.Trim();
                                tk.Quyen = cbtype.Text.Trim();
                                db.TaiKhoans.InsertOnSubmit(tk);
                                db.SubmitChanges();
                                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearTxT();
                                LoadDataTaiKhoan();
                            }
                        }
                    }
                }
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (txtMaTK.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (txtTaiKhoan.Text.Trim().Length.Equals(0) || txtMatKhau.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ThuThu checkTonTaiMa = db.ThuThus.SingleOrDefault(n => n.MaTT.Equals(cbMaTT.Text.Trim()));
                if (checkTonTaiMa == null)
                {
                    if (cbMaTT.Text.Equals("") && cbtype.Text.Equals("Admin"))
                    {
                        TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text.Trim()));
                        tk.TK = txtTaiKhoan.Text.Trim();
                        tk.MK = txtMatKhau.Text.Trim();
                        tk.MaTT = cbMaTT.Text.Trim().Equals("") ? null : cbMaTT.Text.Trim();
                        tk.Quyen = cbtype.Text.Trim();
                        db.SubmitChanges();
                        MessageBox.Show("Sửa thông tin tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataTaiKhoan();
                        ClearTxT();
                    }
                    else
                    {
                        if (cbMaTT.Text.Trim().Equals("") && cbtype.Text.Trim().Equals("User"))
                        {
                            MessageBox.Show("Vui lòng chọn mã thủ thư cần sửa thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Thủ thư có mã " + cbMaTT.Text.ToString() + " hiện không tồn tại. Vui lòng kiểm tra lại!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else if (cbMaTT.Text.Trim().Length.Equals(0) && cbtype.Text.Equals("User"))
                {
                    MessageBox.Show("Vui lòng chọn mã thủ thư cần sửa thông tin!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (cbMaTT.Text.Trim().Length > 0 && cbtype.Text.Equals("Admin"))
                {
                    MessageBox.Show("Sửa thông tin tài khoản cho thủ thư vui lòng chọn loại User!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!btnthem.Enabled || cbMaTT.Enabled) {
                    if (!cbMaTT.Enabled)
                    {
                        TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text.Trim()));
                        tk.TK = txtTaiKhoan.Text.Trim();
                        tk.MK = txtMatKhau.Text.Trim();
                        tk.MaTT = cbMaTT.Text.Trim();
                        tk.Quyen = cbtype.Text.Trim();
                        db.SubmitChanges();
                        MessageBox.Show("Sửa thông tin tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataTaiKhoan();
                        ClearTxT();
                    }
                    else
                    {
                        for (int i = 0; i < dataGridViewTaiKhoan.Rows.Count; i++)
                        {
                            string check = dataGridViewTaiKhoan.Rows[i].Cells[3].Value == null ? "" : dataGridViewTaiKhoan.Rows[i].Cells[3].Value.ToString();
                            if (check == cbMaTT.Text.Trim())
                            {
                                MessageBox.Show("Vui lòng kiểm tra lại mã thủ thư phù hợp!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text.Trim()));
                        tk.TK = txtTaiKhoan.Text.Trim();
                        tk.MK = txtMatKhau.Text.Trim();
                        tk.MaTT = cbMaTT.Text.Trim();
                        tk.Quyen = cbtype.Text.Trim();
                        db.SubmitChanges();
                        MessageBox.Show("Sửa thông tin tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataTaiKhoan();
                        ClearTxT();
                    }
                }
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (txtMaTK.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text));
                if (tk == null)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MessageBox.Show("Bạn có chắc chắn xoá tài khoản này khỏi hệ thống không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        db.TaiKhoans.DeleteOnSubmit(tk);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataTaiKhoan();
                        ClearTxT();
                    }
                }
            }
        }

        private void FrmTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadDataTaiKhoan();
            LoadComboMaTT();
            cbMaTT.SelectedIndex = -1;
        }

        private void dataGridViewTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewTaiKhoan.SelectedRows)
            {
                txtMaTK.Text = dr.Cells[0].Value.ToString();
                txtTaiKhoan.Text = dr.Cells[1].Value.ToString();
                txtMatKhau.Text = dr.Cells[2].Value.ToString();
                cbMaTT.Text = dr.Cells[3].Value == null ? "" : dr.Cells[3].Value.ToString();
                cbtype.Text = dr.Cells[4].Value.ToString();
            }
            btnthem.Enabled = false;
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            cbMaTT.Enabled = false;
        }

        private void txtMaTT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            cbMaTT.Enabled = true;
            btnthem.Enabled = true;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtMaTK.Clear();
            cbMaTT.SelectedIndex = -1;
            cbtype.SelectedIndex = 0;
        }

        private void cbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!txtMaTK.Text.Trim().Length.Equals(0)) {
                if (!btnthem.Enabled)
                {
                    if (cbtype.Text.Equals("User"))
                    {
                        TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK == int.Parse(txtMaTK.Text.ToString()));
                        if (tk != null)
                        {
                            if (tk.MaTT == null)
                            {
                                cbMaTT.Text = "";
                                cbMaTT.Enabled = true;
                            }
                            else {
                                cbMaTT.Text = tk.MaTT.ToString();
                                cbMaTT.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        cbMaTT.Enabled = false;
                        cbMaTT.Text = "";
                    }
                }
            }
            else
            {
                if (cbtype.Text.Equals("User"))
                {
                    cbMaTT.Enabled = true;
                }
                else
                {
                    cbMaTT.Enabled = false;
                    cbMaTT.Text = "";
                }
            }
        }
    }
}
