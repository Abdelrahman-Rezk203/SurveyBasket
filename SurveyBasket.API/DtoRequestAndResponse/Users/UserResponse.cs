namespace SurveyBasket.API.DtoRequestAndResponse.Users
{
    public record UserResponse(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        bool IsDisabled,
        IList<string> Roles
        );
    
}
