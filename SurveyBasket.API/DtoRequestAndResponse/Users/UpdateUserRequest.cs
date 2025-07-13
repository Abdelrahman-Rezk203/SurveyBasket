namespace SurveyBasket.API.DtoRequestAndResponse.Users
{
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string Email, 
        string UserName,
        IList<string> Roles
        );
  
}
