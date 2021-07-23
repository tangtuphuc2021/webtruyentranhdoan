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
    public class AdminController : Controller
    {
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {

            var tendn = collection["txtusername"];
            var matkhau = collection["txtpassadmin"];


            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";    
            }
            else
            {

                 Admin ad = data.Admins.SingleOrDefault(a => a.UserAdmin == tendn && a.PassAdmin == matkhau);
                if (ad != null)
                {
            
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");

                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Truyen(int ? page, string keyword)
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
                    List<Truyen> lstCus = data.Truyens.Where(n => n.TenTruyen.ToLower().Contains(keyword.ToLower())).ToList();
                    return View(lstCus.OrderByDescending(n => n.MaTruyen).ToPagedList(pagenum, pagesize));
                }
                return View(data.Truyens.OrderByDescending(n => n.MaTruyen).ToList().ToPagedList(pagenum ,pagesize));
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
                List<Truyen> lstCus = data.Truyens.Where(n => n.TenTruyen.ToLower().Contains(keyword.ToLower())).ToList();
                return View("Truyen", lstCus.OrderByDescending(n => n.MaTruyen).ToPagedList(pagenum, pagesize));
            }
        }




        public ActionResult chitietsach(int id)
        {

            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var truyen = from s in data.Truyens where s.MaTruyen == id select s;
                return View(truyen.SingleOrDefault());
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
                ViewBag.MaTL = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai");
                ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
                ViewBag.MaTinhTrang = new SelectList(data.TinhTrangs.ToList().OrderBy(n => n.MaTinhTrang), "MaTinhTrang", "TenTinhTrang");
                return View();
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Truyen truyen, HttpPostedFileBase fileupload)
        {
            ViewBag.MaTL = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            ViewBag.MaTinhTrang = new SelectList(data.TinhTrangs.ToList().OrderBy(n => n.MaTinhTrang), "MaTinhTrang", "TenTinhTrang");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/HinhAnh"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    truyen.Anhbia = fileName;
                    data.Truyens.InsertOnSubmit(truyen);
                    data.SubmitChanges();
                }
                return RedirectToAction("Truyen", "Admin");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Truyen truyen = data.Truyens.SingleOrDefault(n => n.MaTruyen == id);
            ViewBag.MaTruyen = truyen.MaTruyen;
            if (truyen == null)
            {

                Response.StatusCode = 404;
                return null;
            }
            return View(truyen);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            Truyen truyen = data.Truyens.SingleOrDefault(n => n.MaTruyen == id);
            ViewBag.MaTruyen = truyen.MaTruyen;
            if (truyen == null)
            {

                Response.StatusCode = 404;
                return null;
            }
            data.Truyens.DeleteOnSubmit(truyen);
            data.SubmitChanges();
            return RedirectToAction("Truyen");
        }





        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
               
               
                    Truyen truyen = data.Truyens.SingleOrDefault(n => n.MaTruyen == id);

                    ViewBag.MaTL = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai");
                    ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
                    ViewBag.MaTinhTrang = new SelectList(data.TinhTrangs.ToList().OrderBy(n => n.MaTinhTrang), "MaTinhTrang", "TenTinhTrang");
                    return View(truyen);
                
               
            }
        }
        [HttpPost, ActionName("Edit")]

        public ActionResult XacNhansua(int id, HttpPostedFileBase fileupload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Truyen truyen = data.Truyens.SingleOrDefault(n => n.MaTruyen == id);
                ViewBag.MaTL = new SelectList(data.TheLoais.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai",truyen.MaTL);
                ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB",truyen.MaNXB);
                ViewBag.MaTinhTrang = new SelectList(data.TinhTrangs.ToList().OrderBy(n => n.MaTinhTrang), "MaTinhTrang", "TenTinhTrang",truyen.MaTinhTrang);
                if (fileupload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else {
                    if (ModelState.IsValid)
                    {
                        var filename = Path.GetFileName(fileupload.FileName);
                        var path = Path.Combine(Server.MapPath("~/HinhAnh"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Ảnh đã có rồi";
                        }
                        else
                        {
                            fileupload.SaveAs(path);
                        }
                        truyen.Anhbia = filename;
                        UpdateModel(truyen);
                        data.SubmitChanges();

                    }
                    return RedirectToAction("Truyen");
                } 
                    
                  
                }
            }
        }

    }
