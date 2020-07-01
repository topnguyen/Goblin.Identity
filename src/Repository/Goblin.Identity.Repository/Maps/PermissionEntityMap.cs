using Goblin.Identity.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goblin.Identity.Repository.Maps
{
    public class PermissionEntityMap : GoblinEntityMap<PermissionEntity>
    {
        public override void Map(EntityTypeBuilder<PermissionEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(PermissionEntity));

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}