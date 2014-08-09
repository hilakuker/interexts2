using Interext.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interext.OtherCalsses
{
    public class CascadingDeleteHelper
    {
        public static void Delete(object entity, Func<object, bool> beforeDeleteCallback)
        {
            //if (!(entity is BusinessEntity))
            //{
            //    throw new ArgumentException("Argument is not a valid BusinessEntity");
            //}

            if (beforeDeleteCallback == null || beforeDeleteCallback(entity))
            {
                Type currentType = entity.GetType();
                foreach (var property in currentType.GetProperties())
                {
                    var attribute = property
                        .GetCustomAttributes(true)
                        .Where(a => a is AssociationAttribute)
                        .FirstOrDefault();

                    if (attribute != null)
                    {
                        AssociationAttribute assoc = attribute as AssociationAttribute;

                        if (!assoc.IsForeignKey)
                        {
                            var propertyValue = property.GetValue(entity, null);
                            if (propertyValue != null && propertyValue is IEnumerable)
                            {
                                IEnumerable relations = propertyValue as IEnumerable;
                                List<object> relatedEntities = new List<object>();

                                foreach (var relation in relations)
                                {
                                    relatedEntities.Add(relation);
                                }

                                relatedEntities.ForEach(e => Delete(e, beforeDeleteCallback));
                            }
                        }
                    }
                }
                //SingletonDataContext.DataContext.GetTable(currentType).DeleteOnSubmit(entity);
                //SingletonDataContext.DataContext.SubmitChanges();
            }
        }
    }
}