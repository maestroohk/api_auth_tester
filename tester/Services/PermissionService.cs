using Microsoft.EntityFrameworkCore;
using tester.Data;
using tester.Models;
using tester.Services.Interfaces;

namespace tester.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;

        public PermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
           return await _context.Permissions.ToListAsync();
        }
    }
}
