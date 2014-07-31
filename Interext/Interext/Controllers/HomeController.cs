using Interext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<Event> model = _db.Events.ToList();

            if (Request.IsAjaxRequest())
            {
                model = _db.Events.Take(1).ToList(); // temp
                return PartialView("~/Views/Event/_EventsWall.cshtml", model);
            }

            //return View(model);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string FreeText)
        {
            List<Event> model=null;
            if (ModelState.IsValid)
            {
                //if (Request.IsAjaxRequest())
                //{
                    model = _db.Events.Where(x=>x.Title.ToLower().Contains(FreeText.ToLower())).ToList(); // temp
                    return PartialView("~/Views/Event/_EventsWall.cshtml", model);
                //}   
            }
            return View(model);
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