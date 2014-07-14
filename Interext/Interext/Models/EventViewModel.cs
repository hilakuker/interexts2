using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Place Place { get; set; }
        public DateTime DateTimeOfTheEvent { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public ICollection<UserProfile> UsersAttending { get; set; }
        public ICollection<UserProfile> UsersInvited { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<Interest> AllInterests { get; set; }
    }
}