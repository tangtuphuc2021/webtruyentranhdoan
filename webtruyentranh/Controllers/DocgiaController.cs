using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;

using PagedList;
namespace webtruyentranh.Controllers
{
    public class DocgiaController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Docgia
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
                    List<DocGia> dg = data.DocGias.Where(n => n.HoTen.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(dg.OrderByDescending(n => n.MaDG).ToPagedList(pagenum, pagesize));
                }
                return View(data.DocGias.OrderByDescending(n => n.MaDG).ToList().ToPagedList(pagenum, pagesize));
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
                List<DocGia> dg = data.DocGias.Where(n => n.HoTen.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Index", dg.OrderByDescending(n => n.MaDG).ToPagedList(pagenum, pagesize));
            }
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var docgia = from dg in data.DocGias where dg.MaDG == id select dg;
                return View(docgia.Single());
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
        public ActionResult create(DocGia docgia)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.DocGias.InsertOnSubmit(docgia);

                data.SubmitChanges();
                return RedirectToAction("Index", "Docgia");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var docgia = from dg in data.DocGias where dg.MaDG == id select dg;
                return View(docgia.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DocGia docgia = data.DocGias.SingleOrDefault(n => n.MaDG == id);
                UpdateModel(docgia);
                data.SubmitChanges();
                return RedirectToAction("Index", "Docgia");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var docgia = from dg in data.DocGias where dg.MaDG == id select dg;
                return View(docgia.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DocGia docgia = data.DocGias.SingleOrDefault(n => n.MaDG == id);
                data.DocGias.DeleteOnSubmit(docgia);
                data.SubmitChanges();
                return RedirectToAction("Index", "Docgia");
            }
        }
    }
}