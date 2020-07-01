using Goblin.Identity.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goblin.Identity.Repository.Maps
{
    public class UserRoleEntityMap : GoblinEntityMap<UserRoleEntity>
    {
        public override void Map(EntityTypeBuilder<UserRoleEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(UserRoleEntity));

            builder.HasIndex(x => x.UserId);
            
            builder.HasIndex(x => x.RoleId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId);
            
            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);
        }
    }
}