/*
 * MIT License
 *Copyright (c) 2021 易开元(Easy-Open-Meta)

 *Permission is hereby granted, free of charge, to any person obtaining a copy
 *of this software and associated documentation files (the "Software"), to deal
 *in the Software without restriction, including without limitation the rights
 *to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *copies of the Software, and to permit persons to whom the Software is
 *furnished to do so, subject to the following conditions:

 *The above copyright notice and this permission notice shall be included in all
 *copies or substantial portions of the Software.

 *THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *SOFTWARE.
 *
 */
using EOM.TSHotelManagement.Common.Contract;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 管理员数据访问接口
    /// </summary>
    public interface IAdminService
    {

        /// <summary>
        /// 根据超管密码查询员工类型和权限
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadAdministratorOutputDto> SelectManagerByPass(ReadAdministratorInputDto readAdministratorInputDto);

        /// <summary>
        /// 后台系统登录
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadAdministratorOutputDto> Login(ReadAdministratorInputDto readAdministratorInputDto);

        /// <summary>
        /// 根据超管账号查询对应的密码
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadAdministratorOutputDto> SelectAdminPwdByAccount(ReadAdministratorInputDto readAdministratorInputDto);

        /// <summary>
        /// 获取所有管理员列表
        /// </summary>
        /// <returns></returns>
        ListOutputDto<ReadAdministratorOutputDto> GetAllAdminList(ReadAdministratorInputDto readAdministratorInputDto);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdateNewPwdByOldPwd(UpdateAdministratorInputDto updateAdministratorInputDto);

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="createAdministratorInputDto"></param>
        /// <returns></returns>
        BaseOutputDto AddAdmin(CreateAdministratorInputDto createAdministratorInputDto);

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdAdmin(UpdateAdministratorInputDto updateAdministratorInputDto);

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="deleteAdministratorInputDto"></param>
        /// <returns></returns>
        BaseOutputDto DelAdmin(DeleteAdministratorInputDto deleteAdministratorInputDto);

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadAdministratorOutputDto> GetAdminInfoByAdminAccount(ReadAdministratorInputDto readAdministratorInputDto);

        /// <summary>
        /// 获取所有管理员类型
        /// </summary>
        /// <returns></returns>
        ListOutputDto<ReadAdministratorTypeOutputDto> GetAllAdminTypes(ReadAdministratorTypeInputDto readAdministratorTypeInputDto);

        /// <summary>
        /// 添加管理员类型
        /// </summary>
        /// <param name="createAdministratorTypeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto AddAdminType(CreateAdministratorTypeInputDto createAdministratorTypeInputDto);

        /// <summary>
        /// 更新管理员类型
        /// </summary>
        /// <param name="updateAdministratorTypeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdAdminType(UpdateAdministratorTypeInputDto updateAdministratorTypeInputDto);

        /// <summary>
        /// 删除管理员类型
        /// </summary>
        /// <param name="deleteAdministratorTypeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto DelAdminType(DeleteAdministratorTypeInputDto deleteAdministratorTypeInputDto);

        /// <summary>
        /// 批量更新管理员账户
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdAccount(UpdateAdministratorInputDto updateAdministratorInputDto);
    }
}