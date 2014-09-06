namespace Interext.Migrations
{
    using Interext.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Interext.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Interext.Models.ApplicationDbContext context)
        {
            CreateAdminUser(context);

            var mainCategories = new List<Interest>();

            Interest books = new Interest
            {
                Title = "Books",
                ImageUrl = "/Content/images/interests/books.jpg",
                InterestsCategory = null,
                ImagesForStock = "/Content/images/interests/Books/1.jpg*/Content/images/interests/Books/2.jpg*/Content/images/interests/Books/3.jpg*/Content/images/interests/Books/4.jpg" 
            };
            Interest sports = new Interest
            {
                Title = "Sports",
                ImageUrl = "/Content/images/interests/sports.jpg",
                InterestsCategory = null,
                ImagesForStock = "/Content/images/interests/Sports/1.jpg*/Content/images/interests/Sports/2.jpg*/Content/images/interests/Sports/3.jpg" 
            };
            Interest cooking = new Interest
            {
                Title = "Cooking",
                ImageUrl = "/Content/images/interests/cooking.jpg",
                InterestsCategory = null,
                ImagesForStock = "/Content/images/interests/Cooking/1.jpg" 
            };
            Interest outdoor = new Interest
            {
                Title = "Outdoor",
                ImageUrl = "/Content/images/interests/outdoor.jpg",
                InterestsCategory = null,
                ImagesForStock = "/Content/images/interests/Outdoor/1.jpg"
            };
            Interest arts = new Interest
            {
                Title = "Arts",
                ImageUrl = "/Content/images/interests/Arts.jpg",
                InterestsCategory = null,
                ImagesForStock = "/Content/images/interests/Arts/1.jpg*/Content/images/interests/Arts/2.jpg*/Content/images/interests/Arts/3.jpg*/Content/images/interests/Arts/4.jpg*/Content/images/interests/Arts/5.jpg*/Content/images/interests/Arts/6.jpg*/Content/images/interests/Arts/7.jpg*/Content/images/interests/Arts/8.jpg*/Content/images/interests/Arts/9.jpg*/Content/images/interests/Arts/10.jpg*/Content/images/interests/Arts/11.jpg*/Content/images/interests/Arts/12.jpg"
            };
            Interest music = new Interest
            {
                Title = "Music",
                ImageUrl = "/Content/images/interests/music.jpg",
                InterestsCategory = null,
                ImagesForStock = "/Content/images/interests/Music/1.jpg"
            };
            Interest pets = new Interest
            {
                Title = "Pets",
                ImageUrl = "/Content/images/interests/pets.jpg",
                InterestsCategory = null,
                ImagesForStock = "/Content/images/interests/Pets/1.jpg*/Content/images/interests/Pets/2.jpg*/Content/images/interests/Pets/3.jpg*/Content/images/interests/Pets/4.jpg*/Content/images/interests/Pets/5.jpg*/Content/images/interests/Pets/6.jpg*/Content/images/interests/Pets/7.jpg*/Content/images/interests/Pets/8.jpg*/Content/images/interests/Pets/9.jpg*/Content/images/interests/Pets/10.jpg*/Content/images/interests/Pets/11.jpg*/Content/images/interests/Pets/12.jpg*/Content/images/interests/Pets/13.jpg"
            };

            mainCategories.Add(music);
            mainCategories.Add(books);
            mainCategories.Add(sports);
            mainCategories.Add(arts);
            mainCategories.Add(cooking);
            mainCategories.Add(outdoor);
            mainCategories.Add(pets);

            mainCategories.ForEach(x => context.Interests.AddOrUpdate(y => y.Title, x));

            context.SaveChanges();

            Interest books2 = context.Interests.Where(x => x.Title == "Books").ToList()[0];
            Interest sports2 = context.Interests.Where(x => x.Title == "Sports").ToList()[0];
            Interest cooking2 = context.Interests.Where(x => x.Title == "Cooking").ToList()[0];
            Interest outdoor2 = context.Interests.Where(x => x.Title == "Outdoor").ToList()[0];
            Interest arts2 = context.Interests.Where(x => x.Title == "Arts").ToList()[0];
            Interest music2 = context.Interests.Where(x => x.Title == "Music").ToList()[0];
            Interest pets2 = context.Interests.Where(x => x.Title == "Pets").ToList()[0];

            var subCategories = new List<Interest>();
            Interest jogging = new Interest { Title = "Jogging", ImageUrl = "", InterestsCategory = sports2 };
            Interest bicycle = new Interest { Title = "Cycling", ImageUrl = "", InterestsCategory = sports2 };
            Interest basketball = new Interest { Title = "Basketball", ImageUrl = "", InterestsCategory = sports2 };
            Interest tennis = new Interest { Title = "Tennis", ImageUrl = "", InterestsCategory = sports2 };
            Interest football = new Interest { Title = "Football", ImageUrl = "", InterestsCategory = sports2 };
            Interest Dance = new Interest { Title = "Dance", ImageUrl = "", InterestsCategory = sports2 };
            subCategories.Add(Dance);

            Interest Dogs = new Interest { Title = "Dogs", ImageUrl = "", InterestsCategory = pets2 };
            subCategories.Add(Dogs);
            Interest Cats = new Interest { Title = "Cats", ImageUrl = "", InterestsCategory = pets2 };
            subCategories.Add(Cats);
            Interest Birds = new Interest { Title = "Birds", ImageUrl = "", InterestsCategory = pets2 };
            subCategories.Add(Birds);
            Interest Fish = new Interest { Title = "Fish", ImageUrl = "", InterestsCategory = pets2 };
            subCategories.Add(Fish);
            Interest PetShelter = new Interest { Title = "Pet Shelter", ImageUrl = "", InterestsCategory = pets2 };
            subCategories.Add(PetShelter);

            Interest Baking = new Interest { Title = "Baking", ImageUrl = "", InterestsCategory = cooking2 };
            subCategories.Add(Baking);
            Interest barbecue = new Interest { Title = "Barbecue", ImageUrl = "", InterestsCategory = cooking2 };
            subCategories.Add(barbecue);

            Interest tracking = new Interest { Title = "Tracking", ImageUrl = "", InterestsCategory = outdoor2 };
            subCategories.Add(tracking);
            Interest mauntainClimbing = new Interest { Title = "Mauntain Climbing", ImageUrl = "", InterestsCategory = outdoor2 };
            subCategories.Add(mauntainClimbing);
            Interest Camping = new Interest { Title = "Camping", ImageUrl = "", InterestsCategory = outdoor2 };
            subCategories.Add(Camping);


            Interest Fantasy = new Interest { Title = "Fantasy", ImageUrl = "", InterestsCategory = books2 };
            Interest Adventure = new Interest { Title = "Adventure", ImageUrl = "", InterestsCategory = books2 };
            Interest Classics = new Interest { Title = "Classics", ImageUrl = "", InterestsCategory = books2 };
            Interest Crime = new Interest { Title = "Crime", ImageUrl = "", InterestsCategory = books2 };
            Interest EroticFiction = new Interest { Title = "Erotic Fiction", ImageUrl = "", InterestsCategory = books2 };
            Interest GraphicNovelsAnimeManga = new Interest { Title = "Graphic Novels, Anime & Manga", ImageUrl = "", InterestsCategory = books2 };
            Interest HistoricalFiction = new Interest { Title = "Historical Fiction", ImageUrl = "", InterestsCategory = books2 };
            Interest Horror = new Interest { Title = "Horror", ImageUrl = "", InterestsCategory = books2 };
            Interest MythLegendToldFiction = new Interest { Title = "Myth & Legend Told As Fiction", ImageUrl = "", InterestsCategory = books2 };
            Interest ReligiousSpiritualFiction = new Interest { Title = "Religious & Spiritual Fiction", ImageUrl = "", InterestsCategory = books2 };
            Interest Romance = new Interest { Title = "Romance", ImageUrl = "", InterestsCategory = books2 };
            Interest Sagas = new Interest { Title = "Sagas", ImageUrl = "", InterestsCategory = books2 };
            Interest ScienceFiction = new Interest { Title = "Science Fiction", ImageUrl = "", InterestsCategory = books2 };
            Interest Thrillers = new Interest { Title = "Thrillers", ImageUrl = "", InterestsCategory = books2 };

            Interest Painting = new Interest { Title = "Painting", ImageUrl = "", InterestsCategory = arts2 };
            subCategories.Add(Painting);
            Interest Theater = new Interest { Title = "Theater", ImageUrl = "", InterestsCategory = arts2 };
            subCategories.Add(Theater);
            Interest Architecture = new Interest { Title = "Architecture", ImageUrl = "", InterestsCategory = arts2 };
            subCategories.Add(Architecture);
            Interest PerformingArts = new Interest { Title = "Performing Arts", ImageUrl = "", InterestsCategory = arts2 };
            subCategories.Add(PerformingArts);
            Interest GraphicDesign = new Interest { Title = "Graphic Design", ImageUrl = "", InterestsCategory = arts2 };
            subCategories.Add(GraphicDesign);
            Interest Tattoos = new Interest { Title = "Tattoos", ImageUrl = "", InterestsCategory = arts2 };
            subCategories.Add(Tattoos);

            Interest ClassicMusic = new Interest { Title = "Classic", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(ClassicMusic);
            Interest RockMusic = new Interest { Title = "Rock", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(RockMusic);
            Interest AlternativeMusic = new Interest { Title = "Alternative", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(AlternativeMusic);
            Interest PopMusic = new Interest { Title = "Pop", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(PopMusic);
            Interest IndieMusic = new Interest { Title = "Indie", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(IndieMusic);
            Interest MetalMusic = new Interest { Title = "Metal", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(MetalMusic);
            Interest ClassicRock = new Interest { Title = "Classic Rock", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(ClassicRock);
            Interest Techno = new Interest { Title = "Techno", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(Techno);
            Interest Reggae = new Interest { Title = "Reggae", ImageUrl = "", InterestsCategory = music2 };
            subCategories.Add(Reggae);

            subCategories.Add(Thrillers);
            subCategories.Add(ScienceFiction);
            subCategories.Add(Sagas);
            subCategories.Add(Romance);
            subCategories.Add(ReligiousSpiritualFiction);
            subCategories.Add(MythLegendToldFiction);
            subCategories.Add(Horror);
            subCategories.Add(HistoricalFiction);
            subCategories.Add(GraphicNovelsAnimeManga);
            subCategories.Add(EroticFiction);
            subCategories.Add(Crime);
            subCategories.Add(Adventure);
            subCategories.Add(Classics);
            subCategories.Add(Fantasy);
            subCategories.Add(jogging);
            subCategories.Add(bicycle);
            subCategories.Add(basketball);
            subCategories.Add(tennis);
            subCategories.Add(football);


            subCategories.ForEach(x => context.Interests.AddOrUpdate(y => y.Title, x));

            context.SaveChanges();
        }
        public string GenerateUserName(string email)
        {
            return email.Replace("@", "").Replace(".", "").Replace("-", "");
        }
        private void CreateAdminUser(ApplicationDbContext context)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (RoleManager.FindByName("Admin") == null)
            {
                RoleManager.Create(new IdentityRole("Admin"));
            }
            if (db.Users.SingleOrDefault(x => x.UserName == "admin@123.com") == null)
            {
                var user = new ApplicationUser()
                {
                    Email = "admin@123.com",
                    FirstName = "admin",
                    LastName = "admin",
                    BirthDate = new DateTime(1987, 12, 13),
                    Gender = "Female",
                    Age = 26,
                    UserName = GenerateUserName("admin@123.com"),
                    AccountIsActive = true,
                    ImageUrl = "/Content/images/users/admin.png",
                    HomeAddress = "Tel Aviv, Israel",
                    PlaceLatitude = 32.0852999,
                    PlaceLongitude = 34.781767599999966
                };
                UserManager.Create(user, "123456");
                ApplicationUser newUser = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
                UserManager.AddToRole(newUser.Id, "Admin");
            }
        }
    }
}
