using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
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
        [HttpPost]
        public BaseOutputDto InsertRole([FromBody] CreateRoleInputDto createRoleInputDto)
        {
            return _roleAppService.InsertRole(createRoleInputDto);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="updateRoleInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateRole([FromBody] UpdateRoleInputDto updateRoleInputDto)
        {
            return _roleAppService.UpdateRole(updateRoleInputDto);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteRoleInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteRole([FromBody] DeleteRoleInputDto deleteRoleInputDto)
        {
            return _roleAppService.DeleteRole(deleteRoleInputDto);
        }
    }
}
