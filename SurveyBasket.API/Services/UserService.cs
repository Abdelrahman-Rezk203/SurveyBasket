using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.DtoRequestAndResponse;
using SurveyBasket.API.DtoRequestAndResponse.Users;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;

namespace SurveyBasket.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;

        public UserService(ApplicationDbContext applicationDbContext , UserManager<ApplicationUser> userManager , IRoleService roleService)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            this._roleService = roleService;
        }


        public async Task<Result<UserProfileResponse>> GetUserProfile(string UserId, CancellationToken cancellationToken = default)
        {
            //var UserInfo = await _applicationDbContext.Users.Where(x => x.Id == UserId)
            //                                                .Select(x => new
            //                                                {
            //                                                    x.UserName,
            //                                                    x.Email,
            //                                                    x.FirstName,
            //                                                    x.LastName

            //                                                }).FirstOrDefaultAsync(cancellationToken);

            var UserInfo = await _applicationDbContext.Users.Where(x=>x.Id == UserId)
                                                            .ProjectToType<UserProfileResponse>()
                                                            .FirstAsync();

            return Result.Success(UserInfo);
        }

        public async Task<Result> UpdateUserProfile(string UserId, UserProfileRequest profileRequest, CancellationToken cancellationToken = default)
        {
            //var User = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == UserId, cancellationToken);

            //if (User == null)
            //    return Result.Failure(UserErrors.UserNotFound);

            //User.FirstName = profileRequest.FirstName;
            //User.LastName = profileRequest.LastName;
            //User.Email = profileRequest.Email;
            //User.UserName = profileRequest.UserName;

            await _userManager.Users
                .Where(x => x.Id == UserId)
                .ExecuteUpdateAsync(setters =>
                                             setters
                                             .SetProperty(x => x.FirstName, profileRequest.FirstName)
                                             .SetProperty(x => x.LastName, profileRequest.LastName)

                );
            
            return Result.Success();

        }

        public async Task<Result> ChangePassword(string UserId, ChangePasswordRequest changePassword, CancellationToken cancellationToken = default)
        {
            var User = await _userManager.FindByIdAsync(UserId);
            var result = await _userManager.ChangePasswordAsync(User!, changePassword.CurrentPasword, changePassword.NewPassword);

            if(result.Succeeded)
                return Result.Success();

            return Result.Failure(UserErrors.ChangePaswordInvalid);
            
        }

        public async Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync(bool? IncludeDisabled = false, CancellationToken cancellationToken = default)
        {// نفذ عليها الكاش
            var AllUser = await (from user in _applicationDbContext.Users
                                 join userRole in _applicationDbContext.UserRoles
                                 on user.Id equals userRole.UserId
                                 join roles in _applicationDbContext.Roles
                                 on userRole.RoleId equals roles.Id into r
                                 where !r.Any(x => x.Name == DefaultRoles.Member) && (IncludeDisabled == true || !user.IsDisabled)
                                 select new
                                 {
                                     user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Email,
                                     user.IsDisabled,
                                     Roles = r.Select(x => x.Name).ToList()

                                 })
                                 .GroupBy(user => new { user.Id, user.Email, user.FirstName, user.LastName, user.IsDisabled }) 
                                 .Select(User => new UserResponse 
                                 (
                                     User.Key.Id,
                                     User.Key.FirstName,
                                     User.Key.LastName,
                                     User.Key.Email,
                                     User.Key.IsDisabled,
                                     User.SelectMany(x => x.Roles).ToList()
                                 ))
                                 .ToListAsync(cancellationToken);

            return Result.Success(AllUser.Adapt<IEnumerable<UserResponse>>());
        }

        public async Task<Result<UserResponse>> GetUserAsync(string Id , CancellationToken cancellationToken = default)
        {
            var UserIsExist = await _applicationDbContext.Users.FindAsync(Id, cancellationToken);

            if (UserIsExist is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            

            var UserRoles = await _userManager.GetRolesAsync(UserIsExist);

            var result = new UserResponse
                            (
                                UserIsExist.Id,
                                UserIsExist.Email!,
                                UserIsExist.FirstName,
                                UserIsExist.LastName,
                                UserIsExist.IsDisabled,
                                UserRoles
                            );

            return Result.Success(result);
        }

        public async Task<Result<UserResponse>> AddNewUserAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default)
        {
            
            var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == createUserRequest.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure<UserResponse>(UserErrors.DublicatedEmail);

            var AllowedRoles = await _applicationDbContext.Roles
                .Where(x => !x.IsDeleted).ToListAsync(cancellationToken);


            if (createUserRequest.Roles.Except(AllowedRoles.Select(x => x.Name)).Any())
                return Result.Failure<UserResponse>(UserErrors.InvalidRoles);


            var user = createUserRequest.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, createUserRequest.Password);//بتفهم ال Normalization from user 

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, createUserRequest.Roles);

                var response = (user, createUserRequest.Roles).Adapt<UserResponse>();

                return Result.Success(response);
            }

            var error = result.Errors.First();

            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> UpdateUserAsync(string Id , UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == updateUserRequest.Email && x.Id != Id, cancellationToken);

            if (emailIsExists)
                return Result.Failure<UserResponse>(UserErrors.DublicatedEmail);

            var AllowedRoles = await _applicationDbContext.Roles
                .Where(x => !x.IsDeleted).ToListAsync(cancellationToken);


            if (updateUserRequest.Roles.Except(AllowedRoles.Select(x => x.Name)).Any())
                return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

            var FindUser = await _userManager.FindByIdAsync(Id);

            if (FindUser == null)
                return Result.Failure(UserErrors.UserNotFound);

             FindUser = updateUserRequest.Adapt(FindUser); 

             FindUser.NormalizedEmail = updateUserRequest.Email.ToUpper();
             FindUser.NormalizedUserName = updateUserRequest.UserName.ToUpper();

            //FindUser!.Email = updateUserRequest.Email;
            //FindUser.UserName = updateUserRequest.UserName;
            //FindUser.FirstName = updateUserRequest.FirstName;
            //FindUser.LastName = updateUserRequest.LastName;

            var UpdateUser = await _userManager.UpdateAsync(FindUser);

            if(UpdateUser.Succeeded)
            {
               await _applicationDbContext.UserRoles
                    .Where(x=>x.UserId == Id)
                    .ExecuteDeleteAsync(cancellationToken); 

                await _userManager.AddToRolesAsync(FindUser , updateUserRequest.Roles);

                var response = (FindUser , updateUserRequest.Roles).Adapt<UserResponse>();
                return Result.Success(response);
            }

            var error = UpdateUser.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }


        public async Task<Result> AddToggleStatusUserAsync(string Id, CancellationToken cancellationToken = default)
        {
            var UserIsExist = await _applicationDbContext.Users.FindAsync(Id);

            if (UserIsExist is null)
                return Result.Failure(UserErrors.UserNotFound);

            UserIsExist.IsDisabled = !UserIsExist.IsDisabled;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
          
            return Result.Success(UserIsExist);
        }

        public async Task<Result> UnLockUserAsync(string Id, CancellationToken cancellationToken = default)
        { 
            var UserIsExist = await _applicationDbContext.Users.FindAsync(Id);

            if (UserIsExist is null)
                return Result.Failure(UserErrors.UserNotFound);

            UserIsExist.LockoutEnabled = false;
            UserIsExist.LockoutEnd = null;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(UserIsExist);
        }



    }
}
