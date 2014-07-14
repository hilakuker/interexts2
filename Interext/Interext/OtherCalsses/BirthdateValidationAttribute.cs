using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Interext.OtherCalsses
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BirthdateValidationAttribute : ValidationAttribute
    {
        public BirthdateValidationAttribute()
            : base("Date cannot be bigger than Today's date")
        {
                
        }
        public override bool IsValid(object value) {
            bool valueIsValid = true;
            
            if (value != null)
            {
                DateTime dateStart = (DateTime)value;
                if (dateStart > DateTime.Now)
                    valueIsValid = false;
            }
            return valueIsValid;
        }
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = new ValidationResult("Date cannot be bigger than Today's date");
            if (value != null)
            {
                DateTime dateStart = (DateTime)value;
                if (dateStart > DateTime.Now)
                    return validationResult;
                else return ValidationResult.Success;
            }
            else return ValidationResult.Success;
        }
    }
}