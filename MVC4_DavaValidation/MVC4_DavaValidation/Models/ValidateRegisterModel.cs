using System.ComponentModel.DataAnnotations;
using MVC4_DavaValidation.Attributes;

namespace MVC4_DavaValidation.Models
{
    public class ValidateRegisterModel
    {
        [Required]
        [ValidName]
        public string FirstName { get; set; }

        [Required]
        [OnlyOne]
        [CannotBeNotApplicable]
        [CannotContainNumbers]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}