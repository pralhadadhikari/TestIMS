namespace IMS.API.DTOs
{
    public class AuthResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public UserInfo User { get; set; }
    }

    public class UserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int StoreId { get; set; }

        public string UserRoleId { get; set; }

        public string ProfileUrl { get; set; }
        public string Address { get; set; }
    }
}
