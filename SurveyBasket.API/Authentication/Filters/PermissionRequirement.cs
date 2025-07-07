using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Authentication.Filters;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permissions { get; }

    public PermissionRequirement(string permission)
    {
        Permissions = permission;
    }
}