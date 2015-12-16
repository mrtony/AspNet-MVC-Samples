using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC4_DavaValidation.Attributes
{
    public class ValidNameAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly OnlyOneAttribute _onlyOneEntityValidator;
        private readonly CannotBeNotApplicableAttribute _cannotBeNotApplicable;
        private readonly CannotContainNumbersAttribute _cannotContainNumbers;

        public ValidNameAttribute()
        {
            _onlyOneEntityValidator = new OnlyOneAttribute { ErrorMessage = "cannot contain more than one {0}" };
            _cannotBeNotApplicable = new CannotBeNotApplicableAttribute { ErrorMessage = "cannot be Not Applicable" };
            _cannotContainNumbers = new CannotContainNumbersAttribute { ErrorMessage = "cannot contain numbers" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errormessage = new StringBuilder("This field ");
            if (value != null)
            {
                bool isValid = true;
                if (!_onlyOneEntityValidator.IsValid(value))
                {
                    isValid = false;
                    errormessage.Append(_onlyOneEntityValidator
                        .FormatErrorMessage(validationContext.DisplayName) + ", ");
                }
                if (!_cannotBeNotApplicable.IsValid(value))
                {
                    isValid = false;
                    errormessage.Append(_cannotBeNotApplicable
                        .FormatErrorMessage(validationContext.DisplayName) + ", ");
                }
                if (!_cannotContainNumbers.IsValid(value))
                {
                    isValid = false;
                    errormessage.Append(_cannotContainNumbers
                        .FormatErrorMessage(validationContext.DisplayName) + " ");
                }

                return isValid ? null
                    : new ValidationResult(errormessage.ToString(), new[] { validationContext.MemberName });
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "validname",
            };

            var clientValidationRules = new List<string>
                                              {
                                                  _onlyOneEntityValidator.GetClientValidationRules(metadata,context).First().ValidationType,
                                                  _cannotBeNotApplicable.GetClientValidationRules(metadata,context).First().ValidationType,
                                                  _cannotContainNumbers.GetClientValidationRules(metadata,context).First().ValidationType,
                                              };

            var regexPatterns = new List<string>
                               {
                                   _onlyOneEntityValidator.Pattern,
                                   _cannotBeNotApplicable.Pattern,
                                   _cannotContainNumbers.Pattern,
                               };
            var errorMessages = new List<string>
                                    {
                                        _onlyOneEntityValidator.FormatErrorMessage(metadata.GetDisplayName()),
                                        _cannotBeNotApplicable.FormatErrorMessage(metadata.GetDisplayName()),
                                        _cannotContainNumbers.FormatErrorMessage(metadata.GetDisplayName())
                                    };

            rule.ValidationParameters.Add("clientvalidationrules", clientValidationRules.Aggregate((i, j) => i + "," + j));
            rule.ValidationParameters.Add("regexpatterns", regexPatterns.Aggregate((i, j) => i + "," + j));
            rule.ValidationParameters.Add("errormessages", errorMessages.Aggregate((i, j) => i + "," + j));
            yield return rule;
        }
    }
}