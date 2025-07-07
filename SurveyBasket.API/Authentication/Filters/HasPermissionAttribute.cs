using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Authentication.Filters;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(permission)
    {
    }
}