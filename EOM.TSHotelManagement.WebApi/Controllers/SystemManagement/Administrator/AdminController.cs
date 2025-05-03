using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
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
        /// 根据超管密码查询员工类型和权限
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public SingleOutputDto<ReadAdministratorOutputDto> SelectManagerByPass([FromBody] ReadAdministratorInputDto admin)
        {
            return adminService.SelectManagerByPass(admin);
        }

        /// <summary>
        /// 后台系统登录
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public SingleOutputDto<ReadAdministratorOutputDto> Login([FromBody] ReadAdministratorInputDto admin)
        {
            return adminService.Login(admin);
        }

        /// <summary>
        /// 根据超管账号查询对应的密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadAdministratorOutputDto> SelectAdminPwdByAccount([FromQuery] ReadAdministratorInputDto account)
        {
            return adminService.SelectAdminPwdByAccount(account);
        }

        /// <summary>
        /// 获取所有管理员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadAdministratorOutputDto> GetAllAdminList(ReadAdministratorInputDto readAdministratorInputDto)
        {
            return adminService.GetAllAdminList(readAdministratorInputDto);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateNewPwdByOldPwd([FromBody] UpdateAdministratorInputDto admin)
        {
            return adminService.UpdateNewPwdByOldPwd(admin);
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto AddAdmin([FromBody] CreateAdministratorInputDto admin)
        {
            return adminService.AddAdmin(admin);
        }

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdAdmin([FromBody] UpdateAdministratorInputDto updateAdministratorInputDto)
        {
            return adminService.UpdAdmin(updateAdministratorInputDto);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="deleteAdministratorInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DelAdmin([FromBody] DeleteAdministratorInputDto deleteAdministratorInputDto)
        {
            return adminService.DelAdmin(deleteAdministratorInputDto);
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadAdministratorOutputDto> GetAdminInfoByAdminAccount([FromQuery] ReadAdministratorInputDto admin)
        {
            return adminService.GetAdminInfoByAdminAccount(admin);
        }

        /// <summary>
        /// 获取所有管理员类型
        /// </summary>
        /// <returns></returns>
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
        [HttpPost]
        public BaseOutputDto AddAdminType([FromBody] CreateAdministratorTypeInputDto createAdministratorTypeInputDto)
        {
            return adminService.AddAdminType(createAdministratorTypeInputDto);
        }

        /// <summary>
        /// 更新管理员类型
        /// </summary>
        /// <param name="updateAdministratorTypeInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdAdminType([FromBody] UpdateAdministratorTypeInputDto updateAdministratorTypeInputDto)
        {
            return adminService.UpdAdminType(updateAdministratorTypeInputDto);
        }

        /// <summary>
        /// 删除管理员类型
        /// </summary>
        /// <param name="deleteAdministratorTypeInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DelAdminType([FromBody] DeleteAdministratorTypeInputDto deleteAdministratorTypeInputDto)
        {
            return adminService.DelAdminType(deleteAdministratorTypeInputDto);
        }

        /// <summary>
        /// 批量更新管理员账户
        /// </summary>
        /// <param name="admins"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdAccount([FromBody] UpdateAdministratorInputDto admins)
        {
            return adminService.UpdAccount(admins);
        }

    }
}
