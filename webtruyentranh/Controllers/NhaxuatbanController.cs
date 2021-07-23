using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
using PagedList;
namespace webtruyentranh.Controllers
{
    public class NhaxuatbanController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Nhaxuatban
        public ActionResult Index(int? page, string keyword)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                int pagesize = 4;
                int pagenum = (page ?? 1);
                if (!string.IsNullOrEmpty(keyword))
                {
                    TempData["kwd"] = keyword;
                    List<NHAXUATBAN> nxb = data.NHAXUATBANs.Where(n => n.TenNXB.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(nxb.OrderByDescending(n => n.MaNXB).ToPagedList(pagenum, pagesize));
                }
                return View(data.NHAXUATBANs.OrderByDescending(n => n.MaNXB).ToList().ToPagedList(pagenum, pagesize));
            }
        }

        [HttpPost]
        public ActionResult TimKiem(string keyword)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                int pagesize = 4;
                int pagenum = 1;

                TempData["kwd"] = keyword;
                List<NHAXUATBAN> nxb = data.NHAXUATBANs.Where(n => n.TenNXB.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Index", nxb.OrderByDescending(n => n.MaNXB).ToPagedList(pagenum, pagesize));
            }
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