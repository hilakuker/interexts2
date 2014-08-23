using Interext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interext.Controllers
{
    [Authorize(Roles = "Admin, AnotherRole")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var notHandledReports = db.ReportedUrl.Where(x => x.ReportStatus == e_reportStatus.ToBeHandled).ToList();
            var notHandledReportsView = new List<ReportsViewModel>();
            foreach (ReportLinks report in notHandledReports)
            {
                notHandledReportsView.Add(new ReportsViewModel() 
                { CreatedTime = report.CreatedTime, Handled = false, ReportedUrl = report.ReportedUrl, Id = report.Id});
            }
            return View(notHandledReportsView);
        }
        [HttpPost]
        public ActionResult Index(List<ReportsViewModel> model)
        {
            foreach (ReportsViewModel report in model)
            {
                if (report.Handled)
                {
                    var reportInDb = db.ReportedUrl.Find(report.Id);
                    reportInDb.ReportStatus = e_reportStatus.Handled;
                }
            }
            db.SaveChanges();
            return View();
        }
    }
}