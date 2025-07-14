using OneOf;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.Dto;
using SurveyBasket.API.DtoRequestAndResponse;

namespace SurveyBasket.API.Repositories
{
    public interface IAuth
    {
        Task<OneOf<AuthResponse,Error>> GetTokenAsync(string email, string Password, CancellationToken cancellationToken = default);
        Task<OneOf<bool,Error>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<OneOf<AuthResponse,Error>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<OneOf<bool,Error>> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<OneOf<bool, Error>> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);
        Task<OneOf<bool, Error>> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default);
        Task<OneOf<bool, Error>> SendResetPasswordCodeAsync(ForgetPasswordRequest forgetPasswordRequest);
        Task<OneOf<bool, Error>> ResetPasswordAsync(ResetPasswordRequest resetPassword);

    }
}
