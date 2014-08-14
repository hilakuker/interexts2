using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interext.OtherCalsses
{
    //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BirthdateValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "validateBirthday"
            };

            //clientValidationRule.ValidationParameters.Add("otherproperty", OtherProperty);

            return new[] { clientValidationRule };
        }
        public BirthdateValidationAttribute()
            : base("The <span class=\"field-name-error\">Birth day</span> field is incorrect")
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
            //ValidationResult validationResult = new ValidationResult("Date cannot be bigger than Today's date");
            //ValidationResult validationResult = new ValidationResult("The <span class=\"field-name-error\">Birth day</span> field is incorrect");
            ValidationResult validationResult = new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            if (value != null)
            {
                DateTime dateStart = (DateTime)value;
                if (dateStart > DateTime.Now)
                    return validationResult;
                else return ValidationResult.Success;
                //else return null;
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
    //        return value != null && (DateTime)value > DateTime.Now;
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