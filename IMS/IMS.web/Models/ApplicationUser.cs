using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IMS.web.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        //public string PhoneNumber { get; set; }
        [Display(Name = "Store Name")]
        public int StoreId { get; set; }
        [Display(Name = "User Role")]
        public string UserRoleId { get; set; }
        public string ProfileUrl { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get;set; }
        public string ModifiedBy { get; set; }


    }
}
