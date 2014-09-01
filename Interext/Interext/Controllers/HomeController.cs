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
                List<Event> model = GetEventForUser(user, searchword, advanced);
                ViewBag.CurrentUser = user;
                ViewBag.SearchWord = searchword;
                ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(user.Interests, _db);
                return View(model);
            }
            else return RedirectToAction("Login", "Account");
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
        private List<Event> GetEventForUser(ApplicationUser user, string searchword, string advanced)
        {
            bool isSearchWord = true;
            if (String.IsNullOrEmpty(searchword) || advanced == "1")
            {
                isSearchWord = false;
            }

            return _db.Events.Where(x => (x.EventStatus != e_EventStatus.Deleted)
                    && (isSearchWord ? (x.Title.ToLower().Contains(searchword.ToLower()) ||
                        x.Description.ToLower().Contains(searchword.ToLower())) : true)).ToList();
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
            bool isFreeText = !String.IsNullOrEmpty(FreeText);
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

            if (radiusOfTheLocation != "The exact location")
            {

                string temp = radiusOfTheLocation.Replace("km", "");
                if (double.TryParse(temp, out radius) && double.TryParse(PlaceLongitude, out longitude) && double.TryParse(PlaceLatitude, out latitude))
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

            if (ModelState.IsValid)
            {
                List<Event> eventList = _db.Events.ToList();
                model = eventList.Where(
                    x => (x.EventStatus != e_EventStatus.Deleted)
                    && (isFreeText ? (x.Title.ToLower().Contains(FreeText.ToLower()) ||
                        x.Description.ToLower().Contains(FreeText.ToLower())) : true)
                    && (isLocation && !searchAccordingToRadius ? (x.Place.ToLower() == locationSearchTextField.ToLower()) : true)
                    && (isLocation && searchAccordingToRadius ? (calulateDistance(x, latitude, longitude) <= radius) : true)
                    && (isFromDate ? (x.DateTimeOfTheEvent.Date >= DateFrom) : true)
                    && (isToDate ? (x.DateTimeOfTheEvent.Date <= DateTo) : true)
                    && (isParticipantAge ? (x.AgeOfParticipantsMin <= participantAge) : true)
                    && (isParticipantAge ? (x.AgeOfParticipantsMax.HasValue ? x.AgeOfParticipantsMax >= participantAge : true) : true)
                    && (isGenderNotBoth ? x.GenderParticipant == Gender : true)
                    && (isInterests ? x.Interests.Intersect(interests).Count() > 0 : true)
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