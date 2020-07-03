using System.Linq;
using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<GoblinIdentityRegisterModel, UserEntity>().IgnoreAllNonExisting();
            
            CreateMap<GoblinIdentityUpdateProfileModel, UserEntity>().IgnoreAllNonExisting();
            
            CreateMap<UserEntity, GoblinIdentityUserModel>().IgnoreAllNonExisting()
                .ForMember(x => x.Roles, o => o.MapFrom(x => x.UserRoles.Select(y => y.Role.Name)))
                .ForMember(x => x.Permissions, o => o.MapFrom(x => x.UserRoles.SelectMany(y => y.Role.RolePermissions.Select(k => k.Permission.Name))));
        }
    }
}