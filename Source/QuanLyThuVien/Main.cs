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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void quảnLýSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLSach s = new FrmQLSach();
            s.ShowDialog();
        }

        private void quảnLýĐọcGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLDocGia dg = new FrmQLDocGia();
            dg.ShowDialog();
        }

        private void quảnLýThẻToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLThe t = new FrmQLThe();
            t.ShowDialog();
        }

        private void theoDõiMượnSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMuonTraSach a = new FrmMuonTraSach();
            a.ShowDialog();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            FrmDangNhap dn = new FrmDangNhap();
            dn.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
