using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.DtoRequestAndResponse.Roles;

namespace SurveyBasket.API.Repositories
{
    public interface IRoleService
    {
        Task<Result<IEnumerable<RoleResponse>>> GetAllRolesAsync(bool? IncludeDisable = false, CancellationToken cancellationToken = default);
        Task<Result<RoleDetailsResponse>> GetRolesDetailsAsync(string Id, CancellationToken cancellationToken = default);
        Task<Result<RoleDetailsResponse>> AddRoleAsync(RoleRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateRoleAsync(string Id , RoleRequest request, CancellationToken cancellationToken = default);
        Task<Result> AddToggleStatusForRoleAsync(string Id , CancellationToken cancellationToken = default);


    }
}
