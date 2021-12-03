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
    public partial class FrmDangNhap : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmDangNhap()
        {
            InitializeComponent();
            this.ActiveControl = txtTaiKhoan;
            txtTaiKhoan.Focus();
        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {
            if (checkboxShowPassword.Checked)
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = false;
            }
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbType.SelectedItem = "User";
        }

        private void checkboxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxShowPassword.Checked)
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = false;
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaiKhoan.Focus();
            }
            else if (txtMatKhau.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
            }
            else
            {
                TaiKhoan check = db.TaiKhoans.SingleOrDefault(n => n.TK.Equals(txtTaiKhoan.Text.Trim()) && n.MK.Equals(txtMatKhau.Text.Trim()) && n.Quyen.Equals(cbType.Text));
                if (check == null)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    if (cbType.Text.Equals("User"))
                    {
                        Model.maTT = check.MaTT;
                        MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Main frmMain = new Main();
                        frmMain.ShowDialog();
                    }
                    if (cbType.Text.Equals("Admin"))
                    {
                        MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        FrmQuanLy frmTT = new FrmQuanLy();
                        frmTT.ShowDialog();
                    }
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát khỏi hệ thống không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}