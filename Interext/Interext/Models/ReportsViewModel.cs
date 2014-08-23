using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class ReportsViewModel
    {
        public int Id { get; set; }
        [Required]
        public string ReportedUrl { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        public bool Handled { get; set; }
    }
    public class ReportsIndexViewModel
    {
        public ReportsIndexViewModel()
        {
            Reports = new List<ReportsViewModel>();
        }
        public List<ReportsViewModel> Reports { get; set; }
    }
}