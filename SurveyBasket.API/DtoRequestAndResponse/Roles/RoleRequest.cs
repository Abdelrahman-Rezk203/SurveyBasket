namespace SurveyBasket.API.DtoRequestAndResponse.Roles
{
    public record RoleRequest(
        string Name,
        IList<string> Permissions
        );
   
}
