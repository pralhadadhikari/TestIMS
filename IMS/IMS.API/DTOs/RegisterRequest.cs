using System.ComponentModel.DataAnnotations;

namespace IMS.API.DTOs
{
        public class RegisterRequest
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Middle Name")]
            public string MiddleName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public string Address { get; set; }

            [Display(Name = "Store Name")]
            public int StoreId { get; set; }

            [Display(Name = "User Role")]
            public string UserRole { get; set; }

            //public string ProfileUrl { get; set; }

            //[Required]
            //public string UserName { get; set; }
            [Required]
            [Phone]
            [RegularExpression(
            @"^(97|98)\d{8}$",
            ErrorMessage = "Please enter a valid 10-digit Nepali mobile number.")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [MinLength(6)]
            public string Password { get; set; }
        }
    
}
