using Interext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Interext.OtherCalsses;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Interext.Controllers
{
    public class PlaceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private UserManager<ApplicationUser> UserManager { get; set; }

        public PlaceController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PlaceViewModel model, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                Place placeToCreate = new Place()
                {
                    CreatorUser = user,
                    Description = model.Description,
                    LocationAddress = model.LocationAddress,
                    Title = model.Title
                };
                db.Places.Add(placeToCreate);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var a = ex.EntityValidationErrors;
                }
                saveImage(ref placeToCreate, ImageUrl);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = placeToCreate.Id });
            }
            return View(model);
        }

        private void saveImage(ref Place placeToCreate, HttpPostedFileBase ImageUrl)
        {
            if (ImageUrl != null)
            {
                placeToCreate.ImageUrl = ImageSaver.SavePlaceImage(placeToCreate.Id, ImageUrl, Server);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place @place = db.Places.Find(id);
            if (@place == null)
            {
                return HttpNotFound();
            }

            PlaceViewModel placeToShow = new PlaceViewModel()
            {
                CreatorUser = @place.CreatorUser,
                ImageUrl = @place.ImageUrl,
                LocationAddress = @place.LocationAddress,
                Title = @place.Title,
                Description = @place.Description,
                Id = @place.Id
            };
            return View(placeToShow);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place @place = db.Places.Find(id);
            if (@place == null)
            {
                return HttpNotFound();
            }
            PlaceViewModel model = new PlaceViewModel()
            {
                Id = @place.Id,
                CreatorUser = @place.CreatorUser,
                Description = @place.Description,
                ImageUrl = @place.ImageUrl,
                Title = @place.Title
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlaceViewModel model, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                Place @place = db.Places.Find(model.Id);
                @place.CreatorUser = user;
                @place.Title = model.Title;
                if (model.ImageUrl != null)
                {
                    saveImage(ref @place, ImageUrl);
                }
                @place.Description = model.Description;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                }
                return RedirectToAction("Details", new { id = @place.Id });
            }
            return View(model);
        }
        public string CheckIfUserCanEditPlace(PlaceViewModel model)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id == model.CreatorUser.Id)
            {
                return string.Format("<a href=\"/Place/Edit?id=" + model.Id + "\">Edit place<a/>");
            }
            else
            {
                return "";
            }
        }
    }
}
