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
    /// 员工信息接口
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdateEmployee(UpdateEmployeeInputDto updateEmployeeInputDto);

        /// <summary>
        /// 员工账号禁/启用
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto ManagerEmployeeAccount(UpdateEmployeeInputDto updateEmployeeInputDto);

        /// <summary>
        /// 更新员工职位和部门
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>

        BaseOutputDto UpdateEmployeePositionAndClub(UpdateEmployeeInputDto updateEmployeeInputDto);

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="createEmployeeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto AddEmployee(CreateEmployeeInputDto createEmployeeInputDto);

        /// <summary>
        /// 获取所有工作人员信息
        /// </summary>
        /// <returns></returns>
        ListOutputDto<ReadEmployeeOutputDto> SelectEmployeeAll(ReadEmployeeInputDto readEmployeeInputDto);

        /// <summary>
        /// 检查指定部门下是否存在工作人员
        /// </summary>
        /// <param name="readEmployeeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto CheckEmployeeByDepartment(ReadEmployeeInputDto readEmployeeInputDto);

        /// <summary>
        /// 根据登录名称查询员工信息
        /// </summary>
        /// <param name="readEmployeeInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadEmployeeOutputDto> SelectEmployeeInfoByEmployeeId(ReadEmployeeInputDto readEmployeeInputDto);

        /// <summary>
        /// 根据登录名称、密码查询员工信息
        /// </summary>
        /// <param name="readEmployeeInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadEmployeeOutputDto> SelectEmployeeInfoByEmployeeIdAndEmployeePwd(ReadEmployeeInputDto readEmployeeInputDto);

        /// <summary>
        /// 根据员工编号和密码修改密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto UpdEmployeePwdByWorkNo(UpdateEmployeeInputDto updateEmployeeInputDto);

        /// <summary>
        /// 重置员工账号密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        BaseOutputDto ResetEmployeeAccountPassword(UpdateEmployeeInputDto updateEmployeeInputDto);
    }
}