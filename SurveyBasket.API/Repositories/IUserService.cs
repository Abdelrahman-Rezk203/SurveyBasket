using SurveyBasket.API.Abstractions;
using SurveyBasket.API.DtoRequestAndResponse;
using SurveyBasket.API.DtoRequestAndResponse.Users;

namespace SurveyBasket.API.Repositories
{
    public interface IUserService
    {
        Task<Result<UserProfileResponse>> GetUserProfile(string UserId, CancellationToken cancellationToken = default);
        Task<Result>UpdateUserProfile(string UserId,UserProfileRequest  profileRequest , CancellationToken cancellationToken = default);
        Task<Result> ChangePassword(string UserId, ChangePasswordRequest changePassword, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync(bool? IncludeDisabled = false, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetUserAsync(string Id, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> AddNewUserAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default);
        Task<Result>UpdateUserAsync(string Id,UpdateUserRequest  updateUserRequest, CancellationToken cancellationToken = default);
        Task<Result> AddToggleStatusUserAsync(string Id, CancellationToken cancellationToken = default);
        Task<Result> UnLockUserAsync(string Id, CancellationToken cancellationToken = default);



    }
}
