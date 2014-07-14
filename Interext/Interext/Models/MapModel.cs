using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class MapModel
    {
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}