namespace SurveyBasket.API.DtoRequestAndResponse
{
    public record UserProfileResponse(
        string UserName,
        string Email,
        string FirstName,
        string LastName
        );
    
}
