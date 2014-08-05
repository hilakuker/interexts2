using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class Event
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public virtual ApplicationUser CreatorUser { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        //the options for the events or groups are only these that u are member of

        public string Place { get; set; } // the object Place with all the info .. like previous events .. if not specified then 
        //it is the place/location of the Group
        public double PlaceLongitude { get; set; }
        public double PlaceLatitude { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateTimeOfTheEvent { get; set; }

        [Required]
        public string BackroundColor { get; set; }
        public string BackroundColorOpacity { get; set; }
        public string SideOfText { get; set; }
        public int? NumOfParticipantsMin { get; set; }
        public int? NumOfParticipantsMax { get; set; }
        public int? AgeOfParticipantsMin { get; set; }
        public int? AgeOfParticipantsMax { get; set; }
        public string GenderParticipant { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTimeCreated { get; set; }

        public ICollection<UserProfile> UsersAttending { get; set; }
        public ICollection<UserProfile> UsersAwatingApproval { get; set; }
        public ICollection<UserProfile> UsersApprovedAttendance { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        
    }
}