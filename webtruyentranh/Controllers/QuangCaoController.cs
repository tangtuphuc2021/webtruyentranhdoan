using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;
namespace webtruyentranh.Controllers
{
    public class QuangCaoController : Controller
    {
        QuangCaodbContextDataContext data = new QuangCaodbContextDataContext();
        public ActionResult TinTuc()
        {
            var TinTuc = from tt in data.TinTucs select tt;
            return PartialView(TinTuc);
        }

        public ActionResult ThongTinTinTuc(int id)
        {
            var tintuc = from TT in data.TinTucs where TT.MaTinTuc == id select TT;
            return View(tintuc.Single());
        }
        // GET: QuangCao
        public ActionResult Index()
        {
            return View();
        }
    }
}