namespace SurveyBasket.API.DtoRequestAndResponse
{
    public record RegisterRequest(
        string UserName,
        string Email,
        string Password,
        string FirstName,
        string LastName
        );
    
    
}
