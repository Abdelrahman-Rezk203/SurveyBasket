using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.Repositories
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileResponse>> GetUserProfile(string UserId, CancellationToken cancellationToken = default);
        Task<Result>UpdateUserProfile(string UserId,UserProfileRequest  profileRequest , CancellationToken cancellationToken = default);
        Task<Result> ChangePassword(string UserId, ChangePasswordRequest changePassword, CancellationToken cancellationToken = default);
        
    }
}
