using System.ComponentModel.DataAnnotations;

namespace IMS.API.DTOs
{
    public class StoreCreateRequest
    {
        [Required]
        public string StoreName { get; set; }

        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string RegistrationNo { get; set; }

        public string PanNo { get; set; }

        public bool IsActive { get; set; }
    }
}
