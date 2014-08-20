using Interext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interext.OtherCalsses
{
    public static class InterestsFromObjects
    {
        private static void setInterests(ICollection<Interest> interestsColleciton, ref List<InterestViewModel> allInterests)
        {
            foreach (var item in interestsColleciton)
            {
                InterestViewModel interest = allInterests.SingleOrDefault(x => x.Id == item.Id);
                if (interest != null)
                    interest.IsSelected = true;
                else
                {
                    foreach (var item2 in allInterests)
                    {
                        InterestViewModel interest2 = item2.SubInterests.SingleOrDefault(x => x.Id == item.Id);
                        if (interest2 != null)
                            interest2.IsSelected = true;
                    }
                }
            }
        }

        public static List<InterestViewModel> InitAllInterests(ApplicationDbContext db)
        {
            List<InterestViewModel> allInterests = new List<InterestViewModel>();
            List<Interest> categories = db.Interests.Where(x => x.InterestsCategory == null).ToList();
            foreach (var item in categories)
            {
                InterestViewModel category = new InterestViewModel { Id = item.Id, ImageUrl = item.ImageUrl, Title = item.Title, SubInterests = new List<InterestViewModel>(), IsSelected = false };
                foreach (var subitem in db.Interests.Where(x => x.InterestsCategory.Id == category.Id))
                {
                    InterestViewModel subcategory = new InterestViewModel { Id = subitem.Id, ImageUrl = subitem.ImageUrl, Title = subitem.Title, SubInterests = null, IsSelected = false };
                    category.SubInterests.Add(subcategory);
                }
                allInterests.Add(category);
            }
            return allInterests;
        }

        internal static List<InterestViewModel> LoadInterestViewModelsFromInterests(ICollection<Interest> interests, ApplicationDbContext db)
        {
            List<InterestViewModel> allInterests = InitAllInterests(db);
            setInterests(interests, ref allInterests);
            return allInterests;
        }
    }
}