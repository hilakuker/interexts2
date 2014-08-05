using Interext.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;
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
            List<Event> model = _db.Events.ToList();
            if (Request.IsAjaxRequest())
            {
                model = _db.Events.Take(1).ToList(); // temp
                return PartialView("~/Views/Event/_EventsWall.cshtml", model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string FreeText, string locationSearchTextField, string DateOfTheEventFrom,
            string DateOfTheEventTo, string AgeOfParticipant, string Gender, string radiusOfTheLocation,
            double PlaceLongitude, double PlaceLatitude)
        {
            List<Event> model = null;
            bool searchAccordingToRadius = false;
            bool isFreeText = !String.IsNullOrEmpty(FreeText);
            bool isLocation = !String.IsNullOrEmpty(locationSearchTextField);
            bool isFromDate = false;
            bool isToDate = false;
            bool isParticipantAge = false;
                double radius = 0;
            

            if (radiusOfTheLocation != "The exact location")
            {
                searchAccordingToRadius = true;
                string temp = radiusOfTheLocation.Replace("km", "");
                radius = double.Parse(temp);
            }
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
                //if (isLocation && searchAccordingToRadius)
                //{
                //    model = _db.Events.ToList();
                //    model.Where(x => Math.Sqrt(((Math.Pow((x.PlaceLatitude - PlaceLatitude), 2) + 
                //        Math.Pow((x.PlaceLongitude - PlaceLongitude), 2)))) < radius);
                //}
                List<Event> eventList = _db.Events.ToList();
                model = eventList.Where(
                    x => (isFreeText ? x.Title.ToLower().Contains(FreeText.ToLower()) || 
                        x.Description.ToLower().Contains(FreeText.ToLower()) : true)
                    && (isLocation ? (x.Place.ToLower() == locationSearchTextField.ToLower()) : true)
                    && (isLocation && searchAccordingToRadius ? (calulateDistance(x, PlaceLatitude, PlaceLongitude) < radius)
                    : true)
                    && (isFromDate ? (x.DateTimeOfTheEvent.Date >= DateFrom) : true)
                    && (isToDate ? (x.DateTimeOfTheEvent.Date <= DateTo) : true)
                    && (isParticipantAge ? (x.AgeOfParticipantsMin <= participantAge) : true)
                    && (isParticipantAge ? (x.AgeOfParticipantsMax.HasValue? x.AgeOfParticipantsMax >= participantAge: true) : true)
                    && x.GenderParticipant == Gender
                    ).ToList(); // temp
                return PartialView("~/Views/Event/_EventsWall.cshtml", model);
                //}   
            }
            return View(model);
        }

        private double calulateDistance(Event x, double PlaceLatitude, double PlaceLongitude)
        {
            var locA = new GeoCoordinate(PlaceLatitude, PlaceLongitude);
            var locB = new GeoCoordinate(x.PlaceLatitude, x.PlaceLongitude);
            return (locA.GetDistanceTo(locB));
        }

        private void checkRange(int x, int y, int a, int b)
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