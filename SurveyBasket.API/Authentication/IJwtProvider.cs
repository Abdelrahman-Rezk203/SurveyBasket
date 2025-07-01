using Microsoft.Extensions.Options;
using SurveyBasket.API.Entities;

namespace SurveyBasket.API.Authentication
{
    public interface IJwtProvider
    {
        public (string token, int expireIn) GenerateToken(ApplicationUser applicationUser,IConfiguration configuration);
        public string? ValidateToken(string token);

    }
}
