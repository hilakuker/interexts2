using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class PlaceViewModel
    {
        public ApplicationUser CreatorUser { get; set; }
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Title Cannot Remain Empty")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        [Required(ErrorMessage = "Address Cannot Remain Empty")]
        public string LocationAddress { get; set; }
    }
}