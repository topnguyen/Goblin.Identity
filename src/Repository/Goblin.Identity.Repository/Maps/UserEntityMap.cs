using Goblin.Identity.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goblin.Identity.Repository.Maps
{
    public class UserEntityMap : GoblinEntityMap<UserEntity>
    {
        public override void Map(EntityTypeBuilder<UserEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(UserEntity));

            builder.HasIndex(x => x.UserName).IsUnique();
            
            builder.HasIndex(x => x.Email);
        }
    }
}