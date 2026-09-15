
namespace IIMS.Application.Common.Interface
{
    public interface IClaimedService
    {
        string UserId { get; }

        string Role { get; }

        Task<int> GetStoreIdAsync();
    }
}
