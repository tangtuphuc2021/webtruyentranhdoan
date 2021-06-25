using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class WebTruyenController : Controller
    {
        //tạo 1 đối tượng data chứa dữ liệu model dbwebtruyen đã tạo
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        //hàm lấy n quyển sách
        private List<Truyen> laytruyenmoi (int count)
        {
            //sap xep giam dan theo ngay cap nhat , lay count dong dau
            return data.Truyens.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        // GET: WebTruyen
        public ActionResult Index()
        {
            //lay 5 quyen sach moi nhat
            var truyenmoi = laytruyenmoi(5);
            return View(truyenmoi);
        }
        public ActionResult TheLoai()
        {
            var theloai = from cd in data.TheLoais select cd;
            return PartialView(theloai);
        }
        public ActionResult NXB()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }
      
        public ActionResult Sachtheotheloai(int id)
        {
            var truyen = from s in data.Truyens where s.MaTL==id select s;
            return View(truyen);
        }
        public ActionResult Chitiettruyen(int id)
        {
            var truyen = from s in data.Truyens where s.MaTruyen == id select s;
            return View(truyen.Single());
        }
        public ActionResult SachtheoNXB(int id)
        {
            var truyen = from s in data.Truyens where s.MaNXB == id select s;
            return View(truyen);
        }
    }
}