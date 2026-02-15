using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Contract.SystemManagement.Dto.Permission;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 管理员控制器
    /// </summary>
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// 管理员模块
        /// </summary>
        private readonly IAdminService adminService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminService"></param>
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        /// <summary>
        /// 后台系统登录
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public SingleOutputDto<ReadAdministratorOutputDto> Login([FromBody] ReadAdministratorInputDto admin)
        {
            return adminService.Login(admin);
        }

        /// <summary>
        /// 获取所有管理员列表
        /// </summary>
        /// <returns></returns>
        [RequirePermission("system:admin:list")]
        [HttpGet]
        public ListOutputDto<ReadAdministratorOutputDto> GetAllAdminList(ReadAdministratorInputDto readAdministratorInputDto)
        {
            return adminService.GetAllAdminList(readAdministratorInputDto);
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [RequirePermission("system:admin:create")]
        [HttpPost]
        public BaseResponse AddAdmin([FromBody] CreateAdministratorInputDto admin)
        {
            return adminService.AddAdmin(admin);
        }

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:admin:update")]
        [HttpPost]
        public BaseResponse UpdAdmin([FromBody] UpdateAdministratorInputDto updateAdministratorInputDto)
        {
            return adminService.UpdAdmin(updateAdministratorInputDto);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="deleteAdministratorInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:admin:delete")]
        [HttpPost]
        public BaseResponse DelAdmin([FromBody] DeleteAdministratorInputDto deleteAdministratorInputDto)
        {
            return adminService.DelAdmin(deleteAdministratorInputDto);
        }

        /// <summary>
        /// 获取所有管理员类型
        /// </summary>
        /// <returns></returns>
        [RequirePermission("system:admintype:list")]
        [HttpGet]
        public ListOutputDto<ReadAdministratorTypeOutputDto> GetAllAdminTypes(ReadAdministratorTypeInputDto readAdministratorTypeInputDto)
        {
            return adminService.GetAllAdminTypes(readAdministratorTypeInputDto);
        }

        /// <summary>
        /// 添加管理员类型
        /// </summary>
        /// <param name="createAdministratorTypeInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:admintype:create")]
        [HttpPost]
        public BaseResponse AddAdminType([FromBody] CreateAdministratorTypeInputDto createAdministratorTypeInputDto)
        {
            return adminService.AddAdminType(createAdministratorTypeInputDto);
        }

        /// <summary>
        /// 更新管理员类型
        /// </summary>
        /// <param name="updateAdministratorTypeInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:admintype:update")]
        [HttpPost]
        public BaseResponse UpdAdminType([FromBody] UpdateAdministratorTypeInputDto updateAdministratorTypeInputDto)
        {
            return adminService.UpdAdminType(updateAdministratorTypeInputDto);
        }

        /// <summary>
        /// 删除管理员类型
        /// </summary>
        /// <param name="deleteAdministratorTypeInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:admintype:delete")]
        [HttpPost]
        public BaseResponse DelAdminType([FromBody] DeleteAdministratorTypeInputDto deleteAdministratorTypeInputDto)
        {
            return adminService.DelAdminType(deleteAdministratorTypeInputDto);
        }

        /// <summary>
        /// 为用户分配角色（全量覆盖）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequirePermission("system:user:assign")]
        [HttpPost]
        public BaseResponse AssignUserRoles([FromBody] AssignUserRolesInputDto input)
        {
            return adminService.AssignUserRoles(input);
        }

        /// <summary>
        /// 读取指定用户已分配的角色编码集合
        /// </summary>
        /// <param name="userNumber">用户编码</param>
        /// <returns>角色编码集合（RoleNumber 列表）</returns>
        [RequirePermission("system:user:assign.view")]
        [HttpGet]
        public ListOutputDto<string> ReadUserRoles([FromQuery] string userNumber)
        {
            return adminService.ReadUserRoles(userNumber);
        }

        /// <summary>
        /// 读取指定用户的“角色-权限”明细（来自 RolePermission 关联，并联到 Permission 得到权限码与名称）
        /// </summary>
        /// <param name="userNumber">用户编码</param>
        /// <returns>明细列表（包含 RoleNumber、PermissionNumber、PermissionName、MenuKey）</returns>
        [RequirePermission("system:user:assign.view")]
        [HttpGet]
        public ListOutputDto<UserRolePermissionOutputDto> ReadUserRolePermissions([FromQuery] string userNumber)
        {
            return adminService.ReadUserRolePermissions(userNumber);
        }

        /// <summary>
        /// 为指定用户分配“直接权限”（通过专属角色 R-USER-{UserNumber} 写入 RolePermission，全量覆盖）
        /// </summary>
        [RequirePermission("system:user:assign")]
        [HttpPost]
        public BaseResponse AssignUserPermissions([FromBody] AssignUserPermissionsInputDto input)
        {
            return adminService.AssignUserPermissions(input);
        }

        /// <summary>
        /// 读取指定用户的“直接权限”（仅来自专属角色 R-USER-{UserNumber} 的权限编码列表）
        /// </summary>
        [RequirePermission("system:user:assign.view")]
        [HttpGet]
        public ListOutputDto<string> ReadUserDirectPermissions([FromQuery] string userNumber)
        {
            return adminService.ReadUserDirectPermissions(userNumber);
        }
    }
}
