using Goblin.Identity.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goblin.Identity.Repository.Maps
{
    public class RoleEntityMap : GoblinEntityMap<RoleEntity>
    {
        public override void Map(EntityTypeBuilder<RoleEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(RoleEntity));

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}