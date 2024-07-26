using Microsoft.AspNetCore.Authorization;

namespace tester.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(string permission) 
        {
            Policy = permission;
        }
    }
}
