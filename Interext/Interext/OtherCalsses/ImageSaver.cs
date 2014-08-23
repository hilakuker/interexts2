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
using System.Drawing;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;


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
                    if (!Directory.Exists(SavePath))
                    {
                        Directory.CreateDirectory(SavePath);
                    }
                    // need to create folder for each user, the name of the folder is the id of the user
                    var path = Path.Combine(SavePath, fileName);
                    ImageUrl.SaveAs(path);
                    
                }
            }
        }

        internal static string SaveUser(string userId, HttpPostedFileBase ImageUrl, HttpServerUtilityBase Server)
        {

            string pathForToSave = Path.Combine(Server.MapPath("~/Content/images/users"), userId);
            string fileName = Path.GetFileName(ImageUrl.FileName);
            string pathForPicture = string.Format(@"/Content/images/users/{0}/{1}", userId, fileName);

            if (ImageUrl != null)
            {
                if (ImageUrl.ContentLength > 0)
                {
                    if (!Directory.Exists(pathForToSave))
                    {
                        Directory.CreateDirectory(pathForToSave);
                    }
                    // need to create folder for each user, the name of the folder is the id of the user
                    var path = Path.Combine(pathForToSave, fileName);
                    ResizeStreamAndSave(160, 160, ImageUrl.InputStream, path);

                }
            }
            Server.MapPath(pathForPicture);
            return pathForPicture;
        }

        internal static string SaveEvent(int eventId, HttpPostedFileBase ImageUrl, HttpServerUtilityBase Server)
        {

            string pathForToSave = Path.Combine(Server.MapPath("~/Content/images/events"), eventId.ToString());
            string fileName = Path.GetFileName(ImageUrl.FileName);
            string pathForPicture = string.Format(@"/Content/images/events/{0}/{1}", eventId, fileName);
            ImageSaver.SaveImage(ImageUrl, pathForToSave, fileName);
            Server.MapPath(pathForPicture);
            return pathForPicture;
        }

        internal static string SavePlaceImage(int placeID, HttpPostedFileBase ImageUrl, HttpServerUtilityBase Server)
        {
            string pathForToSave = Path.Combine(Server.MapPath("~/Content/images/Places"), placeID.ToString());
            string fileName = Path.GetFileName(ImageUrl.FileName);
            string pathForPicture = string.Format(@"/Content/images/Places/{0}/{1}", placeID, fileName);
            ImageSaver.SaveImage(ImageUrl, pathForToSave, fileName);
            Server.MapPath(pathForPicture);
            return pathForPicture;
        }
        internal static bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        internal static void ResizeStreamAndSave(int Width, int Height, Stream filePath, string outputPath)
        {

            var image = System.Drawing.Image.FromStream(filePath);
            var thumbnailBitmap = FixedSize(image, Width, Height, true);

            thumbnailBitmap.Save(outputPath, image.RawFormat);
            //thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }


        internal static System.Drawing.Bitmap FixedSize(System.Drawing.Image image, int Width, int Height, bool needToFill)
        {
            #region arithmerics
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale = 0;
            double nScaleW = 0;
            double nScaleH = 0;

            nScaleW = ((double)Width / (double)sourceWidth);
            nScaleH = ((double)Height / (double)sourceHeight);
            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                nScale = Math.Max(nScaleH, nScaleW);
                destY = (Height - sourceHeight * nScale) / 2;
                destX = (Width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            int destWidth = (int)Math.Round(sourceWidth * nScale);
            int destHeight = (int)Math.Round(sourceHeight * nScale);
            #endregion

            System.Drawing.Bitmap bmPhoto = null;
            try
            {
                bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, Width, Height), ex);
            }
            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

                return bmPhoto;
            }
        }

    }
}