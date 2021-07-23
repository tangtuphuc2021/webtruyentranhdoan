using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webtruyentranh.Models;

namespace webtruyentranh.Controllers
{
    public class ChapTruyenController : Controller
    {
        // GET: ChapTruyen
        dbQlwebtruyenDataContext data = new dbQlwebtruyenDataContext();
        public ActionResult DSChuong(int id)
        {
            var chap = from s in data.Chaps where s.MaTruyen == id select s;

            return View(chap);
        }
        public ActionResult ChuongHinh(int id)
        {
            var hinh = from h in data.HinhAnhs where h.MaChap == id select h;
            return View(hinh);
        }
        
    }
}