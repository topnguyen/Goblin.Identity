using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<GoblinIdentityRegisterModel, UserEntity>().IgnoreAllNonExisting();
            
            CreateMap<UserEntity, GoblinIdentityUserModel>().IgnoreAllNonExisting();
        }
    }
}