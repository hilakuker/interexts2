using Interext.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Interext.OtherCalsses
{
    public class MinMaxValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private const string lessThanErrorMessage = "Maximum value must be bigger than Minumum value.";
        private const string lessThanOrEqualToErrorMessage = "Maximum value must be bigger than Minumu value.";

        public string OtherProperty { get; private set; }

        private bool allowEquality;

        public bool AllowEquality
        {
            get { return this.allowEquality; }
            set
            {
                this.allowEquality = value;

                // Set the error message based on whether or not
                // equality is allowed
                this.ErrorMessage = (value ? lessThanOrEqualToErrorMessage : lessThanErrorMessage);
            }
        }

        public MinMaxValidationAttribute(string otherProperty)
            : base(lessThanErrorMessage)
        {
            if (otherProperty == null) { throw new ArgumentNullException("otherProperty"); }
            this.OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, this.OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

                if (otherPropertyInfo == null)
                {
                    return null;
                }

                object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                decimal decValue;
                decimal decOtherPropertyValue;

                // Check to ensure the validating property is numeric
                if (!decimal.TryParse(value.ToString(), out decValue))
                {
                    return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
                }

                // Check to ensure the other property is numeric
                if (!decimal.TryParse(otherPropertyValue.ToString(), out decOtherPropertyValue))
                {
                    return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
                }

                // Check for equality
                if (AllowEquality && decValue == decOtherPropertyValue)
                {
                    return null;
                }
                // Check to see if the value is greater than the other property value
                else if (decValue < decOtherPropertyValue)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return null;
        }

        public static string FormatPropertyForClientValidation(string property)
        {
            if (property == null)
            {
                throw new ArgumentException("Value cannot be null or empty.", "property");
            }
            return "*." + property;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationNumericLessThanRule(FormatErrorMessage(metadata.DisplayName), FormatPropertyForClientValidation(this.OtherProperty), this.AllowEquality);
        }
    }
}