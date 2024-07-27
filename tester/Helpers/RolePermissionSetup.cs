using System.Security.Claims;
using tester.Models;

public static class RolePermissionSetup
{
    public static void AddRolePermissionPolicies(this IServiceCollection services, IEnumerable<Role> roles, IEnumerable<Permission> permissions, IEnumerable<RolePermission> rolePermissions)
    {
        var permissionRoles = new Dictionary<string, List<string>>();

        // Initialize the dictionary
        foreach (var permission in permissions)
        {
            permissionRoles[permission.PermissionName] = new List<string>();
        }

        // Map permissions to roles
        foreach (var rp in rolePermissions)
        {
            var role = roles.FirstOrDefault(r => r.RoleId == rp.RoleId);
            var permission = permissions.FirstOrDefault(p => p.PermissionId == rp.PermissionId);
            if (role != null && permission != null)
            {
                if (!permissionRoles.ContainsKey(permission.PermissionName))
                {
                    permissionRoles[permission.PermissionName] = new List<string>();
                }
                permissionRoles[permission.PermissionName].Add(role.RoleName);
            }
        }

        // Add authorization policies dynamically based on roles and permissions
        services.AddAuthorization(options =>
        {
            foreach (var permission in permissionRoles)
            {
                options.AddPolicy(permission.Key, policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && permission.Value.Contains(c.Value))));
            }
        });
    }
}
