using Interext.OtherCalsses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class EventViewModel:ImageModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        [Display(Name = "Place")]
        public string Place { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date is required")]
        [EventTimeValidation("DateTimeOfTheEvent")]

        public DateTime DateTimeOfTheEvent { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public ICollection<UserProfile> UsersAttending { get; set; }
        public ICollection<UserProfile> UsersInvited { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public string BackroundColor { get; set; }
        public string BackroundColorCapacity { get; set; }
        public string SideOfText { get; set; }
        [RegularExpression (@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [MinMaxValidation("NumOfParticipantsMax", AllowEquality = true)]
        public int NumOfParticipantsMin { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [Range(1, 100)]
        public int NumOfParticipantsMax { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        [Range(1, 100)]
        [MinMaxValidation("AgeOfParticipantsMax", AllowEquality = true)]
        public int AgeOfParticipantsMin { get; set; }
        [RegularExpression(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])", ErrorMessage = "Input must be a number")]
        public int AgeOfParticipantsMax { get; set; }
        public string GenderParticipant { get; set; }  public ICollection<Interest> AllInterests { get; set; }
    }


}