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
    public partial class FrmQuyDinh : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmQuyDinh()
        {
            InitializeComponent();
        }

        private void FrmQuyDinh_Load(object sender, EventArgs e)
        {
            LoadDataQuyDinh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
            {
                txtMaQuyDinh.Text = dr.Cells[0].Value.ToString();
                txtTenQuyDinh.Text = dr.Cells[1].Value.ToString();
                txtSoLuong.Text = dr.Cells[2].Value.ToString();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaQuyDinh.Text.Trim().Length.Equals(0) || txtTenQuyDinh.Text.Trim().Length.Equals(0) || txtSoLuong.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn loại quy định cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                QuyDinh dg = db.QuyDinhs.SingleOrDefault(n => n.MaQD.Equals(int.Parse(txtMaQuyDinh.Text.ToString())));
                dg.SoLuongQD = int.Parse(txtSoLuong.Text.Trim());
                db.SubmitChanges();
                MessageBox.Show("Sửa thông tin quy định thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearTxt();
                LoadDataQuyDinh();
            }
        }

        private void ClearTxt()
        {
            txtMaQuyDinh.Clear();
            txtTenQuyDinh.Clear();
            txtSoLuong.Clear();
        }

        private void LoadDataQuyDinh()
        {
            var load = from a in db.QuyDinhs
                       select a;

            dataGridView1.DataSource = load;
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}