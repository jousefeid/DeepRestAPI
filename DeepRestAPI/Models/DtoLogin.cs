using System.ComponentModel.DataAnnotations;

namespace DeepRestAPI.Models
{
    public class DtoLogin
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
