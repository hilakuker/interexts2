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

            Interest books = new Interest { Title = "Books", ImageUrl = "/Content/images/interests/books.jpg", InterestsCategory = null };
            Interest sports = new Interest { Title = "Sports", ImageUrl = "/Content/images/interests/sports.jpg", InterestsCategory = null };
            Interest cooking = new Interest { Title = "Cooking", ImageUrl = "/Content/images/interests/cooking.jpg", InterestsCategory = null };
            Interest nature = new Interest { Title = "Nature", ImageUrl = "/Content/images/interests/nature.jpg", InterestsCategory = null };

            mainCategories.Add(books);
            mainCategories.Add(sports);
            mainCategories.Add(cooking);
            mainCategories.Add(nature);

            mainCategories.ForEach(x => context.Interests.AddOrUpdate(y => y.Title, x));

            context.SaveChanges();

            Interest books2 = context.Interests.Where(x => x.Title == "Books").ToList()[0];
            Interest sports2 = context.Interests.Where(x => x.Title == "Sports").ToList()[0];
            Interest cooking2 = context.Interests.Where(x => x.Title == "Cooking").ToList()[0];
            Interest nature2 = context.Interests.Where(x => x.Title == "Nature").ToList()[0];

            var subCategories = new List<Interest>();
            Interest jogging = new Interest { Title = "Jogging", ImageUrl = "", InterestsCategory = sports2 };
            Interest bicycle = new Interest { Title = "Bicycle", ImageUrl = "", InterestsCategory = sports2 };
            Interest basketball = new Interest { Title = "Basketball", ImageUrl = "", InterestsCategory = sports2 };
            Interest tennis = new Interest { Title = "Tennis", ImageUrl = "", InterestsCategory = sports2 };
            Interest football = new Interest { Title = "Football", ImageUrl = "", InterestsCategory = sports2 };

            Interest tracking = new Interest { Title = "Tracking", ImageUrl = "", InterestsCategory = nature2 };
            Interest mauntainClimbing = new Interest { Title = "Mauntain Climbing", ImageUrl = "", InterestsCategory = nature2 };
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
            subCategories.Add(tracking);
            subCategories.Add(mauntainClimbing);

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
                    UserName = GenerateUserName("admin@123.com")
                };
                UserManager.Create(user, "123456");
                ApplicationUser newUser = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
                UserManager.AddToRole(newUser.Id, "Admin");
            }
        }
    }
}
