using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
using Goblin.Core.Models;
using Goblin.Core.Utils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Core;
using Goblin.Identity.Share;
using Goblin.Identity.Share.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Identity.Service
{
    [ScopedDependency(ServiceType = typeof(IUserService))]
    public class UserService : Base.Service, IUserService
    {
        private readonly IGoblinRepository<UserEntity> _userRepo;
        private readonly IGoblinRepository<RoleEntity> _roleRepo;
        private readonly IGoblinRepository<UserRoleEntity> _userRoleRepo;

        public UserService(IGoblinUnitOfWork goblinUnitOfWork,
            IGoblinRepository<UserEntity> userRepo,
            IGoblinRepository<RoleEntity> roleRepo,
            IGoblinRepository<UserRoleEntity> userRoleRepo
        ) : base(
            goblinUnitOfWork)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
        }

        public Task<GoblinApiPagedResponseModel<GoblinIdentityUserModel>> GetPagedAsync(
            GoblinIdentityGetPagedUserModel model, CancellationToken cancellationToken = default)
        {
            var query = _userRepo.Get();

            // Filters

            if (model.Email != null)
            {
                query = query.Where(x => x.Email.Contains(model.Email));
            }

            if (model.FullName != null)
            {
                query = query.Where(x => x.FullName.Contains(model.FullName));
            }

            if (model.UserName != null)
            {
                query = query.Where(x => x.UserName.Contains(model.UserName));
            }

            // Excludes

            var excludeIds = model.ExcludeIds.ToList<long>(long.TryParse);

            if (excludeIds.Any())
            {
                query = query.Where(x => !excludeIds.Contains(x.Id));
            }

            var includeIds = model.IncludeIds.ToList<long>(long.TryParse);

            // Includes

            if (includeIds.Any())
            {
                var includeIdsQuery = _userRepo.Get(x => includeIds.Contains(x.Id));

                query = query.Union(includeIdsQuery);
            }

            // Build Model Query

            var modelQuery = query.QueryTo<GoblinIdentityUserModel>();

            // Order

            var sortByDirection = model.IsSortAscending ? "ASC" : "DESC";

            var orderByDynamicStatement = $"{model.SortBy} {sortByDirection}";

            modelQuery = !string.IsNullOrWhiteSpace(model.SortBy)
                ? modelQuery.OrderBy(orderByDynamicStatement)
                : modelQuery.OrderByDescending(x => x.LastUpdatedTime);

            // Get Result

            var total = query.LongCount();

            var items =
                modelQuery
                    .Skip(model.Skip)
                    .Take(model.Take)
                    .ToList();

            var pagedResponse = new GoblinApiPagedResponseModel<GoblinIdentityUserModel>
            {
                Total = total,
                Items = items
            };

            return Task.FromResult(pagedResponse);
        }

        public async Task<GoblinIdentityEmailConfirmationModel> RegisterAsync(GoblinIdentityRegisterModel model,
            CancellationToken cancellationToken = default)
        {
            model.Email = model.Email?.Trim().ToLowerInvariant();

            model.UserName = model.UserName?.Trim().ToLowerInvariant();

            CheckUniqueEmail(model.Email);

            CheckUniqueUserName(model.UserName);

            using var transaction =
                await GoblinUnitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(true);

            var userEntity = model.MapTo<UserEntity>();

            userEntity.PasswordLastUpdatedTime =
                userEntity.RevokeTokenGeneratedBeforeTime = GoblinDateTimeHelper.SystemTimeNow;

            userEntity.PasswordHash =
                PasswordHelper.HashPassword(model.Password, userEntity.PasswordLastUpdatedTime);

            userEntity.EmailConfirmToken = StringHelper.Generate(6, false, false);

            userEntity.EmailConfirmTokenExpireTime =
                GoblinDateTimeHelper.SystemTimeNow.Add(SystemSetting.Current.EmailConfirmTokenLifetime);

            _userRepo.Add(userEntity);

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            // User Roles

            if (model.Roles?.Any() == true)
            {
                model.Roles = model.Roles.Select(x => x.Trim()).ToList();

                var roleEntities = await _roleRepo.Get(x => model.Roles.Contains(x.Name)).ToListAsync(cancellationToken)
                    .ConfigureAwait(true);

                foreach (var roleEntity in roleEntities)
                {
                    _userRoleRepo.Add(new UserRoleEntity
                    {
                        UserId = userEntity.Id,
                        RoleId = roleEntity.Id
                    });
                }

                await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            }

            transaction.Commit();

            // Email Confirmation Code

            var emailConfirmationModel = new GoblinIdentityEmailConfirmationModel
            {
                Id = userEntity.Id,
                EmailConfirmToken = userEntity.EmailConfirmToken,
                EmailConfirmTokenExpireTime = userEntity.EmailConfirmTokenExpireTime
            };

            return emailConfirmationModel;
        }

        public async Task<GoblinIdentityUserModel> GetProfileAsync(long id,
            CancellationToken cancellationToken = default)
        {
            var userModel =
                await _userRepo
                    .Get(x => x.Id == id)
                    .QueryTo<GoblinIdentityUserModel>()
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            return userModel;
        }

        public async Task UpdateProfileAsync(long id, GoblinIdentityUpdateProfileModel model,
            CancellationToken cancellationToken = default)
        {
            var userEntity = await _userRepo.Get(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(true);

            if (userEntity == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            using var transaction =
                await GoblinUnitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(true);

            if (model.IsUpdateRoles)
            {
                _userRoleRepo.DeleteWhere(x => x.UserId == userEntity.Id);

                await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

                // User Roles

                if (model.Roles?.Any() == true)
                {
                    model.Roles = model.Roles.Select(x => x.Trim()).ToList();

                    var roleEntities = await _roleRepo.Get(x => model.Roles.Contains(x.Name))
                        .ToListAsync(cancellationToken).ConfigureAwait(true);

                    foreach (var roleEntity in roleEntities)
                    {
                        _userRoleRepo.Add(new UserRoleEntity
                        {
                            UserId = userEntity.Id,
                            RoleId = roleEntity.Id
                        });
                    }
                }
            }

            model.MapTo(userEntity);

            _userRepo.Update(userEntity,
                x => x.AvatarUrl,
                x => x.FullName,
                x => x.Bio,
                x => x.GithubId,
                x => x.SkypeId,
                x => x.FacebookId,
                x => x.WebsiteUrl,
                x => x.CompanyName,
                x => x.CompanyUrl
            );

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            transaction.Commit();
        }

        public async Task<GoblinIdentityEmailConfirmationModel> UpdateIdentityAsync(long id,
            GoblinIdentityUpdateIdentityModel model,
            CancellationToken cancellationToken = default)
        {
            model.NewEmail = model.NewEmail?.Trim().ToLowerInvariant();

            model.NewUserName = model.NewUserName?.Trim().ToLowerInvariant();

            var userEntity = await _userRepo.Get(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(true);

            if (userEntity == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            var emailConfirmationModel = new GoblinIdentityEmailConfirmationModel
            {
                Id = userEntity.Id
            };

            var changedProperties = new List<string>();

            // Update Password
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var newPasswordHashWithOldSalt =
                    PasswordHelper.HashPassword(model.NewPassword, userEntity.PasswordLastUpdatedTime);

                // If user have changed password, then update password and related information
                if (newPasswordHashWithOldSalt != userEntity.PasswordHash)
                {
                    userEntity.PasswordLastUpdatedTime =
                        userEntity.RevokeTokenGeneratedBeforeTime = GoblinDateTimeHelper.SystemTimeNow;
                    changedProperties.Add(nameof(userEntity.PasswordLastUpdatedTime));
                    changedProperties.Add(nameof(userEntity.RevokeTokenGeneratedBeforeTime));

                    userEntity.PasswordHash =
                        PasswordHelper.HashPassword(model.NewPassword, userEntity.PasswordLastUpdatedTime);
                    changedProperties.Add(nameof(userEntity.PasswordHash));
                }
            }

            // Update Email
            if (!string.IsNullOrWhiteSpace(model.NewEmail) && model.NewEmail != userEntity.Email)
            {
                userEntity.EmailConfirmToken = StringHelper.Generate(6, false, false);
                changedProperties.Add(nameof(userEntity.EmailConfirmToken));

                userEntity.EmailConfirmTokenExpireTime =
                    GoblinDateTimeHelper.SystemTimeNow.Add(SystemSetting.Current.EmailConfirmTokenLifetime);
                changedProperties.Add(nameof(userEntity.EmailConfirmTokenExpireTime));


                // Email Confirmation Token

                emailConfirmationModel.EmailConfirmToken = userEntity.EmailConfirmToken;

                emailConfirmationModel.EmailConfirmTokenExpireTime = userEntity.EmailConfirmTokenExpireTime;
            }

            // Update UserName
            if (!string.IsNullOrWhiteSpace(model.NewUserName))
            {
                userEntity.UserName = model.NewUserName;
                changedProperties.Add(nameof(userEntity.UserName));
            }

            if (!changedProperties.Any())
            {
                return emailConfirmationModel;
            }

            _userRepo.Update(userEntity, changedProperties.ToArray());

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return emailConfirmationModel;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var userEntity =
                await _userRepo
                    .Get(x => x.Id == id)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            if (userEntity == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            _userRepo.Delete(userEntity);

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task ConfirmEmail(GoblinIdentityConfirmEmailModel model,
            CancellationToken cancellationToken = default)
        {
            var userEntity = await _userRepo.Get(x => x.Id == model.Id)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(true);

            if (userEntity == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            if (userEntity.EmailConfirmToken == model.EmailConfirmToken)
            {
                if (userEntity.EmailConfirmTokenExpireTime < GoblinDateTimeHelper.SystemTimeNow)
                {
                    throw new GoblinException(nameof(GoblinIdentityErrorCode.ConfirmEmailTokenExpired),
                        GoblinIdentityErrorCode.ConfirmEmailTokenExpired);
                }
            }
            else
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.ConfirmEmailTokenInCorrect),
                    GoblinIdentityErrorCode.ConfirmEmailTokenInCorrect);
            }

            userEntity.EmailConfirmToken = null;
            userEntity.EmailConfirmTokenExpireTime = null;
            userEntity.EmailConfirmedTime = GoblinDateTimeHelper.SystemTimeNow;

            _userRepo.Update(userEntity,
                x => x.EmailConfirmToken,
                x => x.EmailConfirmTokenExpireTime,
                x => x.EmailConfirmedTime
            );

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task<GoblinIdentityResetPasswordTokenModel> RequestResetPasswordAsync(
            GoblinIdentityRequestResetPasswordModel model,
            CancellationToken cancellationToken = default)
        {
            var userEntity = await _userRepo.Get(x => x.Email == model.Email && x.EmailConfirmedTime != null)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(true);

            if (userEntity == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            var resetPasswordTokenModel = new GoblinIdentityResetPasswordTokenModel { };

            resetPasswordTokenModel.SetPasswordToken =
                userEntity.SetPasswordToken = StringHelper.Generate(6, false, false);

            resetPasswordTokenModel.SetPasswordTokenExpireTime = userEntity.SetPasswordTokenExpireTime =
                GoblinDateTimeHelper.SystemTimeNow.Add(SystemSetting.Current.SetPasswordTokenLifetime);

            _userRepo.Update(userEntity,
                x => x.SetPasswordToken,
                x => x.SetPasswordTokenExpireTime
            );

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            return resetPasswordTokenModel;
        }

        public async Task ResetPasswordAsync(GoblinIdentityResetPasswordModel model,
            CancellationToken cancellationToken = default)
        {
            var userEntity = await _userRepo.Get(x => x.Email == model.Email && x.EmailConfirmedTime != null)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(true);

            if (userEntity == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            if (userEntity.SetPasswordToken == model.SetPasswordToken)
            {
                if (userEntity.SetPasswordTokenExpireTime < GoblinDateTimeHelper.SystemTimeNow)
                {
                    throw new GoblinException(nameof(GoblinIdentityErrorCode.SetPasswordTokenExpired),
                        GoblinIdentityErrorCode.SetPasswordTokenExpired);
                }
            }
            else
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.SetPasswordTokenInCorrect),
                    GoblinIdentityErrorCode.SetPasswordTokenInCorrect);
            }

            var changedProperties = new List<string>();

            var newPasswordHashWithOldSalt =
                PasswordHelper.HashPassword(model.NewPassword, userEntity.PasswordLastUpdatedTime);

            // If user have changed password, then update password and related information
            if (newPasswordHashWithOldSalt != userEntity.PasswordHash)
            {
                userEntity.PasswordLastUpdatedTime =
                    userEntity.RevokeTokenGeneratedBeforeTime = GoblinDateTimeHelper.SystemTimeNow;
                changedProperties.Add(nameof(userEntity.PasswordLastUpdatedTime));
                changedProperties.Add(nameof(userEntity.RevokeTokenGeneratedBeforeTime));

                userEntity.PasswordHash =
                    PasswordHelper.HashPassword(model.NewPassword, userEntity.PasswordLastUpdatedTime);
                changedProperties.Add(nameof(userEntity.PasswordHash));
            }

            userEntity.SetPasswordToken = null;
            changedProperties.Add(nameof(userEntity.SetPasswordToken));

            userEntity.SetPasswordTokenExpireTime = null;
            changedProperties.Add(nameof(userEntity.SetPasswordTokenExpireTime));

            _userRepo.Update(userEntity, changedProperties.ToArray());

            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        }

        public async Task<string> GenerateAccessTokenAsync(GoblinIdentityGenerateAccessTokenModel model,
            CancellationToken cancellationToken = default)
        {
            var userEntity = await _userRepo.Get(x => x.UserName == model.UserName)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(true);

            // Check User is exist

            if (userEntity == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            // Compare password hash from request and database

            var passwordHash = PasswordHelper.HashPassword(model.Password, userEntity.PasswordLastUpdatedTime);

            if (passwordHash != userEntity.PasswordHash)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.WrongPassword),
                    GoblinIdentityErrorCode.WrongPassword);
            }

            // Generate Access Token

            var now = GoblinDateTimeHelper.SystemTimeNow;

            var accessTokenData = new TokenDataModel<AccessTokenDataModel>
            {
                ExpireTime = now.Add(SystemSetting.Current.AccessTokenLifetime),
                CreatedTime = now,
                Data = new AccessTokenDataModel
                {
                    UserId = userEntity.Id
                }
            };

            var accessToken = JwtHelper.Generate(accessTokenData);

            return accessToken;
        }

        public async Task<GoblinIdentityUserModel> GetProfileByAccessTokenAsync(string accessToken,
            CancellationToken cancellationToken = default)
        {
            var accessTokenDataModel = JwtHelper.Get<TokenDataModel<AccessTokenDataModel>>(accessToken);

            // Check Valid

            if (accessTokenDataModel == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.AccessTokenIsInvalid),
                    GoblinIdentityErrorCode.AccessTokenIsInvalid);
            }

            // Check Expire

            if (accessTokenDataModel.ExpireTime < GoblinDateTimeHelper.SystemTimeNow)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.AccessTokenIsExpired),
                    GoblinIdentityErrorCode.AccessTokenIsExpired);
            }

            var userModel =
                await _userRepo
                    .Get(x => x.Id == accessTokenDataModel.Data.UserId)
                    .QueryTo<GoblinIdentityUserModel>()
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            if (accessTokenDataModel.CreatedTime < userModel.RevokeTokenGeneratedBeforeTime)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.AccessTokenIsRevoked),
                    GoblinIdentityErrorCode.AccessTokenIsRevoked);
            }
            // If user not found, then ignore

            if (userModel == null)
            {
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNotFound),
                    GoblinIdentityErrorCode.UserNotFound);
            }

            return userModel;
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
                throw new GoblinException(nameof(GoblinIdentityErrorCode.UserNameNotUnique),
                    GoblinIdentityErrorCode.UserNameNotUnique);
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
                throw new GoblinException(nameof(GoblinIdentityErrorCode.EmailNotUnique),
                    GoblinIdentityErrorCode.EmailNotUnique);
            }
        }
    }
}