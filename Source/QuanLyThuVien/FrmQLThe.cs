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
    public partial class FrmQLThe : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmQLThe()
        {
            InitializeComponent();
        }

        private void LoadDataThe()
        {
            var load = from a in db.Thes
                       select new
                       {
                           a.MaThe,
                           a.MaDocGia,
                           a.NgayCapThe,
                           a.NgayHetHan,
                           a.MaTT
                       };
            dataGridViewThe.DataSource = load;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
        }

        private void LoadComboMaDG()
        {
            var load = from a in db.DocGias select a.Hoten;
            cbMaDG.DataSource = load;
            cbMaDG.Font = new Font("arial", 10);
            cbMaDG.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FrmQLThe_Load(object sender, EventArgs e)
        {
            cbMaDG.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadDataThe();
            LoadComboMaDG();
        }

        private void dataGridViewThe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewThe.SelectedRows)
            {
                txtMaThe.Text = dr.Cells[0].Value.ToString();
                var query1 = db.DocGias.SingleOrDefault(n => n.MaDocGia.Equals(dr.Cells[1].Value.ToString())).Hoten;
                dateTimeNCT.Text = dr.Cells[2].Value.ToString();
                dateTimeNHH.Text = dr.Cells[3].Value.ToString();
                cbMaDG.Text = query1.ToString();
            }
            btnThem.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            txtMaThe.Enabled = false;
            cbMaDG.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var checkMaThe = db.Thes.SingleOrDefault(n => n.MaThe.Equals(txtMaThe.Text.Trim()));
            if (txtMaThe.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng nhập mã thẻ cần cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (checkMaThe != null)
            {
                MessageBox.Show("Mã thẻ đã tồn tại. Vui lòng chọn mã thẻ khác!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (dateTimeNCT.Value.Date != DateTime.Now.Date)
            {
                MessageBox.Show("Ngày cấp thẻ phải là ngày hiện hành", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (dateTimeNHH.Value.Date <= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày hết hạn phải lớn hơn ngày hiện hành", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                var maDG = db.DocGias.SingleOrDefault(n => n.Hoten.Equals(cbMaDG.Text)).MaDocGia;
                The t = db.Thes.SingleOrDefault(n => n.MaDocGia.Equals(maDG));
                if (t != null)
                {
                    MessageBox.Show("Độc giả " + cbMaDG.Text + " đã được cấp thẻ rồi!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (dateTimeNCT.Value.Date >= dateTimeNHH.Value.Date)
                {
                    MessageBox.Show("Ngày hết hạn thẻ phải lớn hơn ngày cấp thẻ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    The the = new The();
                    the.MaThe = txtMaThe.Text.Trim();
                    var query1 = db.DocGias.SingleOrDefault(n => n.Hoten.Equals(cbMaDG.Text)).MaDocGia;
                    the.MaDocGia = int.Parse(query1.ToString());
                    the.NgayCapThe = DateTime.Parse(dateTimeNCT.Value.ToString());
                    the.NgayHetHan = DateTime.Parse(dateTimeNHH.Value.ToString());
                    the.MaTT = Model.maTT;
                    db.Thes.InsertOnSubmit(the);
                    db.SubmitChanges();
                    MessageBox.Show("Đã cấp thẻ cho độc giả thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataThe();
                    txtMaThe.Clear();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaThe.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn thẻ cần sửa thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dateTimeNCT.Value.Date != DateTime.Now.Date)
            {
                MessageBox.Show("Ngày cấp thẻ phải là ngày hiện tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (dateTimeNHH.Value.Date <= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày hết hạn phải lớn hơn ngày hiện hành", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                The t = db.Thes.SingleOrDefault(n => n.MaThe.Equals(txtMaThe.Text));
                if (t == null)
                {
                    MessageBox.Show("Mã thẻ của độc giả không hợp lệ! Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    t.NgayCapThe = DateTime.Parse(dateTimeNCT.Value.ToString());
                    t.NgayHetHan = DateTime.Parse(dateTimeNHH.Value.ToString());
                    db.SubmitChanges();
                    MessageBox.Show("Đã sửa thông tin thẻ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataThe();
                    txtMaThe.Clear();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaThe.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn mã thẻ cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var tenDG = db.Thes.SingleOrDefault(n => n.MaDocGia.Equals(cbMaDG.Text)).DocGia.Hoten;
                if (MessageBox.Show("Bạn có chắc chắn muốn huỷ thẻ đọc giả " + tenDG + " không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    The t = db.Thes.SingleOrDefault(n => n.MaThe.Equals(txtMaThe.Text));
                    if (t == null)
                    {
                        MessageBox.Show("Không tìm thấy mã thẻ này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        PhieuTra pt = db.PhieuTras.SingleOrDefault(n => n.MaThe.Equals(txtMaThe.Text.Trim()));
                        if (pt != null)
                        {
                            db.PhieuTras.DeleteOnSubmit(pt);
                            db.SubmitChanges();
                            db.Thes.DeleteOnSubmit(t);
                            db.SubmitChanges();
                            MessageBox.Show("Đã xoá thành công 1 thẻ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataThe();
                            txtMaThe.Clear();
                        }
                        else
                        {
                            db.Thes.DeleteOnSubmit(t);
                            db.SubmitChanges();
                            MessageBox.Show("Đã xoá thành công 1 thẻ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataThe();
                            txtMaThe.Clear();
                        }
                    }
                }
            }
        }

        private void txtMaThe_KeyPress(object sender, KeyPressEventArgs e)
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
            txtMaThe.Enabled = true;
            btnXoa.Enabled = false;
            cbMaDG.Enabled = true;
            txtMaThe.Clear();
            txtMaThe.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Thẻ Thư Viện", 430, 330);
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Image image = Resources.bus_icon;
            e.Graphics.DrawString("Thẻ Thư Viện Yên Thư", new Font("Times new Roman", 20, FontStyle.Bold), Brushes.Black, new Point(100, 0));
            //e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            foreach (DataGridViewRow row in dataGridViewThe.SelectedRows)
            {
                e.Graphics.DrawString("Mã thẻ: " + row.Cells[0].Value.ToString(), new Font("Times new Roman", 16, FontStyle.Regular), Brushes.Black, new Point(90, 50));
                e.Graphics.DrawString("Họ tên: " + cbMaDG.Text, new Font("Times new Roman", 16, FontStyle.Regular), Brushes.Black, new Point(90, 80));
                e.Graphics.DrawString("Ngày cấp thẻ: " + row.Cells[2].FormattedValue.ToString(), new Font("Times new Roman", 16, FontStyle.Regular), Brushes.Black, new Point(90, 110));
                e.Graphics.DrawString("Ngày hết hạn: " + row.Cells[3].FormattedValue.ToString(), new Font("Times new Roman", 16, FontStyle.Regular), Brushes.Black, new Point(90, 140));
                var tenTT = db.Thes.FirstOrDefault(n => n.MaTT.Equals(row.Cells[4].Value.ToString())).ThuThu.Hoten;
                e.Graphics.DrawString(tenTT, new Font("Times new Roman", 16, FontStyle.Regular), Brushes.Black, new Point(230, 290));
            }
            e.Graphics.DrawString("Ký tên (Thủ Thư)", new Font("Times new Roman", 16, FontStyle.Regular), Brushes.Black, new Point(230, 200));
        }
    }
}