using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class DonmuatruyenController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Donmuatruyen
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(data.DonMuaTruyens.ToList());
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var donmuatruyen =from dmt in data.DonMuaTruyens where dmt.MaDonHang==id select dmt;
                return View(donmuatruyen.Single());
            }
        }
        public ActionResult Create()
        {

            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                
                ViewBag.Docgia = new SelectList(data.DocGias.ToList().OrderBy(n => n.MaDG), "MaDG", "HoTen");
                return View();
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult create(DonMuaTruyen donmuatruyen)
        {
            ViewBag.Docgia = new SelectList(data.DocGias.ToList().OrderBy(n => n.MaDG), "MaDG", "HoTen");
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.DonMuaTruyens.InsertOnSubmit(donmuatruyen);
                data.SubmitChanges();
                return RedirectToAction("Index", "Donmuatruyen");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var donmuatruyen = from dmt in data.DonMuaTruyens where dmt.MaDonHang == id select dmt;
                return View(donmuatruyen.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DonMuaTruyen dmt = data.DonMuaTruyens.SingleOrDefault(n => n.MaDonHang == id);
                data.DonMuaTruyens.DeleteOnSubmit(dmt);
                data.SubmitChanges();
                return RedirectToAction("Index", "Donmuatruyen");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
           
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var donmuatruyen = from dmt in data.DonMuaTruyens where dmt.MaDonHang == id select dmt;
                return View(donmuatruyen.Single());
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
                return RedirectToAction("Index", "Donmuatruyen");
            }
        }
    }
}