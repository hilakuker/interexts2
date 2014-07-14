using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Interext.Models;
using System.Web.Security;
using Microsoft.Owin.Security.Facebook;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using Interext.OtherCalsses;


namespace Interext.OtherCalsses
{
    public static class ImageSaver
    {
        internal static void SaveImage(HttpPostedFileBase ImageUrl, string SavePath, string fileName)
        {
            if (ImageUrl != null)
            {
                if (ImageUrl.ContentLength > 0)
                {
                    Directory.CreateDirectory(SavePath);
                    // need to create folder for each user, the name of the folder is the id of the user
                    var path = Path.Combine(SavePath, fileName);
                    ImageUrl.SaveAs(path);

                }
            }
        }
    }
}