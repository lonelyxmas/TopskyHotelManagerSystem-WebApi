using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Contract.SystemManagement.Dto.Permission;
using EOM.TSHotelManagement.Contract.SystemManagement.Dto.Role;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    public class RoleController : ControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="readRoleInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:role:list")]
        [HttpGet]
        public ListOutputDto<ReadRoleOutputDto> SelectRoleList([FromQuery] ReadRoleInputDto readRoleInputDto)
        {
            return _roleAppService.SelectRoleList(readRoleInputDto);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="createRoleInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:role:create")]
        [HttpPost]
        public BaseResponse InsertRole([FromBody] CreateRoleInputDto createRoleInputDto)
        {
            return _roleAppService.InsertRole(createRoleInputDto);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="updateRoleInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:role:update")]
        [HttpPost]
        public BaseResponse UpdateRole([FromBody] UpdateRoleInputDto updateRoleInputDto)
        {
            return _roleAppService.UpdateRole(updateRoleInputDto);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteRoleInputDto"></param>
        /// <returns></returns>
        [RequirePermission("system:role:delete")]
        [HttpPost]
        public BaseResponse DeleteRole([FromBody] DeleteRoleInputDto deleteRoleInputDto)
        {
            return _roleAppService.DeleteRole(deleteRoleInputDto);
        }

        /// <summary>
        /// 为角色授予权限（全量覆盖）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequirePermission("system:role:grant")]
        [HttpPost]
        public BaseResponse GrantRolePermissions([FromBody] GrantRolePermissionsInputDto input)
        {
            return _roleAppService.GrantRolePermissions(input);
        }

        /// <summary>
        /// 读取指定角色已授予的权限编码集合
        /// </summary>
        /// <param name="roleNumber">角色编码</param>
        [RequirePermission("system:role:list")]
        [HttpGet]
        public ListOutputDto<string> ReadRolePermissions([FromQuery] string roleNumber)
        {
            return _roleAppService.ReadRolePermissions(roleNumber);
        }

        /// <summary>
        /// 读取隶属于指定角色的管理员用户编码集合
        /// </summary>
        /// <param name="roleNumber">角色编码</param>
        [RequirePermission("system:role:list")]
        [HttpGet]
        public ListOutputDto<string> ReadRoleUsers([FromQuery] string roleNumber)
        {
            return _roleAppService.ReadRoleUsers(roleNumber);
        }

        /// <summary>
        /// 为角色分配管理员（全量覆盖）
        /// </summary>
        /// <param name="input">包含角色编码与管理员编码集合</param>
        [RequirePermission("system:role:grant")]
        [HttpPost]
        public BaseResponse AssignRoleUsers([FromBody] AssignRoleUsersInputDto input)
        {
            return _roleAppService.AssignRoleUsers(input);
        }
    }
}
