using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
using PagedList;
namespace webtruyentranh.Controllers
{
    public class DonHangController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: DonHang
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                int pagesize = 4;
                int pagenum = (page ?? 1);
                return View(data.ChiTietDonMuas.OrderByDescending(n => n.MaDonHang).ToList().ToPagedList(pagenum, pagesize));
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {

                DonMuaTruyen donmuatruyen = data.DonMuaTruyens.SingleOrDefault(n => n.MaDonHang == id);
                ViewBag.Tinhtranggiaohang = new SelectList(data.TrangThais.ToList().OrderBy(n => n.TrangThai1), "id", "TrangThai1", donmuatruyen.Tinhtranggiaohang);
                return View(donmuatruyen);

            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Xacnhansua(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DonMuaTruyen donmuatruyen = data.DonMuaTruyens.SingleOrDefault(n => n.MaDonHang == id);
                UpdateModel(donmuatruyen);
                data.SubmitChanges();
                return RedirectToAction("Index", "DonHang");
            }
        }
    }
}