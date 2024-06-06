namespace IMS.Models.ViewModels
{
    public class UserInfo
    {
        public string Id { get;set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int StoreId { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
