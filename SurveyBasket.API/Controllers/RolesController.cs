using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Abstractions.ResultPattern;
using SurveyBasket.API.DtoRequestAndResponse.Roles;
using SurveyBasket.API.Repositories;
using SurveyBasket.Authentication.Filters;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAllRoles")]
        [HasPermission(Permissions.getRoles)]
        public async Task<IActionResult> GetAllRoles([FromQuery] bool? includeDisable = false, CancellationToken cancellationToken = default)
        {
            var Roles = await _roleService.GetAllRolesAsync(includeDisable, cancellationToken);
            return Ok(Roles.Value);
        }

        [HttpGet("GetRolesDetails/{Id}")]
        [HasPermission(Permissions.getRoles)]
        public async Task<IActionResult> GetRolesDetails([FromRoute] string Id, CancellationToken cancellationToken = default)
        {
            var Roles = await _roleService.GetRolesDetailsAsync(Id, cancellationToken);
            return Roles.IsSuccess ? Ok(Roles.Value) : Roles.ToProblem();
        }

        [HttpPost("AddNewRole")]
        [HasPermission(Permissions.addRoles)]
        public async Task<IActionResult> AddNewRole([FromBody] RoleRequest roleRequest, CancellationToken cancellationToken = default)
        {
            var AddRoles = await _roleService.AddRoleAsync(roleRequest, cancellationToken);
            return AddRoles.IsSuccess ? Ok(AddRoles.Value) : AddRoles.ToProblem();
        }


        [HttpPost("UpdateRole/{Id}")]
        [HasPermission(Permissions.updateRoles)]
        public async Task<IActionResult> UpdateRole([FromRoute] string Id , RoleRequest roleRequest, CancellationToken cancellationToken = default)
        {
            var UpdateRoles = await _roleService.UpdateRoleAsync(Id, roleRequest, cancellationToken);
            return UpdateRoles.IsSuccess ? NoContent() : UpdateRoles.ToProblem();
        }


        [HttpPut("AddToggleStatusForRole/{Id}")]
        [HasPermission(Permissions.UpdateUsers)]
        public async Task<IActionResult> AddToggleStatusForRole([FromRoute] string Id , CancellationToken cancellationToken = default)
        {
            var AddToggleStatus = await _roleService.AddToggleStatusForRoleAsync(Id, cancellationToken);
            return AddToggleStatus.IsSuccess ? NoContent() : AddToggleStatus.ToProblem();
        }



         









    }
}
