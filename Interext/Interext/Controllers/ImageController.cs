using Interext.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Interext.Controllers
{
    public class PartialViewController : Controller
    {
        public ImageModel model { get; set; }
        //
        // GET: /PartialView/
        [ChildActionOnly]
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            model = new ImageModel();
            if (user != null)
            {
                model.ImageUrl = user.ImageUrl;
            }
            return PartialView("~/Views/Shared/_LoginPartial.cshtml", model);
        }

        public ActionResult Test()
        {
            string fileExtenstion = Path.GetExtension(model.ImageUrl);
            return File(model.ImageUrl, fileExtenstion);
        }
	}
}