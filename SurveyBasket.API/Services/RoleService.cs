using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.DtoRequestAndResponse.Roles;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;

namespace SurveyBasket.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public RoleService(RoleManager<ApplicationRole> roleManager , ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            this._dbContext = dbContext;
        }

        public async Task<Result<RoleDetailsResponse>> AddRoleAsync(RoleRequest request, CancellationToken cancellationToken = default)
        {
            var roleIfExsit = await _roleManager.RoleExistsAsync(request.Name);
            if(roleIfExsit)
                return Result.Failure<RoleDetailsResponse>(RoleError.DuplicatedRole);

            var allPermissions = Permissions.GetAllPermissions();

            if(request.Permissions.Except(allPermissions).Any())
                return Result.Failure<RoleDetailsResponse>(RoleError.InvalidPermissions);

            var NewRole = new ApplicationRole()
            {
                Name = request.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            var AddRole = await _roleManager.CreateAsync(NewRole);

            if(AddRole.Succeeded)
            {
                var Permission = request.Permissions.Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = NewRole.Id
                });

                await _dbContext.AddRangeAsync(Permission, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            if(!AddRole.Succeeded)
               return Result.Failure<RoleDetailsResponse>(RoleError.AddRoleFailure);
            var response = new RoleDetailsResponse(NewRole.Id, NewRole.Name, NewRole.IsDeleted, request.Permissions);
            return Result.Success(response);

        }

        public async Task<Result> UpdateRoleAsync(string Id, RoleRequest request, CancellationToken cancellationToken = default)
        {
           var RoleNameIsExist = await _roleManager.Roles.AnyAsync(x=>x.Name == request.Name &&  x.Id != Id);
            if(RoleNameIsExist)
                return Result.Failure(RoleError.DuplicatedRole);

            var RoleIdFound = await _roleManager.FindByIdAsync(Id);
            if(RoleIdFound is null)
                return Result.Failure(RoleError.RoleNotFound);

            var allPermission = Permissions.GetAllPermissions();

            if(request.Permissions.Except(allPermission).Any())
                return Result.Failure(RoleError.InvalidPermissions);

            RoleIdFound.Name = request.Name;

            var UpdateRole = await _roleManager.UpdateAsync(RoleIdFound);

            if(UpdateRole.Succeeded)
            {
                var currentPermission = await _dbContext.RoleClaims
                    .Where(x => x.RoleId == Id && x.ClaimType == Permissions.Type)
                    .Select(x=>x.ClaimValue)
                    .ToListAsync(cancellationToken);

                var newPermission = request.Permissions.Except(currentPermission).Select(x => new IdentityRoleClaim<string>
                {
                    RoleId = RoleIdFound.Id,
                    ClaimValue = x,
                    ClaimType = Permissions.Type
                });

                var removePermission = currentPermission.Except(request.Permissions);

                await _dbContext.RoleClaims
                    .Where(x=>x.RoleId==Id && removePermission.Contains(x.ClaimValue))
                    .ExecuteDeleteAsync(cancellationToken);

                await _dbContext.AddRangeAsync(newPermission);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success(UpdateRole);
            }


            if (!UpdateRole.Succeeded)
                return Result.Failure(RoleError.UpdateRoleFailure);

            return Result.Success(UpdateRole);
        }


        public async Task<Result<IEnumerable<RoleResponse>>> GetAllRolesAsync(bool? IncludeDisable = false, CancellationToken cancellationToken = default)
        {
            var roles = await _roleManager.Roles
                .Where(x => !x.IsDefault && (!x.IsDeleted || IncludeDisable == true)).ToListAsync(cancellationToken);
            return Result.Success(roles.Adapt<IEnumerable<RoleResponse>>());
        
        }

        public async Task<Result<RoleDetailsResponse>> GetRolesDetailsAsync(string Id, CancellationToken cancellationToken = default)
        {
            var RolesDetails = await _roleManager.FindByIdAsync(Id);
            if (RolesDetails == null)
                return Result.Failure<RoleDetailsResponse>(RoleError.RoleNotFound);

            var Claims = await _roleManager.GetClaimsAsync(RolesDetails);

            var response = new RoleDetailsResponse(RolesDetails.Id, RolesDetails.Name!, RolesDetails.IsDeleted, Claims.Select(x => x.Value));
            return Result.Success(response);
        }

        public async Task<Result> AddToggleStatusForRoleAsync(string Id, CancellationToken cancellationToken = default)
        {
            var RoleIsFound = await _roleManager.FindByIdAsync(Id);
            if(RoleIsFound is null)
                return Result.Failure(RoleError.RoleNotFound);

            RoleIsFound.IsDeleted = !RoleIsFound.IsDeleted;

            await _dbContext.SaveChangesAsync(cancellationToken);

            //await _roleManager.UpdateAsync(RoleIsFound); // دا بدل السطرين اللي فوق 

            return Result.Success(RoleIsFound);
        }
    }
}
