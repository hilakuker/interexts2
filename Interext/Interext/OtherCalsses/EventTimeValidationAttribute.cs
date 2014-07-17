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
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EventTimeValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private const string Errormessage = "Event time cannot accur in the past.";
        public string OtherProperty { get; private set; }
        public EventTimeValidationAttribute(string otherProperty)
            : base(Errormessage)
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
            DateTime eventDateTime = (DateTime) value;
            // Check to see if the value is greater than the other property value
            if (eventDateTime < DateTime.Now)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
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
            yield return new ModelClientValidationEventDateRule(FormatErrorMessage(metadata.DisplayName), FormatPropertyForClientValidation(this.OtherProperty));
        }
    }
}