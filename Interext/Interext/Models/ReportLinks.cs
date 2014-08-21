using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class ReportLinks
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ReportedUrl { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }
        public e_reportStatus ReportStatus { get; set; }
    }
    public enum e_reportStatus
    {
       ToBeHandled,
        Handled
    }
}