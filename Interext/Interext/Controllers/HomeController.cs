using Interext.Models;
using Interext.OtherCalsses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        private UserManager<ApplicationUser> UserManager { get; set; }
        public HomeController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this._db));

        }

        public ActionResult Index(string searchword, string advanced)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                var user = UserManager.FindById(userId);
                //List<Event> model = GetEventForUser(searchword, advanced);
                ViewBag.CurrentUser = user;
                //ViewBag.SearchWord = searchword;
                //ViewBag.AllInterests = InterestsFromObjects.InitAllInterests(_db);
                //return View(model);
            }
            ViewBag.SearchWord = searchword;
            ViewBag.AllInterests = InterestsFromObjects.InitAllInterests(_db);
            List<Event> model = new List<Event>();
            return View(model);
            //else return RedirectToAction("Login", "Account");
        }

        public bool Report()
        {
            _db.ReportedUrl.Add(new ReportLinks()
            {
                ReportedUrl = Request.UrlReferrer.ToString(),
                CreatedTime = DateTime.Now,
                ReportStatus = e_reportStatus.ToBeHandled
            });
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        //private ActionResult GetEventsForUser(string searchword, string advanced)
        public ActionResult GetEventsForUser(string searchword, string advanced, string myLocationLatitude, string myLocationLongitude)
        {



            bool isSearchWord = true;
            List<Event> model = null;
            if (String.IsNullOrEmpty(searchword) || advanced == "1")
            {
                isSearchWord = false;
            }
            if (isSearchWord)
            {
                model = _db.Events.Where(x => (x.EventStatus == e_EventStatus.Active)
                        && (isSearchWord ? (x.Title.ToLower().Contains(searchword.ToLower()) ||
                            x.Description.ToLower().Contains(searchword.ToLower())) : true)).ToList();
            }
            else
            {
                bool googleCoordinatesRecieved = true;
                double longitude = 0;
                double latitude = 0;
                if (!double.TryParse(myLocationLatitude, out latitude) || !double.TryParse(myLocationLongitude, out longitude))
                {
                    googleCoordinatesRecieved = false;
                }

                if (User.Identity.IsAuthenticated)
                {
                    string userId = User.Identity.GetUserId();
                    ApplicationUser user = UserManager.FindById(userId);
                    if (!googleCoordinatesRecieved)
                    {
                        longitude = user.PlaceLongitude;
                        latitude = user.PlaceLatitude;
                    }
                    model = GetEventsForLoggedInUser(model, longitude, latitude, user);
                }
                else
                {
                    if (googleCoordinatesRecieved)
                    {
                        GetEventsForUserOnlyByLocation(ref model, longitude, latitude, "Location");
                    }
                    else
                    {
                        model = _db.Events.Where(x => x.EventStatus == e_EventStatus.Active).Take(8).ToList();
                    }
                }
                return PartialView("~/Views/Event/_EventsWall.cshtml", model);
            }
            return View(model);
        }

        private List<Event> GetEventsForLoggedInUser(List<Event> model, double longitude, double latitude, ApplicationUser user)
        {
            var numberOfAllEvents = _db.Events.Where(x => x.EventStatus == e_EventStatus.Active).Count();
            int[] radiuses = new int[] { 2, 5, 10, 15, 20, 50, 100, 200 };

            bool stopSearch = false;
            bool searchAgain = false;
            int radiusIndex = 0;
            int radius;
            while (!stopSearch)
            {
                radius = radiuses[radiusIndex];
                radius = radius * 1000;
                radius += 500;// adding 500 meters to the radius
                model = SearchForEvents("", user.HomeAddress, "", true, false, true, true, false, true, true, false, radius, longitude, latitude, user.Interests.ToList(), DateTime.Today, DateTime.Today, calculateAge((DateTime)user.BirthDate));
                radiusIndex++;
                if (model.Count() >= 8)
                {
                    stopSearch = true;
                }
                if (radiusIndex == radiuses.Count())
                {
                    stopSearch = true;
                    searchAgain = true;
                }
            }
            if (searchAgain)
            {
                stopSearch = false;
                searchAgain = false;
                radiusIndex = 0;
                while (!stopSearch)
                {
                    radius = radiuses[radiusIndex];
                    radius = radius * 1000;
                    radius += 500;// adding 500 meters to the radius
                    model = SearchForEvents("", user.HomeAddress, "", true, false, true, true, false, true, false, false, radiuses[radiusIndex], longitude, latitude, null, DateTime.Today, DateTime.Today, calculateAge((DateTime)user.BirthDate));
                    radiusIndex++;
                    if (model.Count() >= 8)
                    {
                        stopSearch = true;
                    }
                    if (radiusIndex == radiuses.Count())
                    {
                        stopSearch = true;
                        searchAgain = true;
                    }
                }
            }
            if (searchAgain)
            {
                GetEventsForUserOnlyByLocation(ref model, longitude, latitude, user.HomeAddress);
            }
            return model;
        }

        private void GetEventsForUserOnlyByLocation(ref List<Event> model, double longitude, double latitude, string location)
        {
            var numberOfAllEvents = _db.Events.Where(x => x.EventStatus == e_EventStatus.Active).Count();
            int[] radiuses = new int[] { 2, 5, 10, 15, 20, 50, 100, 200 };
            int radius;
            bool stopSearch = false;
            int radiusIndex = 0;
            while (!stopSearch)
            {
                radius = radiuses[radiusIndex];
                radius = radius * 1000;
                radius += 500;// adding 500 meters to the radius
                model = SearchForEvents("", location, "", true, false, true, true, false, false, false, false, radiuses[radiusIndex], longitude, latitude, null, DateTime.Today, DateTime.Today, 1);
                radiusIndex++;
                if (model.Count() >= 8)
                {
                    stopSearch = true;
                }
                if (radiusIndex == radiuses.Count())
                {
                    stopSearch = true;
                    model = _db.Events.Where(x => x.EventStatus == e_EventStatus.Active).Take(8).ToList();
                }
            }
        }
        private int calculateAge(DateTime birthdate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthdate.Year;
            if (birthdate > today.AddYears(-age))
            { age--; }
            return age;
        }
        private List<Interest> GetSelectedInterests(string selectedInterests)
        {
            List<Interest> interests = new List<Interest>();
            foreach (string item in selectedInterests.Split(','))
            {
                if (item != "")
                {
                    int id;
                    if (int.TryParse(item, out id))
                    {
                        Interest interest = _db.Interests.SingleOrDefault(x => x.Id == id);
                        interests.Add(interest);
                    }
                }
            }
            return interests;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string FreeText,
            string locationSearchTextField, string PlaceLongitude, string PlaceLatitude,
            string radiusOfTheLocation, string selectedInterests, string DateOfTheEventFrom, string DateOfTheEventTo,
            string AgeOfParticipant, string Gender
             )
        {



            List<Event> model = null;
            bool searchAccordingToRadius = false;
            bool isFreeText = !String.IsNullOrEmpty(FreeText.Trim());
            bool isLocation = !String.IsNullOrEmpty(locationSearchTextField);
            bool isFromDate = false;
            bool isToDate = false;
            bool isParticipantAge = false;
            bool isInterests = false;
            bool isGenderNotBoth = false;
            double radius = 0;
            double longitude = 0;
            double latitude = 0;

            List<Interest> interests = GetSelectedInterests(selectedInterests);
            if (interests != null && interests.Count > 0)
            {
                isInterests = true;
            }

            if (radiusOfTheLocation != "0" && locationSearchTextField != "")
            {
                if (double.TryParse(radiusOfTheLocation, out radius) && double.TryParse(PlaceLongitude, out longitude) && double.TryParse(PlaceLatitude, out latitude))
                {
                    searchAccordingToRadius = true;
                    radius = radius * 1000;
                    radius += 500;// adding 500 meters to the radius
                }
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

            if (Gender != "Both")
            {
                isGenderNotBoth = true;
            }

            if (!isFreeText && !isLocation && !isFromDate && !isToDate && !isParticipantAge && !isGenderNotBoth && !isInterests)
            {
                Response.Redirect("/");
            }
            else if (ModelState.IsValid)
            {

                model = SearchForEvents(FreeText, locationSearchTextField, Gender, searchAccordingToRadius, isFreeText, isLocation, isFromDate, isToDate, isParticipantAge, isInterests, isGenderNotBoth, radius, longitude, latitude, interests, DateFrom, DateTo, participantAge);
                return PartialView("~/Views/Event/_EventsWall.cshtml", model);
                //}   
            }
            return View(model);
        }

        private List<Event> SearchForEvents(string FreeText, string locationSearchTextField, string Gender, bool searchAccordingToRadius, bool isFreeText, bool isLocation, bool isFromDate, bool isToDate, bool isParticipantAge, bool isInterests, bool isGenderNotBoth, double radius, double longitude, double latitude, List<Interest> interests, DateTime DateFrom, DateTime DateTo, int participantAge)
        {
            List<Event> eventList = _db.Events.ToList();
            List<Event> model = eventList.Where(
                x => (x.EventStatus != e_EventStatus.Deleted)
                && (isFreeText ? (x.Title.ToLower().Contains(FreeText.Trim().ToLower()) ||
                (!String.IsNullOrEmpty(x.Description) ? x.Description.ToLower().Contains(FreeText.Trim().ToLower()) : false)) : true)
                && (isLocation && !searchAccordingToRadius ? (x.Place.ToLower() == locationSearchTextField.ToLower()) : true)
                && (isLocation && searchAccordingToRadius ? (calulateDistance(x, latitude, longitude) <= radius) : true)
                && (isFromDate ? (x.DateTimeOfTheEvent.Date >= DateFrom) : true)
                && (isToDate ? (x.DateTimeOfTheEvent.Date <= DateTo) : true)
                && (isParticipantAge ?
                    ((x.AgeOfParticipantsMin.HasValue && x.AgeOfParticipantsMax.HasValue) ? (x.AgeOfParticipantsMin <= participantAge && participantAge <= x.AgeOfParticipantsMax) :
                    ((x.AgeOfParticipantsMin.HasValue && !x.AgeOfParticipantsMax.HasValue) ? (x.AgeOfParticipantsMin <= participantAge) :
                    (!x.AgeOfParticipantsMin.HasValue && x.AgeOfParticipantsMax.HasValue ? (participantAge <= x.AgeOfParticipantsMax) : true
                    ))) : true)
                && (isGenderNotBoth ? x.GenderParticipant == Gender : true)
                && (isInterests ? x.Interests.Intersect(interests).Count() > 0 : true)
                ).OrderBy(x => x.DateTimeOfTheEvent).ToList(); // temp
            return model;
        }

        private double calulateDistance(Event x, double PlaceLatitude, double PlaceLongitude)
        {
            var locA = new GeoCoordinate(PlaceLatitude, PlaceLongitude);
            var locB = new GeoCoordinate(x.PlaceLatitude, x.PlaceLongitude);
            double distance = locA.GetDistanceTo(locB);
            return (distance);
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