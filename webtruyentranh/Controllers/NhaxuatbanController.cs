using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class NhaxuatbanController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Nhaxuatban
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(data.NHAXUATBANs.ToList());
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhaxuatban =from nxb in data.NHAXUATBANs where nxb.MaNXB==id select nxb;
                return View(nhaxuatban.Single());
            }
        }
        [HttpGet]
        public ActionResult create()
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult create(NHAXUATBAN nhaxuatban)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.NHAXUATBANs.InsertOnSubmit(nhaxuatban);

                data.SubmitChanges();
                return RedirectToAction("Index", "Nhaxuatban");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhaxuatban = from nxb in data.NHAXUATBANs where nxb.MaNXB == id select nxb;
                return View(nhaxuatban.Single());
            }
        }
        [HttpPost,ActionName("Edit")]
        public ActionResult Capnhat(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                NHAXUATBAN nhaxuatban = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
                UpdateModel(nhaxuatban);
                data.SubmitChanges();
                return RedirectToAction("Index", "Nhaxuatban");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhaxuatban = from nxb in data.NHAXUATBANs where nxb.MaNXB == id select nxb;
                return View(nhaxuatban.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                NHAXUATBAN nhaxuatban = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
                data.NHAXUATBANs.DeleteOnSubmit(nhaxuatban);
                data.SubmitChanges();
                return RedirectToAction("Index", "Nhaxuatban");
            }
        }
    }
}