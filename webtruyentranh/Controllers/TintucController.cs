using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace webtruyentranh.Controllers
{
    public class TintucController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();

        // GET: Tintuc
        public ActionResult Index(int ? page,string keyword)
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
                    List<TinTuc> dg = data.TinTucs.Where(n => n.TacGia.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(dg.OrderByDescending(n => n.MaTinTuc).ToPagedList(pagenum, pagesize));
                }
                return View(data.TinTucs.OrderByDescending(n => n.MaTinTuc).ToList().ToPagedList(pagenum, pagesize));
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
                List<TinTuc> dg = data.TinTucs.Where(n => n.TacGia.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Index", dg.OrderByDescending(n => n.MaTinTuc).ToPagedList(pagenum, pagesize));
            }
        }
        public ActionResult Details(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var TinTuc = from s in data.TinTucs where s.MaTinTuc == id select s;
                return View(TinTuc.SingleOrDefault());
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


                return View();
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TinTuc tt, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUploadd,
            HttpPostedFileBase fileUploaddd)
        {
            if (Session["Taikhoanadmin"] == null)

                return RedirectToAction("Login", "Admin");
            else
            {
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh truyện";
                    return View();
                }
                else
                {

                    if (ModelState.IsValid)
                    {
                        var filename = Path.GetFileName(fileUpload.FileName);
                        var filename1 = Path.GetFileName(fileUploadd.FileName);
                        var filename2 = Path.GetFileName(fileUploaddd.FileName);


                        var path = Path.Combine(Server.MapPath("~/HinhAnh"), filename);
                        var path1 = Path.Combine(Server.MapPath("~/HinhAnh"), filename1);
                        var path2 = Path.Combine(Server.MapPath("~/HinhAnh"), filename2);


                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Hình này đã tồn tại";
                        }
                        else if (System.IO.File.Exists(path1))
                        {
                            ViewBag.Thongbao = "Hình này đã tồn tại";

                        }
                        else if (System.IO.File.Exists(path2))
                        {
                            ViewBag.Thongbao = "Hình này đã tồn tại";

                        }
                        else
                            //úp hình lên server
                            fileUpload.SaveAs(path);
                        fileUploadd.SaveAs(path1);
                        fileUploaddd.SaveAs(path2);


                        tt.HinhAnh = filename;
                        tt.HinhAnh2 = filename1;
                        tt.HinhAnh3 = filename2;

                        data.TinTucs.InsertOnSubmit(tt);
                        data.SubmitChanges();

                       
                    }
                    return RedirectToAction("Index", "TinTuc");
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                TinTuc tintuc = data.TinTucs.SingleOrDefault(n => n.MaTinTuc == id);

                if (tintuc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(tintuc);
            }
        }
        [HttpPost, ActionName("Edit")]

        public ActionResult XacNhansua(int id, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadd,
            HttpPostedFileBase fileUploaddd)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                TinTuc tt = data.TinTucs.SingleOrDefault(n => n.MaTinTuc == id);

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
                        var filename1 = Path.GetFileName(fileUploadd.FileName);
                        var filename2 = Path.GetFileName(fileUploaddd.FileName);
                        var path = Path.Combine(Server.MapPath("~/HinhAnh"), filename);
                        var path1 = Path.Combine(Server.MapPath("~/HinhAnh"), filename1);
                        var path2 = Path.Combine(Server.MapPath("~/HinhAnh"), filename2);

                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Hình này đã tồn tại";
                        }
                        else if (System.IO.File.Exists(path1))
                        {
                            ViewBag.Thongbao = "Hình này đã tồn tại";

                        }
                        else if (System.IO.File.Exists(path2))
                        {
                            ViewBag.Thongbao = "Hình này đã tồn tại";

                        }
                        else
                            //úp hình lên server
                            fileupload.SaveAs(path);
                        fileUploadd.SaveAs(path1);
                        fileUploaddd.SaveAs(path2);


                        tt.HinhAnh = filename;
                        tt.HinhAnh2 = filename1;
                        tt.HinhAnh3 = filename2;
                        UpdateModel(tt);
                        data.SubmitChanges();

                    }
                    return RedirectToAction("Index");
                }

            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var TinTuc = from lh in data.TinTucs where lh.MaTinTuc == id select lh;
                return View(TinTuc.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                TinTuc TinTuc = data.TinTucs.SingleOrDefault(n => n.MaTinTuc == id);
                data.TinTucs.DeleteOnSubmit(TinTuc);
                data.SubmitChanges();
                return RedirectToAction("Index", "TinTuc");
            }
        }

    }
}
