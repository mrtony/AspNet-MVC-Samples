using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC4_DavaValidation.Attributes
{
    public class CannotContainNumbersAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _pattern = @"^((?!(?:[0-9])).)*$";
        private const string DefaultErrormessage = "{0} cannot contain numbers";
        public CannotContainNumbersAttribute()
        {
            ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? DefaultErrormessage : ErrorMessage;
        }

        public string Pattern
        {
            get { return _pattern; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value) ? null : new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var regexValidator = new RegularExpressionAttribute(_pattern);
                if (!regexValidator.IsValid(value))
                {
                    return false;
                }
            }
            return true;
        }
        
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
                           {
                               ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                               ValidationType = "cannotcontainnumbers",
                           };
            yield return rule;
        }
    }
}