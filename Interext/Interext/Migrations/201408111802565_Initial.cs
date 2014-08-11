namespace Interext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ImageUrl = c.String(),
                        Place = c.String(),
                        PlaceLongitude = c.Double(nullable: false),
                        PlaceLatitude = c.Double(nullable: false),
                        DateTimeOfTheEvent = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        BackroundColor = c.String(nullable: false),
                        BackroundColorOpacity = c.String(),
                        SideOfText = c.String(),
                        NumOfParticipantsMin = c.Int(),
                        NumOfParticipantsMax = c.Int(),
                        AgeOfParticipantsMin = c.Int(),
                        AgeOfParticipantsMax = c.Int(),
                        GenderParticipant = c.String(),
                        DateTimeCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Place_Id = c.Int(),
                        CreatorUser_Id = c.String(nullable: false, maxLength: 128),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Places", t => t.Place_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Place_Id)
                .Index(t => t.CreatorUser_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        BirthDate = c.DateTime(),
                        Gender = c.String(),
                        ImageUrl = c.String(),
                        HomeAddress = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        InterestsCategory_Id = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interests", t => t.InterestsCategory_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.InterestsCategory_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        LocationAddress = c.String(nullable: false),
                        CreatorUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorUser_Id, cascadeDelete: true)
                .Index(t => t.CreatorUser_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlaceRatingUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        RatedPlace_Id = c.Int(),
                        RatingUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.RatedPlace_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RatingUser_Id)
                .Index(t => t.RatedPlace_Id)
                .Index(t => t.RatingUser_Id);
            
            CreateTable(
                "dbo.InterestEvents",
                c => new
                    {
                        Interest_Id = c.Int(nullable: false),
                        Event_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Interest_Id, t.Event_Id })
                .ForeignKey("dbo.Interests", t => t.Interest_Id, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .Index(t => t.Interest_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.PlaceInterests",
                c => new
                    {
                        Place_Id = c.Int(nullable: false),
                        Interest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Place_Id, t.Interest_Id })
                .ForeignKey("dbo.Places", t => t.Place_Id, cascadeDelete: true)
                .ForeignKey("dbo.Interests", t => t.Interest_Id, cascadeDelete: true)
                .Index(t => t.Place_Id)
                .Index(t => t.Interest_Id);
            
            CreateTable(
                "dbo.InterestApplicationUsers",
                c => new
                    {
                        Interest_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Interest_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Interests", t => t.Interest_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Interest_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlaceRatingUsers", "RatingUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PlaceRatingUsers", "RatedPlace_Id", "dbo.Places");
            DropForeignKey("dbo.Interests", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Events", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InterestApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InterestApplicationUsers", "Interest_Id", "dbo.Interests");
            DropForeignKey("dbo.PlaceInterests", "Interest_Id", "dbo.Interests");
            DropForeignKey("dbo.PlaceInterests", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.Events", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.Places", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Interests", "InterestsCategory_Id", "dbo.Interests");
            DropForeignKey("dbo.InterestEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.InterestEvents", "Interest_Id", "dbo.Interests");
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PlaceRatingUsers", new[] { "RatingUser_Id" });
            DropIndex("dbo.PlaceRatingUsers", new[] { "RatedPlace_Id" });
            DropIndex("dbo.Interests", new[] { "Group_Id" });
            DropIndex("dbo.Events", new[] { "Group_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Event_Id" });
            DropIndex("dbo.Events", new[] { "CreatorUser_Id" });
            DropIndex("dbo.InterestApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.InterestApplicationUsers", new[] { "Interest_Id" });
            DropIndex("dbo.PlaceInterests", new[] { "Interest_Id" });
            DropIndex("dbo.PlaceInterests", new[] { "Place_Id" });
            DropIndex("dbo.Events", new[] { "Place_Id" });
            DropIndex("dbo.Places", new[] { "CreatorUser_Id" });
            DropIndex("dbo.Interests", new[] { "InterestsCategory_Id" });
            DropIndex("dbo.InterestEvents", new[] { "Event_Id" });
            DropIndex("dbo.InterestEvents", new[] { "Interest_Id" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropTable("dbo.InterestApplicationUsers");
            DropTable("dbo.PlaceInterests");
            DropTable("dbo.InterestEvents");
            DropTable("dbo.PlaceRatingUsers");
            DropTable("dbo.Groups");
            DropTable("dbo.Places");
            DropTable("dbo.Interests");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Events");
        }
    }
}
