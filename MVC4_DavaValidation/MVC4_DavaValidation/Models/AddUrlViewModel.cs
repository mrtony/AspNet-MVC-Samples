using System.ComponentModel.DataAnnotations;
using System.Runtime.Remoting.Activation;

namespace MVC4_DavaValidation.Models
{
    public class AddUrlViewModel
    {
        [Required]
        [DataType(DataType.Url)]
        public string OriginalUrl { get; set; }

        public bool UseCustomSlug { get; set; }

        [RequiredIfTrue(BooleanPropertyName = "UseCustomSlug", ErrorMessage = "Please provide a custom slug if you opt for one.")]
        public string CustomSlug { get; set; }
    }
}