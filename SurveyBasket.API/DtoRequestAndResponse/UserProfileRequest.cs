namespace SurveyBasket.API.DtoRequestAndResponse
{
    public record UserProfileRequest(
        string UserName,
        string Email,
        string FirstName,
        string LastName
        );
   
}
