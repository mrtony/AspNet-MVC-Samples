using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC4_DavaValidation.Attributes
{
    public class OnlyOneAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _pattern = @"^((?!(?:\b([A|a]+(N|n)+[D|d])\b|&|\+|/)).)*$";

        private const string DefaultErrormessage = "More than one {0} is not allowed";
        public OnlyOneAttribute()
        {
            ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? DefaultErrormessage : ErrorMessage;
        }

        public string Pattern
        {
            get { return _pattern; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value) ? 
                null : 
                new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
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
                ValidationType = "onlyoneentity",
            };
            rule.ValidationParameters.Add("pattern", _pattern);
            yield return rule;
        }
    }
}