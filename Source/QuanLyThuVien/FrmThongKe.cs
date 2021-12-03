using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using app = Microsoft.Office.Interop.Excel.Application;

namespace QuanLyThuVien
{
    public partial class FrmThongKe : Form
    {
        private DBQuanLyThuVienDataContext db = new DBQuanLyThuVienDataContext();

        public FrmThongKe()
        {
            InitializeComponent();
        }

        private void LoadDataSach()
        {
            var load = from a in db.Saches
                       orderby a.SoLanMuon descending
                       select new
                       {
                           a.MaSach,
                           a.TenSach,
                           a.SoLanMuon,
                       };
            dataGridViewSach.DataSource = load;
            dataGridViewSach.Columns[0].Width = 80;
            dataGridViewSach.Columns[1].Width = 500;
        }

        private void LoadComboSL()
        {
            var sltk = db.QuyDinhs.SingleOrDefault(n => n.MaQD.Equals(2)).SoLuongQD;
            cbSoSach.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 1; i <= sltk; i++)
            {
                cbSoSach.Items.Add(i);
            }
            cbSoSach.Items.Add("");
        }

        private void FrmThongKe_Load(object sender, EventArgs e)
        {
            LoadDataSach();
            LoadComboSL();
        }

        private void cbSoSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSoSach.Text == "")
            {
                LoadDataSach();
            }
            else
            {
                var load = (from a in db.Saches
                            orderby a.SoLanMuon descending
                            select new
                            {
                                a.MaSach,
                                a.TenSach,
                                a.SoLanMuon
                            }).Take(int.Parse(cbSoSach.Text.ToString()));
                dataGridViewSach.DataSource = load;
            }
        }

        private void xuatfileExcel(DataGridView g, string path, string tenfile)
        {
            app obj = new app();
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 30;

            for (int i = 1; i < g.ColumnCount + 1; i++)
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)
            {
                for (int j = 0; j < g.Columns.Count; j++)
                {
                    if (g.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            obj.ActiveWorkbook.SaveCopyAs(path + tenfile + ".xlsx");
            obj.ActiveWorkbook.Saved = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xuatfileExcel(dataGridViewSach, @"C:\Users\Admin\Desktop\QLTV_ThongKe\", "QLThuVien_ThongKe");
            MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}