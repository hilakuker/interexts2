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
            ReportsIndexViewModel notHandledReportsView = getNotReportedLink();
            return View(notHandledReportsView);
        }

        [HttpPost]
        public ActionResult Index(ReportsIndexViewModel model)
        {
            foreach (ReportsViewModel report in model.Reports)
            {
                if (report.Handled)
                {
                    var reportInDb = db.ReportedUrl.Find(report.Id);
                    reportInDb.ReportStatus = e_reportStatus.Handled;
                }
            }
            db.SaveChanges();
            return View(getNotReportedLink());
        }
        private ReportsIndexViewModel getNotReportedLink()
        {
            var notHandledReports = db.ReportedUrl.Where(x => x.ReportStatus == e_reportStatus.ToBeHandled).ToList();
            ReportsIndexViewModel notHandledReportsView = new ReportsIndexViewModel();
            foreach (ReportLinks report in notHandledReports)
            {
                notHandledReportsView.Reports.Add(
                    new ReportsViewModel() { CreatedTime = report.CreatedTime, Handled = false, ReportedUrl = report.ReportedUrl, Id = report.Id });
            }
            return notHandledReportsView;
        }
    }
}