using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVien
{
    public partial class FrmQLDocGia : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmQLDocGia()
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

        private void LoadDataDocGia()
        {
            var load = from a in db.DocGias
                       select new
                       {
                           a.MaDocGia,
                           a.Hoten,
                           a.GioiTinh,
                           a.DiaChi,
                           a.NgaySinh,
                           a.SoDienThoai,
                       };
            dataGridViewDocGia.DataSource = load;
        }

        private void ClearTxt()
        {
            txtMaDG.Clear();
            txtHoTen.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            rbNam.Checked = false;
            rbNu.Checked = false;
            txtMaDG.Clear();
            txtHoTen.Focus();
        }

        private void FrmQLDocGia_Load(object sender, EventArgs e)
        {
            rbNam.Checked = false;
            rbNu.Checked = false;
            LoadDataDocGia();
        }

        private void dataGridViewDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewDocGia.SelectedRows)
            {
                txtMaDG.Text = dr.Cells[0].Value.ToString();
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
                dateTimePicker1.Text = dr.Cells[4].Value.ToString();
            }
            btnThem.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var st = db.QuyDinhs.SingleOrDefault(n => n.MaQD.Equals(3)).SoLuongQD;
            if (txtHoTen.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0) || txtSDT.Text.Length.Equals(1))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (!rbNam.Checked && !rbNu.Checked)
                {
                    MessageBox.Show("Vui lòng chọn giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!checkPhoneNumber(txtSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (dateTimePicker1.Value.Date >= DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ vui lòng thử lại!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (DateTime.Now.Year - dateTimePicker1.Value.Year < st)
                {
                    MessageBox.Show("Độc giả không đủ " + st + " tuổi để cấp thẻ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DocGia dg = new DocGia();
                    dg.Hoten = txtHoTen.Text.Trim();
                    if (rbNam.Checked)
                    {
                        dg.GioiTinh = rbNam.Text;
                    }
                    if (rbNu.Checked)
                    {
                        dg.GioiTinh = rbNu.Text;
                    }
                    dg.DiaChi = txtDiaChi.Text;
                    dg.NgaySinh = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
                    dg.SoDienThoai = txtSDT.Text;
                    db.DocGias.InsertOnSubmit(dg);
                    db.SubmitChanges();
                    MessageBox.Show("Thêm độc giả thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTxt();
                    LoadDataDocGia();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaDG.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn độc giả cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá độc giả này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DocGia dg = db.DocGias.SingleOrDefault(n => n.MaDocGia.Equals(int.Parse(txtMaDG.Text.Trim())));
                    if (dg == null)
                    {
                        MessageBox.Show("Vui lòng chọn độc giả cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.DocGias.DeleteOnSubmit(dg);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        btnThem.Enabled = true;
                        LoadDataDocGia();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaDG.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn độc giả cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtHoTen.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (dateTimePicker1.Value.Date >= DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ vui lòng thử lại!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!checkPhoneNumber(txtSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DocGia dg = db.DocGias.SingleOrDefault(n => n.MaDocGia.Equals(int.Parse(txtMaDG.Text)));
                    dg.Hoten = txtHoTen.Text.Trim();
                    if (rbNam.Checked)
                    {
                        dg.GioiTinh = rbNam.Text;
                    }
                    if (rbNu.Checked)
                    {
                        dg.GioiTinh = rbNu.Text;
                    }
                    dg.DiaChi = txtDiaChi.Text.Trim();
                    dg.NgaySinh = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
                    dg.SoDienThoai = txtSDT.Text.Trim();
                    db.SubmitChanges();
                    MessageBox.Show("Sửa thông tin độc giả thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearTxt();
                    btnThem.Enabled = true;
                    LoadDataDocGia();
                }
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
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

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            ClearTxt();
        }
    }
}