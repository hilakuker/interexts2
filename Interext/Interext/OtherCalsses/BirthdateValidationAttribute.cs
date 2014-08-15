using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interext.OtherCalsses
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BirthdateValidationAttribute : ValidationAttribute
    {
        public BirthdateValidationAttribute()
            : base("Birth Day cannot be bigger than Today's date")
        {

        }
        public override bool IsValid(object value)
        {
            bool valueIsValid = true;

            if (value != null)
            {
                DateTime dateStart = (DateTime)value;
                if (dateStart > DateTime.Now)
                    valueIsValid = false;
            }
            else
            {
                valueIsValid = false; // when it is null it means that the datetime parse was not succeeded
            }
            return valueIsValid;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = new ValidationResult("Birth Day cannot be bigger than Today's date");
            
            if (value != null)
            {
                DateTime dateStart = (DateTime)value;
                if (dateStart > DateTime.Now)
                    return validationResult;
                else return ValidationResult.Success;
            }
            else
            {
                // when it is null it means that the datetime parse was not succeeded
                return validationResult;
            }
        }
    }

    //public class BirthdateValidationAttribute : ValidationAttribute, IClientValidatable
    //{
    //    public override bool IsValid(object value)
    //    {
    //        //bool res = value != null && (DateTime)value < DateTime.Now;
    //        return true;
    //    }

    //    public IEnumerable<ModelClientValidationRule>
    //           GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        yield return new ModelClientValidationRule
    //        {
    //            ErrorMessage = ErrorMessage,
    //            ValidationType = "birthdatee"
    //        };
    //    }
    //}
}