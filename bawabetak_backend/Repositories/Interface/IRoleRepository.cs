
namespace bawabetak_backend.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<List<GetRolesDto>> GetAllRolesAsync();
        Task <IList<string>>GetRolesByUserAsync(ApplicationUser user);
        public Task AddUserToRoleAsync(ApplicationUser user, string roleName);
        public Task CreatRoleAsync(string roleName);
        public Task<bool> RoleExistsAsync(string roleName);

    }
}
