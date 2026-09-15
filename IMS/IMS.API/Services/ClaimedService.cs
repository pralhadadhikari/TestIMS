using IIMS.Application.Common.Interface;
using IMS.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IMS.API.Services
{
    public class ClaimedService:IClaimedService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimedService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string UserId =>
            _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

        public string Role =>
            _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Role);

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            if (string.IsNullOrEmpty(UserId))
                return null;

            return await _userManager.FindByIdAsync(UserId);
        }

        public async Task<int> GetStoreIdAsync()
        {
            var user = await GetCurrentUserAsync();

            return user?.StoreId ?? 0;
        }
    }
}
