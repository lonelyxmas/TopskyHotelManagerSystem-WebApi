using EOM.TSHotelManagement.Common.Contract;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 角色服务接口
    /// </summary>
    public interface IRoleAppService
    {
        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="readRoleInputDto"></param>
        /// <returns></returns>
        ListOutputDto<ReadRoleOutputDto> SelectRoleList(ReadRoleInputDto readRoleInputDto);
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="createRoleInputDto"></param>
        /// <returns></returns>
        BaseOutputDto InsertRole(CreateRoleInputDto createRoleInputDto);
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="updateRoleInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdateRole(UpdateRoleInputDto updateRoleInputDto);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteRoleInputDto"></param>
        /// <returns></returns>
        BaseOutputDto DeleteRole(DeleteRoleInputDto deleteRoleInputDto);
    }
}
