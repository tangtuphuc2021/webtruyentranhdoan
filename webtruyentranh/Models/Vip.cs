using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webtruyentranh.Models
{
    public class Vip
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        public int iMaTruyen { set; get; }
        public String iTenTruyen { set; get; }
        public String iMota{ set; get; }
        public String iAnhbia { set; get; }
        public int iMaTL{ set; get; }
        public int iMaNXB { set; get; }
        public int iMaTinhTrang { set; get; }

        public Vip(int MaTruyen)
        {
            iMaTruyen = MaTruyen;
            Truyen truyen = data.Truyens.Single(n => n.MaTruyen == iMaTruyen);
            iTenTruyen = truyen.TenTruyen;
            iAnhbia = truyen.Anhbia;
            iMaTinhTrang = int.Parse(truyen.MaTinhTrang.ToString());
        }
    }
}