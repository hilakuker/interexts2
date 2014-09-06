//using Interext.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Interext.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

          [Column(TypeName = "datetime2")]
        public DateTime? BirthDate { get; set; }
        [Required]
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<EventVsAttendingUser> EventVsAttentingUsers { get; set; }
        public virtual ICollection<EventVsWaitingApprovalUser> EventVsWaitingApprovalUsers { get; set; }
        public string HomeAddress { get; set; }
        public double PlaceLongitude { get; set; }
        public double PlaceLatitude { get; set; }

       [Required]
        public bool AccountIsActive { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<Event> Events { get; set; }
        //public DbSet<Group> Groups { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<PlaceRatingUser> PlaceRatingUsers { get; set; }
        public DbSet<EventVsAttendingUser> EventVsAttendingUsers { get; set; }
        public DbSet<EventVsWaitingApprovalUser> EventVsWaitingApprovalUsers { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<ReportLinks> ReportedUrl { get; set; }
    }
}