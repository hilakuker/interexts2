using Interext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interext.Controllers
{
    public class HomeController : Controller
    {
        //InterextDB _db = new InterextDB();
        private ApplicationDbContext _db = new ApplicationDbContext();
        public HomeController()
        {
                
        }
        public ActionResult Index()
        {
            //return View();
            //var model = _db.Events.Take(10);
                
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("_Restaurants", model);
            //}

            //return View(model);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}