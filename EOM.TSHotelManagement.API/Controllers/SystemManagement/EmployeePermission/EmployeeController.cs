using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Contract.SystemManagement.Dto.Permission;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// filename OR language.declaration()
    /// EmployeeController.EmployeeController():1
    /// <summary>
    /// 员工组权限分配接口（与管理员一致的 5 个接口）
    /// 前端将调用：
    /// - POST /Employee/AssignUserRoles
    /// - GET  /Employee/ReadUserRoles?userNumber=...
    /// - GET  /Employee/ReadUserRolePermissions?userNumber=...
    /// - POST /Employee/AssignUserPermissions
    /// - GET  /Employee/ReadUserDirectPermissions?userNumber=...
    /// </summary>
    public class EmployeePermissionController : ControllerBase
    {
        private readonly IEmployeePermissionService employeePermService;

        public EmployeePermissionController(IEmployeePermissionService employeePermService)
        {
            this.employeePermService = employeePermService;
        }

        /// filename OR language.declaration()
        /// EmployeeController.AssignUserRoles():1
        /// <summary>
        /// 为员工分配角色（全量覆盖）
        /// </summary>
        [RequirePermission("system:user:assign")]
        [HttpPost]
        public BaseResponse AssignUserRoles([FromBody] AssignUserRolesInputDto input)
        {
            return employeePermService.AssignUserRoles(input);
        }

        /// filename OR language.declaration()
        /// EmployeeController.ReadUserRoles():1
        /// <summary>
        /// 读取员工已分配的角色编码集合
        /// </summary>
        [RequirePermission("system:user:assign.view")]
        [HttpGet]
        public ListOutputDto<string> ReadUserRoles([FromQuery] string userNumber)
        {
            return employeePermService.ReadUserRoles(userNumber);
        }

        /// filename OR language.declaration()
        /// EmployeeController.ReadUserRolePermissions():1
        /// <summary>
        /// 读取员工“角色-权限”明细
        /// </summary>
        [RequirePermission("system:user:assign.view")]
        [HttpGet]
        public ListOutputDto<UserRolePermissionOutputDto> ReadUserRolePermissions([FromQuery] string userNumber)
        {
            return employeePermService.ReadUserRolePermissions(userNumber);
        }

        /// filename OR language.declaration()
        /// EmployeeController.AssignUserPermissions():1
        /// <summary>
        /// 为员工分配“直接权限”（R-USER-{UserNumber} 全量覆盖）
        /// </summary>
        [RequirePermission("system:user:assign")]
        [HttpPost]
        public BaseResponse AssignUserPermissions([FromBody] AssignUserPermissionsInputDto input)
        {
            return employeePermService.AssignUserPermissions(input);
        }

        /// filename OR language.declaration()
        /// EmployeeController.ReadUserDirectPermissions():1
        /// <summary>
        /// 读取员工“直接权限”权限编码集合（来自 R-USER-{UserNumber}）
        /// </summary>
        [RequirePermission("system:user:assign.view")]
        [HttpGet]
        public ListOutputDto<string> ReadUserDirectPermissions([FromQuery] string userNumber)
        {
            return employeePermService.ReadUserDirectPermissions(userNumber);
        }
    }
}