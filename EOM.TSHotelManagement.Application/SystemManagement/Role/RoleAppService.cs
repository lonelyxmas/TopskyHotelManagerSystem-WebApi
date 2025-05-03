using EOM.TSHotelManagement.Common.Contract;
using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Common.Util;
using EOM.TSHotelManagement.EntityFramework;
using jvncorelib.EntityLib;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 角色服务接口
    /// </summary>
    public class RoleAppService : IRoleAppService
    {
        /// <summary>
        /// 角色仓储
        /// </summary>
        private readonly GenericRepository<Role> roleRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleRepository"></param>
        public RoleAppService(GenericRepository<Role> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteRoleInputDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BaseOutputDto DeleteRole(DeleteRoleInputDto deleteRoleInputDto)
        {
            try
            {
                roleRepository.Delete(EntityMapper.Map<DeleteRoleInputDto, Role>(deleteRoleInputDto));
            }
            catch (Exception)
            {
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Role Error", "删除角色失败"));
            }
            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Delete Role Success", "删除角色成功"));
        }

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="readRoleInputDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ListOutputDto<ReadRoleOutputDto> SelectRoleList(ReadRoleInputDto readRoleInputDto)
        {
            var where = Expressionable.Create<Role>();

            if (!readRoleInputDto.RoleNumber.IsNullOrEmpty())
            {
                where = where.And(x => x.RoleNumber.Contains(readRoleInputDto.RoleNumber));
            }
            if (!readRoleInputDto.RoleName.IsNullOrEmpty())
            {
                where = where.And(x => x.RoleName.Contains(readRoleInputDto.RoleName));
            }
            if (!readRoleInputDto.RoleDescription.IsNullOrEmpty())
            {
                where = where.And(x => x.RoleDescription.Contains(readRoleInputDto.RoleDescription));
            }
            if (!readRoleInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(x => x.IsDelete == readRoleInputDto.IsDelete);
            }

            var roles = new List<Role>();

            var count = 0;

            if (!readRoleInputDto.IgnorePaging && readRoleInputDto.Page != 0 && readRoleInputDto.PageSize != 0)
            {
                roles = roleRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readRoleInputDto.Page, readRoleInputDto.PageSize, ref count);
            }
            else
            {
                roles = roleRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var roleList = EntityMapper.MapList<Role, ReadRoleOutputDto>(roles);

            var listOutputDto = new ListOutputDto<ReadRoleOutputDto>
            {
                total = count,
                listSource = roleList
            };

            return listOutputDto;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="createRoleInputDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BaseOutputDto InsertRole(CreateRoleInputDto createRoleInputDto)
        {
            try
            {
                roleRepository.Insert(EntityMapper.Map<CreateRoleInputDto, Role>(createRoleInputDto));
            }
            catch (Exception)
            {
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Create Role Error", "创建角色失败"));
            }
            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Create Role Success", "创建角色成功"));
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="updateRoleInputDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BaseOutputDto UpdateRole(UpdateRoleInputDto updateRoleInputDto)
        {
            try
            {
                roleRepository.Update(EntityMapper.Map<UpdateRoleInputDto, Role>(updateRoleInputDto));
            }
            catch (Exception)
            {
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Role Error", "更新角色失败"));
            }
            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Update Role Success", "更新角色成功"));
        }
    }
}
