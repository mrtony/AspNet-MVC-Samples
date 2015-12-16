using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC4_DavaValidation.Attributes
{
    public class CannotBeNotApplicableAttribute : ValidationAttribute, IClientValidatable
    {   
        private const string _pattern = @"^(?!.*(?:(\b[Nn][Oo][Tt]\s?[Aa][Pp][Pp][Ll][Ii][Cc][Aa][Bb][Ll][Ee])|(\b[Nn](\/?)[Aa]))\b).*$";
        private const string DefaultErrormessage = "{0} field cannot contain Not Applicable";
        public CannotBeNotApplicableAttribute()
        {
            ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? DefaultErrormessage : ErrorMessage;
        }

        public string Pattern
        {
            get { return _pattern; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value) ? null : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
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
                ValidationType = "cannotbenotapplicable",
            };
            rule.ValidationParameters.Add("pattern", _pattern);
            yield return rule;
        }
    }
}