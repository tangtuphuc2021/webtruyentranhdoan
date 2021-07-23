using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class NguoidungController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection,DocGia dg)
        {
            var hoten = collection["Họ Tên"];
            var tendn = collection["Tên đăng nhập"];
            var matkhau = collection["Mật khẩu"];
            var nhaplaimatkhau = collection["Nhập lại mật khẩu"];
            var email = collection["Địa chỉ Email"];
            var diachidg = collection["Địa chỉ"];
            var dienthoaidg = collection["SDT"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}",collection["Ngày sinh"]);
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(diachidg))
            {
                ViewData["Loi6"] = "Phải nhập địa chỉ khách hàng";
            }
            else if (String.IsNullOrEmpty(dienthoaidg))
            {
                ViewData["Loi7"] = "Phải nhập điện thoại";
            }
            else
            {
                dg.HoTen = hoten;
                dg.Taikhoan = tendn;
                dg.Matkhau = matkhau;
                dg.Email = email;
                dg.Diachi = diachidg;
                dg.Dienthoai = dienthoaidg;
                dg.Ngaysinh = DateTime.Parse(ngaysinh);
                data.DocGias.InsertOnSubmit(dg);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
                
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            
            var tendn = collection["Tên đăng nhập"];
            var matkhau = collection["Mật khẩu"];
            

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
                {

                DocGia kh = data.DocGias.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    //ViewBag.tendn = kh.HoTen;
                    Session["id"] = kh.MaDG;
                    return RedirectToAction("Index", "WebTruyen");
                   
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "WebTruyen");
        }
        public ActionResult TTDocGia(int id)
        {
            var info = from IC in data.DocGias where IC.MaDG == id select IC;
            return View(info.Single());
        }
        [HttpPost, ActionName("TTDocGia")]
        public ActionResult Sua(int id)
        {
            DocGia Cus = data.DocGias.SingleOrDefault(n => n.MaDG == id);
            UpdateModel(Cus);
            data.SubmitChanges();
            return RedirectToAction("TTDocGia", "NguoiDung");
        }
        public ActionResult History(int id)
        {
            var his = from h in data.ChiTietDonMuas.OrderByDescending(n => n.MaDonHang) where h.DonMuaTruyen.MaDG == id select h;
            return View(his);
        }
    }
}