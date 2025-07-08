namespace SurveyBasket.API.DtoRequestAndResponse.Roles
{
    public record RoleDetailsResponse(
        string Id,
        string Name,
        bool IsDeleted,
        IEnumerable<string> permissions
        );
    
}
