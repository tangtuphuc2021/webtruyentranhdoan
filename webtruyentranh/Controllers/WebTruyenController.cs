using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
using PagedList;
using PagedList.Mvc;
namespace webtruyentranh.Controllers
{
    public class WebTruyenController : Controller
    {
        //tạo 1 đối tượng data chứa dữ liệu model dbwebtruyen đã tạo
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        //hàm lấy n quyển sách
        private List<Truyen> laytruyenmoi(int count)
        {
            //sap xep giam dan theo ngay cap nhat , lay count dong dau
            return data.Truyens.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        // GET: WebTruyen
        public ActionResult Index(int ? page)
        {
            int pagesize = 5;
            int pagenum = (page ?? 1);
            var truyenmoi = laytruyenmoi(19);
            return View(truyenmoi.ToPagedList(pagenum, pagesize));
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

        public ActionResult Check(int ID)
        {
            var truyen = from s in data.Truyens where s.MaTruyen == ID select s;

            if (truyen.SingleOrDefault(n => n.MaTinhTrang == 1) != null)
               return  RedirectToAction("Chitiettruyen", "WebTruyen", new { id=ID});

            return RedirectToAction("Chitiettruyentraphi", "WebTruyen", new { id=ID });
           

        }
        public ActionResult Chitiettruyen(int id)
        {
            
            var truyen = from s in data.Truyens where s.MaTruyen == id select s;

            return View(truyen.Single());
        }


        public ActionResult Chitiettruyentraphi(int id)
        {
            var truyen = from s in data.Truyens where s.MaTruyen == id select s;
            return View(truyen.Single());
        }



        public ActionResult Sachtheotheloai(int id, int? page)
        {
            int pagesize = 5;
            int pagenum = (page ?? 1);
            var truyen = from s in data.Truyens where s.MaTL==id select s;
            return View(truyen.ToPagedList(pagenum, pagesize));
        }
        public ActionResult ThaoLuan()
        {

            return PartialView();
        }




        public ActionResult SachtheoNXB(int id,int ? page)
        {
            int pagesize = 5;
            int pagenum = (page ?? 1);
            var truyen = from s in data.Truyens where s.MaNXB == id select s;
            return View(truyen.ToPagedList(pagenum, pagesize));
        }
        public ActionResult TinhTrang()
        {
          
            var tinhtrang = from cd in data.TinhTrangs select cd;
            return PartialView(tinhtrang);
        }
        
        public ActionResult Sachtheotinhtrang(int id, int? page)
        {
            int pagesize = 5;
            int pagenum = (page ?? 1);
            var truyen = from s in data.Truyens where s.MaTinhTrang == id select s;
            return View(truyen.ToPagedList(pagenum, pagesize));
        }
        [HttpPost]
        public ActionResult Search(string keyword)
        {

            var all = data.Truyens.Where(x => x.TenTruyen.Contains(keyword));

            return View(all);
        }
    }
}