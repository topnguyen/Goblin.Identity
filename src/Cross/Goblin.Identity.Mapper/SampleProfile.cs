using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Mapper
{
    public class SampleProfile : Profile
    {
        public SampleProfile()
        {
            CreateMap<GoblinIdentityCreateSampleModel, SampleEntity>()
                .IgnoreAllNonExisting();
            
            CreateMap<SampleEntity, GoblinIdentitySampleModel>()
                .IgnoreAllNonExisting();
        }
    }
}