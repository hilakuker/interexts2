namespace Interext.Migrations
{
    using Interext.Models;
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
    }
}
