using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webtruyentranh.Models
{
    public class Giohang
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        public int iMaTruyen { set; get; }
        public String sTenTruyen { set; get; }
        public String sAnhbia { set; get; }
        public double dGiaban { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dGiaban; }
        }
        

        public Giohang(int MaTruyen)
        {
            iMaTruyen = MaTruyen;
            Truyen truyen = data.Truyens.Single(n => n.MaTruyen == iMaTruyen);
            sTenTruyen = truyen.TenTruyen;
            sAnhbia = truyen.Anhbia;
            dGiaban = double.Parse(truyen.Giaban.ToString());
            iSoluong = 1;
        }
    }
}