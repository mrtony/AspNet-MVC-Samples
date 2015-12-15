using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC4_DavaValidation.Models
{
    public sealed class ValidBirthDate : ValidationAttribute, IClientValidatable
    {
        public string OtherProperty { get; set; }

        public ValidBirthDate(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime _birthJoin = Convert.ToDateTime(value);
                if (_birthJoin > DateTime.Now)
                {
                    return new ValidationResult("Birth date can not be greater than current date.");
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "validbirthdate",
            };
            //會在html中產生data-val-validbirthdate-other. 所以可以利用這種方式, 帶入參數
            rule.ValidationParameters["other"] = "123";
            rule.ValidationParameters.Add("para1", OtherProperty);
            yield return rule;
        }
    }
}