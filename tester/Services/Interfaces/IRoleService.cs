using tester.Models;

namespace tester.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<IEnumerable<RolePermission>> GetRolePermissionsAsync(int roleId);
        Task<IEnumerable<RolePermission>> GetAllRolePermissionsAsync();
    }
}
