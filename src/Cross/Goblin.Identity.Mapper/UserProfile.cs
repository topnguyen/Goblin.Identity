using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Share.Models;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<GoblinIdentityRegisterModel, UserEntity>().IgnoreAllNonExisting();
            
            CreateMap<GoblinIdentityUpdateProfileModel, UserEntity>().IgnoreAllNonExisting();
            
            CreateMap<UserEntity, GoblinIdentityUserModel>().IgnoreAllNonExisting();
        }
    }
}