using System.Security.Claims;

namespace SurveyBasket.API.Extentions
{
    public static class UserExtention
    {
        public static string? GetUserId(this ClaimsPrincipal User )
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return UserId;
        }
    }
}
