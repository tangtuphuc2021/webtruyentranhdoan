using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using webtruyentranh.Models;

namespace webtruyentranh.Controllers
{
    public class ChuongController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();

        // GET: Chuong
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
                    List<Chap> chap = data.Chaps.Where(n => n.TenTruyen.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(chap.OrderByDescending(n => n.MaChap).ToPagedList(pagenum, pagesize));
                }
                return View(data.Chaps.OrderByDescending(n => n.MaChap).ToList().ToPagedList(pagenum, pagesize));
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
                List<Chap> chap = data.Chaps.Where(n => n.TenTruyen.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Index", chap.OrderByDescending(n => n.MaChap).ToPagedList(pagenum, pagesize));
            }
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var chap = from c in data.Chaps where c.MaChap == id select c;
                return View(chap.Single());
            }
        }
        public ActionResult Create()
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(Chap chap)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.Chaps.InsertOnSubmit(chap);

                data.SubmitChanges();
                return RedirectToAction("Index", "Chuong");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var chuong = from c in data.Chaps where c.MaChap == id select c;
                return View(chuong.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult CapNhat(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Chap chap = data.Chaps.SingleOrDefault(n => n.MaChap == id);
                UpdateModel(chap);
                data.SubmitChanges();
                return RedirectToAction("Index", "Chuong");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var chap = from c in data.Chaps where c.MaChap == id select c;
                return View(chap.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Chap chap = data.Chaps.SingleOrDefault(n => n.MaChap == id);
                data.Chaps.DeleteOnSubmit(chap);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
        }
    }
}