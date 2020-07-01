using Goblin.Identity.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goblin.Identity.Repository.Maps
{
    public class RolePermissionEntityMap : GoblinEntityMap<RolePermissionEntity>
    {
        public override void Map(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(RolePermissionEntity));

            builder.HasIndex(x => x.RoleId);
            
            builder.HasIndex(x => x.PermissionId);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId);
            
            builder.HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId);
        }
    }
}