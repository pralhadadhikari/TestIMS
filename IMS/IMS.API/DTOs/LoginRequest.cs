using System.ComponentModel.DataAnnotations;

namespace IMS.API.DTOs
{
    public class LoginRequest
    {
        //[Required]
        //public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
