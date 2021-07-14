using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;

namespace webtruyentranh.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang==null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int id,string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n=>n.iMaTruyen==id);
                if(sanpham==null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
                else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        public int TongSoluong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if(lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
                
            }
            return iTongSoLuong;
        }
        private Double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);

            }
            return iTongTien;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if(lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "WebTruyen");
            }
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaTruyen == iMaSP);
            if(sanpham!=null)
            {
                lstGiohang.RemoveAll(n => n.iMaTruyen == iMaSP);
                return RedirectToAction("GioHang");
            }
            if(lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "WebTruyen");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult CapnhatGiohang(int id,FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaTruyen == id);
            if(sanpham!=null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "WebTruyen");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() =="")
            
                return RedirectToAction("Dangnhap", "Nguoidung");

            if (Session["Giohang"] == null)

                return RedirectToAction("Index", "WebTruyen");
            else
            {
                List<Giohang> lstGiohang = Laygiohang();
                ViewBag.Tongsoluong = TongSoluong();
                ViewBag.Tongtien = TongTien();
                return View(lstGiohang);
            }
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DonMuaTruyen dmt = new DonMuaTruyen();
            DocGia kh = (DocGia)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            dmt.MaDG = kh.MaDG;
            dmt.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            dmt.NgayGiao = DateTime.Parse(ngaygiao);
            dmt.Tinhtranggiaohang = false;
            dmt.Dathanhtoan = false;
            data.DonMuaTruyens.InsertOnSubmit(dmt);
            data.SubmitChanges();
            foreach(var item in gh)
            {
                ChiTietDonMua ctdm = new ChiTietDonMua();
                ctdm.MaDonHang = dmt.MaDonHang;
                ctdm.MaTruyen = item.iMaTruyen;
                ctdm.Soluong = item.iSoluong;
                ctdm.Dongia =(decimal) item.dGiaban;
                data.ChiTietDonMuas.InsertOnSubmit(ctdm);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");

        }

       public ActionResult Xacnhandonhang()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}