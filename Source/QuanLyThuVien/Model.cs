using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien
{
    public partial class Model
    {
        public static string maTT;

        public static Boolean checkValid(string text)
        {
            Boolean flag = false;
            foreach (char ch in text)
            {
                if (char.IsLetter(ch))
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}