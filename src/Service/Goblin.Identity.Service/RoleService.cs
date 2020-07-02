using System.Collections.Generic;
using System.Linq;
using Elect.DI.Attributes;
using Goblin.Identity.Contract.Repository.Interfaces;
using Goblin.Identity.Contract.Service;
using System.Threading;
using System.Threading.Tasks;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Goblin.Identity.Contract.Repository.Models;
using Goblin.Identity.Share.Models.RoleModels;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Identity.Service
{
    [ScopedDependency(ServiceType = typeof(IRoleService))]
    public class RoleService : Base.Service, IRoleService
    {
        private readonly IGoblinRepository<PermissionEntity> _permissionRepo;
        private readonly IGoblinRepository<RolePermissionEntity> _rolePermissionRepo;
        private readonly IGoblinRepository<RoleEntity> _roleRepo;

        public RoleService(IGoblinUnitOfWork goblinUnitOfWork,
            IGoblinRepository<PermissionEntity> permissionRepo,
            IGoblinRepository<RolePermissionEntity> rolePermissionRepo,
            IGoblinRepository<RoleEntity> roleRepo) : base(goblinUnitOfWork)
        {
            _permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
            _roleRepo = roleRepo;
        }

        public async Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _roleRepo
                .Get()
                .Select(x => x.Name)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return roles;
        }

        public async Task<GoblinIdentityRoleModel> UpsertAsync(GoblinIdentityUpsertRoleModel model,
            CancellationToken cancellationToken = default)
        {
            using var transaction =
                await GoblinUnitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(true);

            // Handle Role

            var roleEntity = await _roleRepo
                .Get(x => x.Name == model.Name)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(true);

            if (roleEntity == null)
            {
                roleEntity = new RoleEntity
                {
                    Name = model.Name
                };

                _roleRepo.Add(roleEntity);
            }

            // Save Change
            await GoblinUnitOfWork.SaveChangesAsync(cancellationToken);

            // Handle Permission

            if (model.Permissions?.Any() == true)
            {
                model.Permissions = model.Permissions.Select(x => x.Trim()).ToList();

                var existsPermissionEntities =
                    await _permissionRepo
                        .Get(x => model.Permissions.Contains(x.Name))
                        .ToListAsync(cancellationToken).ConfigureAwait(true);

                var existsPermissions = existsPermissionEntities.Select(x => x.Name).ToList();

                foreach (var permission in model.Permissions)
                {
                    if (!existsPermissions.Contains(permission))
                    {
                        var permissionEntity = new PermissionEntity
                        {
                            Name = permission
                        };

                        _permissionRepo.Add(permissionEntity);

                        existsPermissionEntities.Add(permissionEntity);
                    }
                }

                // Save Change
                await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

                // Relationship Role and Permission

                var rolePermissions = await _rolePermissionRepo.Get(x => x.RoleId == roleEntity.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(true);
                
                foreach (var permissionEntity in existsPermissionEntities)
                {
                    if (rolePermissions.Any(x => x.PermissionId == permissionEntity.Id))
                    {
                        continue;
                    }
                    
                    _rolePermissionRepo.Add(new RolePermissionEntity
                    {
                        RoleId = roleEntity.Id,
                        PermissionId = permissionEntity.Id
                    });
                }

                // Save Change
                await GoblinUnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            }

            transaction.Commit();

            var roleModel = await GetAsync(roleEntity.Name, cancellationToken).ConfigureAwait(true);

            return roleModel;
        }

        public async Task<GoblinIdentityRoleModel> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            name = name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var role = await _roleRepo.Get(x => x.Name == name).QueryTo<GoblinIdentityRoleModel>()
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(true);


            return role;
        }

        public async Task<List<string>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        {
            var permissions = await _permissionRepo.Get().Select(x => x.Name).ToListAsync(cancellationToken)
                .ConfigureAwait(true);

            return permissions;
        }
    }
}