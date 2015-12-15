using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC4_DavaValidation.Models
{
    public class User
    {
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 6)]
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8)]
        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8)]
        [Required]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 在controller驗證
        /// </summary>
        public string AddErrorEmail { get; set; }

        /// <summary>
        /// 加入自訂驗證
        /// </summary>
        [UserEmailValidator]
        public string CustomEmail { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{8}$")]
        [StringLength(32)]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(255)]
        public string Remarks { get; set; }
    }
}