using Goblin.Identity.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Identity.Repository
{
    public sealed partial class GoblinDbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<RoleEntity> Roles { get; set; }
        
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        
        public DbSet<PermissionEntity> Permissions { get; set; }
        
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }
    }
}