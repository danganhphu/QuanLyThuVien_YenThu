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
    public partial class FrmQLThuThu : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmQLThuThu()
        {
            InitializeComponent();
        }

        private bool checkPhoneNumber(string phoneNumber)
        {
            var flag = false;
            if (phoneNumber != "")
            {
                var firstNumber = phoneNumber.Substring(0, 2);
                if (firstNumber == "09" || firstNumber == "08" || firstNumber == "07" || firstNumber == "06" || firstNumber == "05" || firstNumber == "04" || firstNumber == "03")
                {
                    if (phoneNumber.Length == 10)
                    {
                        flag = true;
                    }
                }
                else if (firstNumber == "01" || firstNumber == "02")
                {
                    if (phoneNumber.Length == 11)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        private void LoadDataThuThu()
        {
            var load = from a in db.ThuThus
                       select new
                       {
                           a.MaTT,
                           a.Hoten,
                           a.GioiTinh,
                           a.DiaChi,
                           a.NgaySinh,
                           a.SoDienThoai,
                       };
            dataGridViewThuThu.DataSource = load;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
        }

        private void ClearTxt()
        {
            txtMaTT.Clear();
            txtHoTen.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            rbNam.Checked = false;
            rbNu.Checked = false;
            txtMaTT.Clear();
            txtHoTen.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaTT.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn thủ thư cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá thủ thư này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ThuThu tt = db.ThuThus.SingleOrDefault(n => n.MaTT.Equals(txtMaTT.Text.Trim()));
                    if (tt == null)
                    {
                        MessageBox.Show("Vui lòng chọn thủ thư cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.ThuThus.DeleteOnSubmit(tt);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataThuThu();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTT.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn thủ thư cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtHoTen.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0) || txtMaTT.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!checkPhoneNumber(txtSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    ThuThu tt = db.ThuThus.SingleOrDefault(n => n.MaTT.Equals(txtMaTT.Text.Trim()));
                    tt.Hoten = txtHoTen.Text.Trim();
                    if (rbNam.Checked)
                    {
                        tt.GioiTinh = rbNam.Text;
                    }
                    if (rbNu.Checked)
                    {
                        tt.GioiTinh = rbNu.Text;
                    }
                    tt.DiaChi = txtDiaChi.Text.Trim();
                    tt.NgaySinh = DateTime.Parse(dateTimePickerNgaySinh.Value.ToShortDateString());
                    tt.SoDienThoai = txtSDT.Text.Trim();
                    db.SubmitChanges();
                    MessageBox.Show("Sửa thông tin thủ thư thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTxt();
                    LoadDataThuThu();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtHoTen.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0) || txtMaTT.Text.Trim().Length.Equals(0) || txtSDT.Text.Length.Equals(1))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (dateTimePickerNgaySinh.Value.Date >= DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!rbNam.Checked && !rbNu.Checked)
                {
                    MessageBox.Show("Vui lòng chọn giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!checkPhoneNumber(txtSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    ThuThu checkMaTL = db.ThuThus.SingleOrDefault(n => n.MaTT.Equals(txtMaTT.Text));
                    if (checkMaTL != null)
                    {
                        MessageBox.Show("Mã này đã tồn tại trong danh mục! Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        ThuThu tt = new ThuThu();
                        tt.MaTT = txtMaTT.Text.Trim();
                        tt.Hoten = txtHoTen.Text.Trim();
                        if (rbNam.Checked)
                        {
                            tt.GioiTinh = rbNam.Text;
                        }
                        if (rbNu.Checked)
                        {
                            tt.GioiTinh = rbNu.Text;
                        }
                        tt.DiaChi = txtDiaChi.Text;
                        tt.NgaySinh = DateTime.Parse(dateTimePickerNgaySinh.Value.ToShortDateString());
                        tt.SoDienThoai = txtSDT.Text;
                        db.ThuThus.InsertOnSubmit(tt);
                        db.SubmitChanges();
                        MessageBox.Show("Thêm thủ thư thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataThuThu();
                    }
                }
            }
        }

        private void FrmQLThuThu_Load(object sender, EventArgs e)
        {
            rbNam.Checked = false;
            rbNu.Checked = false;
            LoadDataThuThu();
        }

        private void dataGridViewThuThu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewThuThu.SelectedRows)
            {
                txtMaTT.Text = dr.Cells[0].Value.ToString();
                txtHoTen.Text = dr.Cells[1].Value.ToString();
                txtDiaChi.Text = dr.Cells[3].Value.ToString();
                txtSDT.Text = dr.Cells[5].Value.ToString();
                if (dr.Cells[2].Value.ToString().Equals("Nam"))
                {
                    rbNam.Checked = true;
                }
                if (dr.Cells[2].Value.ToString().Equals("Nữ"))
                {
                    rbNu.Checked = true;
                }
                dateTimePickerNgaySinh.Text = dr.Cells[4].Value.ToString();
            }
            btnThem.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            txtMaTT.Enabled = false;
        }

        private void txtHoTen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
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
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            txtMaTT.Enabled = true;
            btnXoa.Enabled = false;
            ClearTxt();
        }
    }
}