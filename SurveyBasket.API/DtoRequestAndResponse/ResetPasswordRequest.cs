namespace SurveyBasket.API.DtoRequestAndResponse
{
    public record ResetPasswordRequest(
        string Email,
        string Code,
        string NewPassword
        );
    
}
