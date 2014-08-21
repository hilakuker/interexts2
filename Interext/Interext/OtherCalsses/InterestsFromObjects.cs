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

        public static List<Interest> GetSelectedInterests(string selectedInterests, ApplicationDbContext db)
        {
            List<Interest> interests = new List<Interest>();
            foreach (string item in selectedInterests.Split(','))
            {
                if (item != "")
                {
                    int id;
                    if (int.TryParse(item, out id))
                    {
                        Interest interest = db.Interests.SingleOrDefault(x => x.Id == id);
                        interests.Add(interest);
                    }
                }
            }
            //add the categories if all the subcategories are selected 

            var allSubCategories = interests.Where(x => x.InterestsCategory != null);
            foreach (var item in allSubCategories)
            {
                var categoryId = item.InterestsCategory.Id;
                if (interests.SingleOrDefault(x => x.Id == categoryId) == null)
                {
                    //the category is not in the list, so check if all its subcategories are in the list
                    //if so, add it to the list
                    var allSubCategoriesOfTheCategory = db.Interests.Where(x => x.InterestsCategory.Id == categoryId);
                    var allSubCategoriesOfTheCategoriesChecked = interests.Where(x => x.InterestsCategory.Id == categoryId);
                    if (allSubCategoriesOfTheCategory.Count() == allSubCategoriesOfTheCategoriesChecked.Count())
                    {
                        //all the sub categories are checked, but the category itself is not. Add it to the list
                        Interest category = db.Interests.SingleOrDefault(x => x.Id == categoryId);
                        interests.Add(category);
                    }
                }
            }
            return interests;
        }



        //internal static void LoadAllInterestsFromEventView(List<Interest> InterestList, ApplicationDbContext db, EventViewModel model)
        //{
        //    model.AllInterests = InitAllInterests(db);
        //    foreach (Interest interestsDB in InterestList)
        //}

        internal static void LoadAllInterestsFromEventView(string selectedInterests, EventViewModel model, ApplicationDbContext db)
        {
            List<Interest> interestsList = GetSelectedInterests(selectedInterests, db);
            model.AllInterests = new List<InterestViewModel>();
            List<Interest> categories = db.Interests.Where(x => x.InterestsCategory == null).ToList();
            foreach (var item in categories)
            {
                InterestViewModel category =
                        new InterestViewModel
                        {
                            Id = item.Id,
                            ImageUrl = item.ImageUrl,
                            Title = item.Title,
                            SubInterests = new List<InterestViewModel>()
                        };
                if (interestsList.Where(x => x.Id == item.Id) == null)
                {
                    category.IsSelected = false;
                }
                else
                {
                    category.IsSelected = true;
                }
                foreach (var subitem in db.Interests.Where(x => x.InterestsCategory.Id == category.Id))
                {
                    InterestViewModel subcategory = 
                        new InterestViewModel { Id = subitem.Id, 
                            ImageUrl = subitem.ImageUrl,
                            Title = subitem.Title,
                            SubInterests = null};
                    if (interestsList.Where(x => x.Id == subitem.Id) == null)
                    {
                        subcategory.IsSelected = false;
                    }
                    else
                    {
                        subcategory.IsSelected = true;
                    }
                    category.SubInterests.Add(subcategory);
                }
                model.AllInterests.Add(category);
            }
        }
    }
}