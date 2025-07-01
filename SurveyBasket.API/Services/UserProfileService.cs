using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;

namespace SurveyBasket.API.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileService(ApplicationDbContext applicationDbContext , UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
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

    }
}
