using System.Linq;
using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Share.Models.RoleModels;

namespace Goblin.Identity.Mapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<GoblinIdentityUpsertRoleModel, RoleEntity>().IgnoreAllNonExisting();

            CreateMap<RoleEntity, GoblinIdentityRoleModel>().IgnoreAllNonExisting()
                .ForMember(x => x.Permissions, o => o.MapFrom(x => x.RolePermissions.Select(y => y.Permission.Name)));
        }
    }
}