namespace SurveyBasket.API.DtoRequestAndResponse
{
    public record ChangePasswordRequest(
        string CurrentPasword,
        string NewPassword
        );
   
}
