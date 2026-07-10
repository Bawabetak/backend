

namespace bawabetak_backend.Repositories.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
           return  await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CreatRoleAsync(string roleName)
        {
             await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<List<GetRolesDto>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(r=> new GetRolesDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync();
        }

        public Task<IList<string>> GetRolesByUserAsync(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
