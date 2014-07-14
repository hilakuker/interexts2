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

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        //the options for the events or groups are only these that u are member of

        public Place Place { get; set; } // the object Place with all the info .. like previous events .. if not specified then 
        //it is the place/location of the Group

        public Group Group { get; set; } // the object Group with all the info .. like previous events .. the place/location 
        //can be overriden by the specific location of the Event

        [Required]
        public DateTime DateTimeOfTheEvent { get; set; }

        public DateTime DateTimeCreated { get; set; }
        public ICollection<UserProfile> UsersAttending { get; set; }
        public ICollection<UserProfile> UsersAwatingApproval { get; set; }
        public ICollection<UserProfile> UsersApprovedAttendance { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        
    }
}