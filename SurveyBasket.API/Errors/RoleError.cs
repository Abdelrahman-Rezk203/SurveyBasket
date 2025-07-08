using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
    public class RoleError 
    {
   
        public static readonly Error RoleNotFound =
            new("Role.NotFound", "Role Is Not Found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidPermissions =
            new (Code: "Role. InvalidPermissions", Description: "Invalid Permissions", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicatedRole =
            new(Code: "Role. DuplicatedRole", Description: "Another role with the same name is already exists", StatusCodes.Status409Conflict);

        public static readonly Error AddRoleFailure =
            new(Code: "Role. AddRoleFailure", Description: "Can't Add New Role", StatusCodes.Status400BadRequest);

        public static readonly Error UpdateRoleFailure =
            new(Code: "Role. UpdateRoleFailure", Description: "Can't Update New Role", StatusCodes.Status400BadRequest);


    }
}
