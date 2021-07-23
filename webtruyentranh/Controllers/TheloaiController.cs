using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class TheloaiController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Theloai
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
                    List<TheLoai> tl = data.TheLoais.Where(n => n.TenTheLoai.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(tl.OrderByDescending(n => n.MaTL).ToPagedList(pagenum, pagesize));
                }
                return View(data.TheLoais.OrderByDescending(n => n.MaTL).ToList().ToPagedList(pagenum, pagesize));
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
                List<TheLoai> tl = data.TheLoais.Where(n => n.TenTheLoai.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Index", tl.OrderByDescending(n => n.MaTL).ToPagedList(pagenum, pagesize));
            }
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var theloai = from tl in data.TheLoais where tl.MaTL == id select tl;
                return View(theloai.Single());
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
        public ActionResult create(TheLoai theloai)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.TheLoais.InsertOnSubmit(theloai);

                data.SubmitChanges();
                return RedirectToAction("Index", "Theloai");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var theloai = from tl in data.TheLoais where tl.MaTL == id select tl;
                return View(theloai.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                TheLoai theloai = data.TheLoais.SingleOrDefault(n => n.MaTL == id);
                UpdateModel(theloai);
                data.SubmitChanges();
                return RedirectToAction("Index", "Theloai");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var theloai = from tl in data.TheLoais where tl.MaTL == id select tl;
                return View(theloai.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                TheLoai theloai = data.TheLoais.SingleOrDefault(n => n.MaTL == id);
                data.TheLoais.DeleteOnSubmit(theloai);
                data.SubmitChanges();
                return RedirectToAction("Index", "Theloai");
            }
        }
    }
}