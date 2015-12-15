using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC4_DavaValidation.Models
{
    public class RequiredIfTrueAttribute : ValidationAttribute, IClientValidatable
    {
        public string BooleanPropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            if ((bool)value)
            {
                return new RequiredAttribute().IsValid(value) ?
                    ValidationResult.Success :
                    new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "requirediftrue",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            modelClientValidationRule.ValidationParameters.Add("boolprop", BooleanPropertyName);
            yield return modelClientValidationRule;
        }
    }
}