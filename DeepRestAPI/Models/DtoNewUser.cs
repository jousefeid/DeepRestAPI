using System.ComponentModel.DataAnnotations;

namespace DeepRestAPI.Models
{
    public class DtoNewUser
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string? phone { get; set; }
    }
}
