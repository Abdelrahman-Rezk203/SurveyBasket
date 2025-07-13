namespace SurveyBasket.API.DtoRequestAndResponse.Users
{
    public record UserProfileRequest(
        string UserName,
        string Email,
        string FirstName,
        string LastName
        );
   
}
