namespace SurveyBasket.API.DtoRequestAndResponse
{
    public record ConfirmEmailRequest(
        string UserId,
        string Code
        );
    
}
