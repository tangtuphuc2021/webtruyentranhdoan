using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class LienHeController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();

        // GET: LienHe
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
                    List<LienHe> lh = data.LienHes.Where(n => n.HoTen.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(lh.OrderByDescending(n => n.MaLienHe).ToPagedList(pagenum, pagesize));
                }
                return View(data.LienHes.OrderByDescending(n => n.MaLienHe).ToList().ToPagedList(pagenum, pagesize));
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
                List<LienHe> lh = data.LienHes.Where(n => n.HoTen.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Index", lh.OrderByDescending(n => n.MaLienHe).ToPagedList(pagenum, pagesize));
            }
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var lienhe = from lh in data.LienHes where lh.MaLienHe == id select lh;
                return View(lienhe.Single());
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var LienHe = from lh in data.LienHes where lh.MaLienHe == id select lh;
                return View(LienHe.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                LienHe lienHe = data.LienHes.SingleOrDefault(n => n.MaLienHe == id);
                data.LienHes.DeleteOnSubmit(lienHe);
                data.SubmitChanges();
                return RedirectToAction("Index", "LienHe");
            }
        }



    }
}