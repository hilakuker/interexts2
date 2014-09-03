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

        [Required(ErrorMessage = " ")]
        public string Title { get; set; }

        public string Description { get; set; }

        //[Display(Name = "Image Url")]
        //[Required(ErrorMessage = "Please upload profile image")]
        public string ImageUrl { get; set; }

        public bool isImageFromStock { get; set; }

        public string ImageFromStock { get; set; }
        public ApplicationUser CreatorUser { get; set; }

        [Required(ErrorMessage = " ")]
        public string Place { get; set; }
        public double PlaceLongitude { get; set; }
        public double PlaceLatitude { get; set; }

        [Required(ErrorMessage = " ")]
        public DateTime DateTimeOfTheEvent { get; set; }

        [Required(ErrorMessage = " ")]
        public string DateOfTheEvent { get; set; }

        public string HourTimeOfTheEvent { get; set; }
        public string MinuteTimeOfTheEvent { get; set; }

        public bool TimeSet { get; set; }

        public string DateOfTheEventNoYear { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public ICollection<ApplicationUser> UsersAttending { get; set; }
        public ICollection<ApplicationUser> UsersWaitingApproval { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        public string BackroundColor { get; set; }
        public bool CurrentUserIsCreator { get; set; }
        public string BackroundColorOpacity { get; set; }
        public string SideOfText { get; set; }
        public Dictionary<string, bool> SideOfTextOptions { get; set; }


        public bool NumOfParticipantsSet { get; set; }

        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [Range(1, 100)]
        public int? NumOfParticipantsMin { get; set; }

        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [MinMaxValidation("NumOfParticipantsMin", AllowEquality = true)]
        public int? NumOfParticipantsMax { get; set; }

        public bool AgeOfParticipantsSet { get; set; }

        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [Range(1, 100)]
        public int? AgeOfParticipantsMin { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [MinMaxValidation("AgeOfParticipantsMin", AllowEquality = true)]
        public int? AgeOfParticipantsMax { get; set; }


        public string GenderParticipant { get; set; }

        public Dictionary<string, bool> GenderParticipantOptions { get; set; }

        public virtual ICollection<InterestViewModel> AllInterests { get; set; }

        public string InterestsToDisplay { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public e_PrivacyType? PrivacyType { get; set; }
        public Dictionary<e_PrivacyType, bool> PrivacyTypeOptions { get; set; }
    }


    public class EventsByMonth
    {
        public string Month { get; set; }
        public List<Event> MonthEvents { get; set; }
    }

}