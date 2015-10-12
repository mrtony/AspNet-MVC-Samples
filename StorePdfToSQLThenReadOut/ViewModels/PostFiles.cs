using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ViewModels
{
    public class PostFiles
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}
