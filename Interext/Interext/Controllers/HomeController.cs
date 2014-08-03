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
        public ActionResult Index(string FreeText, string locationSearchTextField, string DateOfTheEventFrom,
            string DateOfTheEventTo, string AgeOfParticipant, string Gender)
        {
            List<Event> model = null;

            bool isFreeText = !String.IsNullOrEmpty(FreeText);
            bool isLocation = !String.IsNullOrEmpty(locationSearchTextField);
            bool isFromDate = false;
            bool isToDate = false;
            bool isParticipantAge = false;

            DateTime DateFrom = DateTime.MinValue;
            if (!String.IsNullOrEmpty(DateOfTheEventFrom))
            {
                isFromDate = DateTime.TryParse(DateOfTheEventFrom, out DateFrom);
            }

            DateTime DateTo = DateTime.MaxValue;
            if (!String.IsNullOrEmpty(DateOfTheEventTo))
            {
                isToDate = DateTime.TryParse(DateOfTheEventTo, out DateTo);
            }

            int participantAge = 0;
            if (!String.IsNullOrEmpty(AgeOfParticipant))
            {
                isParticipantAge = int.TryParse(AgeOfParticipant, out participantAge);
            }

            if (ModelState.IsValid)
            {
                //if (Request.IsAjaxRequest())
                //{
                model = _db.Events.Where(x => (isFreeText ? x.Title.ToLower().Contains(FreeText.ToLower()) || x.Description.ToLower().Contains(FreeText.ToLower()) : true)
                    && (isLocation ? (x.Place.ToLower() == locationSearchTextField.ToLower()) : true)
                    && (isFromDate ? (x.DateTimeOfTheEvent >= DateFrom) : true)
                    && (isToDate ? (x.DateTimeOfTheEvent <= DateTo) : true)
                    && (isParticipantAge ? (x.AgeOfParticipantsMin <= participantAge) : true)
                    && (isParticipantAge ? (x.AgeOfParticipantsMax.HasValue? x.AgeOfParticipantsMax >= participantAge: true) : true)
                    && x.GenderParticipant == Gender
                    ).ToList(); // temp
                return PartialView("~/Views/Event/_EventsWall.cshtml", model);
                //}   
            }
            return View(model);
        }

        private void checkRenge(int x, int y, int a, int b)
        {
            
            int min = x;
            int max = y;
            if (a < x)
                min = a;
            if (b > y)
                max = b;

           

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