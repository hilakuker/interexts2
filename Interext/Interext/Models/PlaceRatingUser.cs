using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class PlaceRatingUser
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Place RatedPlace { get; set; }
        public ApplicationUser RatingUser { get; set; }
        public int Rating { get; set; }
    }
}