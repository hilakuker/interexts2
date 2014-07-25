using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interext.Models
{
    public class ModelClientValidationEventDateRule : ModelClientValidationRule
    {
        public ModelClientValidationEventDateRule(string errorMessage, object other)
        {
            ErrorMessage = errorMessage;
            ValidationType = "eventdatenotinthepast";
            ValidationParameters["other"] = other;
        }
    }
}