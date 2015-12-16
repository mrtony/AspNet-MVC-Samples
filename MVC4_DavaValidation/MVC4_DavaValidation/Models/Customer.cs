using System;
using System.ComponentModel.DataAnnotations;
using MVC4_DavaValidation.Attributes;

namespace MVC4_DavaValidation.Models
{
    public class Customer
    {
        [Display(Name = "Customer Id")]
        [Required(ErrorMessage = "Customer ID is required")]
        public long CustomerId { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(10, ErrorMessage = "Company Name should be less than or equal to ten characters.")]
        public string CompanyName { get; set; }

        [Display(Name = "Age")]
        [Range(20, 40, ErrorMessage = "Customer Age should be between 20 to 40.")]
        public int Age { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [ValidBirthDate("Age", ErrorMessage = "Birth Date can not be greater than current date")]
        public DateTime BirthDate { get; set; }
    }
}