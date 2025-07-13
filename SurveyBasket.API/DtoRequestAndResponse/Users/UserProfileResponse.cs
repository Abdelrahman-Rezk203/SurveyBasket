namespace SurveyBasket.API.DtoRequestAndResponse.Users
{
    public record UserProfileResponse(
        string UserName,
        string Email,
        string FirstName,
        string LastName
        );
    
}
