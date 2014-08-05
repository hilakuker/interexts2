using Interext.OtherCalsses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Interext.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ApplicationUser CreatorUser { get; set; }

        [Display(Name = "Place")]
        public string Place { get; set; }
        public double PlaceLongitude { get; set; }
        public double PlaceLatitude { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [EventTimeValidation("DateTimeOfTheEvent")]
        public DateTime DateTimeOfTheEvent { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date is required")]
        public DateTime DateOfTheEvent { get; set; }
        [Required(ErrorMessage = "Time of the event hour is required")]
        public string HourTimeOfTheEvent { get; set; }

        [Required(ErrorMessage = "Time of the event minute is required")]
        public string MinuteTimeOfTheEvent { get; set; }
        public string DateOfTheEventNoYear { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public ICollection<UserProfile> UsersAttending { get; set; }
        public ICollection<UserProfile> UsersInvited { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public string BackroundColor { get; set; }

        public string BackroundColorOpacity { get; set; }
        public string SideOfText { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]

        public Dictionary<string, bool> SideOfTextOptions { get; set; }

        public int? NumOfParticipantsMin { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [Range(1, 100)]
        [MinMaxValidation("NumOfParticipantsMin", AllowEquality = true)]
        public int? NumOfParticipantsMax { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [Range(1, 100)]
        public int? AgeOfParticipantsMin { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [MinMaxValidation("AgeOfParticipantsMin", AllowEquality = true)]
        public int? AgeOfParticipantsMax { get; set; }
        public string GenderParticipant { get; set; }

        public Dictionary<string, bool> GenderParticipantOptions { get; set; }

        public ICollection<Interest> AllInterests { get; set; }
    }


}