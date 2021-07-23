using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class QuangCaoController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        public ActionResult TinTuc()
        {
            var TinTuc = from tt in data.TinTucs select tt;
            return PartialView(TinTuc);
        }

        public ActionResult ThongTinTinTuc(int id)
        {
            var tintuc = from TT in data.TinTucs where TT.MaTinTuc == id select TT;
            return View(tintuc.Single());
        }

        [HttpGet]
        public ActionResult LienHe()
        {

            return View();
        }
        [HttpPost]
        public ActionResult LienHe(FormCollection collection, LienHe dg)
        {
            var hoten = collection["name"];
            var diachidg = collection["Place"];
            var email = collection["email"];
            var dienthoaidg = collection["phone"];
            var GopYdg = collection["message"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên đọc giả không được để trống";
            }

            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "Email không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(dienthoaidg))
            {
                ViewData["Loi3"] = "Số điện thoại không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(diachidg))
            {
                ViewData["Loi4"] = "Địa chỉ không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(GopYdg))
            {
                ViewData["Loi5"] = "Ý kiến không được bỏ trống";
            }
            else
            {
                dg.HoTen = hoten;
                dg.Email = email;
                dg.DiaChi = diachidg;
                dg.DienThoai = dienthoaidg;
                dg.GopY = GopYdg;
                data.LienHes.InsertOnSubmit(dg);
                data.SubmitChanges();
                ViewBag.ThongBao = "Gửi phản hồi thành công";


            }
            return this.LienHe();
        }
        // GET: QuangCao
        public ActionResult Index()
        {
            return View();
        }
    }
}