using Elect.DI.Attributes;
using Goblin.Identity.Contract.Repository.Interfaces;
using Goblin.Identity.Contract.Service;
using System.Threading;
using System.Threading.Tasks;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Identity.Service
{
    [ScopedDependency(ServiceType = typeof(ISampleService))]
    public class SampleService : Base.Service, ISampleService
    {
        private readonly IGoblinRepository<SampleEntity> _sampleRepo;

        public SampleService(IGoblinUnitOfWork goblinUnitOfWork, IGoblinRepository<SampleEntity> sampleRepo) : base(
            goblinUnitOfWork)
        {
            _sampleRepo = sampleRepo;
        }

        public async Task<GoblinIdentitySampleModel> CreateAsync(GoblinIdentityCreateSampleModel model,
            CancellationToken cancellationToken = default)
        {
            var sampleEntity = model.MapTo<SampleEntity>();

            _sampleRepo.Add(sampleEntity);

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            var fileModel = sampleEntity.MapTo<GoblinIdentitySampleModel>();

            return fileModel;
        }

        public async Task<GoblinIdentitySampleModel> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            var sampleModel =
                await _sampleRepo
                    .Get(x => x.Id == id)
                    .QueryTo<GoblinIdentitySampleModel>()
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            return sampleModel;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var sampleEntity =
                await _sampleRepo
                    .Get(x => x.Id == id)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            
            _sampleRepo.Delete(sampleEntity);

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }
    }
}