

namespace bawabetak_backend.Repositories.Abstract
{
    public interface IUserRepository 
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

        Task<IdentityResult> ChangePassword(ApplicationUser user, string currentPassword, string newPassword);

        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword);

        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);

        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);

        Task<List<ApplicationUser>> GetAllUsersAsync();

        Task<ApplicationUser?> GetUserByEmailAsync(string email);

        Task<ApplicationUser?> GetUserByIdAsync(string userId);

        Task<bool> IsSamePasswordAsync(ApplicationUser user, string password);

        Task<bool> UserExistsAsync(string userId);
        Task MarkAsVerifiedAsync(string email);
        
    }
}