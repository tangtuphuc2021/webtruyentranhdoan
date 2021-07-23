using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
using PagedList;
namespace webtruyentranh.Controllers
{
    public class ChuonghinhController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Chuonghinh
        public ActionResult Index(int? page, string keyword)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                int pagesize = 5;
                int pagenum = (page ?? 1);
                if (!string.IsNullOrEmpty(keyword))
                {
                    TempData["kwd"] = keyword;
                    List<HinhAnh> ha = data.HinhAnhs.Where(n => n.TenTruyen.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(ha.OrderByDescending(n => n.MaHinhAnh).ToPagedList(pagenum, pagesize));
                }
                return View(data.HinhAnhs.OrderByDescending(n => n.MaHinhAnh).ToList().ToPagedList(pagenum, pagesize));
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
                List<HinhAnh> ha = data.HinhAnhs.Where(n => n.TenTruyen.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Index", ha.OrderByDescending(n => n.MaHinhAnh).ToPagedList(pagenum, pagesize));
            }
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var hinh = from h in data.HinhAnhs where h.MaHinhAnh == id select h;
                return View(hinh.Single());
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
        [ValidateInput(false)]
        public ActionResult Create(HinhAnh hinh, HttpPostedFileBase fileupload)
        {

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh truyện";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/ChapHinh"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    hinh.HaChap = fileName;
                    data.HinhAnhs.InsertOnSubmit(hinh);
                    data.SubmitChanges();
                }
                return RedirectToAction("Index", "Chuonghinh");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                HinhAnh hinh = data.HinhAnhs.SingleOrDefault(n => n.MaHinhAnh == id);

                if (hinh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(hinh);
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult CapNhat(int id, HttpPostedFileBase fileupload)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                HinhAnh hinh = data.HinhAnhs.SingleOrDefault(n => n.MaHinhAnh == id);

                if (fileupload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh truyện";
                    return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var filename = Path.GetFileName(fileupload.FileName);
                        var path = Path.Combine(Server.MapPath("~/ChapHinh"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            fileupload.SaveAs(path);
                        }
                        hinh.HaChap = filename;
                        UpdateModel(hinh);
                        data.SubmitChanges();

                    }
                    return RedirectToAction("Index");
                }


            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            HinhAnh hinh = data.HinhAnhs.SingleOrDefault(n => n.MaHinhAnh == id);
            ViewBag.MaHinhAnh = hinh.MaHinhAnh;
            if (hinh == null)
            {

                Response.StatusCode = 404;
                return null;
            }
            return View(hinh);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            HinhAnh hinh = data.HinhAnhs.SingleOrDefault(n => n.MaHinhAnh == id);
            ViewBag.MaHinhAnh = hinh.MaHinhAnh;
            if (hinh == null)
            {

                Response.StatusCode = 404;
                return null;
            }
            data.HinhAnhs.DeleteOnSubmit(hinh);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}