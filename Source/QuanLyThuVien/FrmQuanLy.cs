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
    public partial class FrmQuanLy : Form
    {
        public FrmQuanLy()
        {
            InitializeComponent();
        }

        private void btnkh_Click(object sender, EventArgs e)
        {
            FrmTaiKhoan tk = new FrmTaiKhoan();
            tk.ShowDialog();
        }

        private void btnnv_Click(object sender, EventArgs e)
        {
            FrmQLThuThu qltt = new FrmQLThuThu();
            qltt.ShowDialog();
        }

        private void FrmQuanLy_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            FrmDangNhap dn = new FrmDangNhap();
            dn.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmThongKe tk = new FrmThongKe();
            tk.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmQuyDinh tk = new FrmQuyDinh();
            tk.ShowDialog();
        }
    }
}