namespace SurveyBasket.API.DtoRequestAndResponse.Users
{
    public record CreateUserRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string UserName,
        IList<string> Roles
        );
   
}
