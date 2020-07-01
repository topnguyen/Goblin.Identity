using System.Linq;
using Elect.DI.Attributes;
using Goblin.Identity.Contract.Repository.Interfaces;
using Goblin.Identity.Contract.Service;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.StringUtils;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Goblin.Core.DateTimeUtils;
using Goblin.Core.Errors;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Core;
using Goblin.Identity.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Identity.Service
{
    [ScopedDependency(ServiceType = typeof(IUserService))]
    public class UserService : Base.Service, IUserService
    {
        private readonly IGoblinRepository<UserEntity> _userRepo;

        public UserService(IGoblinUnitOfWork goblinUnitOfWork, IGoblinRepository<UserEntity> userRepo) : base(goblinUnitOfWork)
        {
            _userRepo = userRepo;
        }

        public async Task<GoblinIdentityRegisterResponseModel> RegisterAsync(GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default)
        {
            CheckUniqueEmail(model.Email);

            CheckUniqueUserName(model.UserName);
            
            var userEntity = model.MapTo<UserEntity>();

            // Set Password if have
            
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                userEntity.PasswordLastUpdatedTime = GoblinDateTimeHelper.SystemTimeNow;

                userEntity.PasswordHash = PasswordHelper.HashPassword(model.Password, userEntity.PasswordLastUpdatedTime.Value);
            }
            
            userEntity.EmailConfirmToken = StringHelper.Generate(6, false, false);
            
            userEntity.EmailConfirmTokenExpireTime = GoblinDateTimeHelper.SystemTimeNow.Add(SystemSetting.Current.EmailConfirmTokenLifetime);

            _userRepo.Add(userEntity);

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            
            var registerResponseModel = new GoblinIdentityRegisterResponseModel
            {
                Id = userEntity.Id,
                EmailConfirmToken = userEntity.EmailConfirmToken,
                EmailConfirmTokenExpireTime = userEntity.EmailConfirmTokenExpireTime
            };
            
            return registerResponseModel;
        }

        public async Task<GoblinIdentityUserModel> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            var userModel =
                await _userRepo
                    .Get(x => x.Id == id)
                    .QueryTo<GoblinIdentityUserModel>()
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            return userModel;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var userEntity =
                await _userRepo
                    .Get(x => x.Id == id)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            
            _userRepo.Delete(userEntity);

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        private void CheckUniqueUserName(string userName, long? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return;
            }
            
            var query = _userRepo.Get(x => x.UserName == userName);

            if (excludeId != null)
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isUnique = !query.Any();

            if (!isUnique)
            {
                throw new GoblinException(nameof(GoblinErrorCode.BadRequest), "UserName already exists");
            }
        }
        
        private void CheckUniqueEmail(string email, long? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }
            
            var query = _userRepo.Get(x => x.Email == email && x.EmailConfirmedTime != null);

            if (excludeId != null)
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isUnique = !query.Any();

            if (!isUnique)
            {
                throw new GoblinException(nameof(GoblinErrorCode.BadRequest), "Email already exists");
            }
        }
    }
}