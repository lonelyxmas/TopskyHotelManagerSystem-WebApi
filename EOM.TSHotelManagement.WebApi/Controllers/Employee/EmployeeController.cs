using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 员工信息控制器
    /// </summary>
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService workerService;

        public EmployeeController(IEmployeeService workerService)
        {
            this.workerService = workerService;
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateEmployee([FromBody] UpdateEmployeeInputDto worker)
        {
            return workerService.UpdateEmployee(worker);
        }

        /// <summary>
        /// 员工账号禁/启用
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto ManagerEmployeeAccount([FromBody] UpdateEmployeeInputDto worker)
        {
            return workerService.ManagerEmployeeAccount(worker);
        }

        /// <summary>
        /// 更新员工职位和部门
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateEmployeePositionAndClub([FromBody] UpdateEmployeeInputDto worker)
        {
            return workerService.UpdateEmployeePositionAndClub(worker);
        }

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto AddEmployee([FromBody] CreateEmployeeInputDto worker)
        {
            return workerService.AddEmployee(worker);
        }

        /// <summary>
        /// 获取所有工作人员信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadEmployeeOutputDto> SelectEmployeeAll([FromQuery] ReadEmployeeInputDto inputDto)
        {
            return workerService.SelectEmployeeAll(inputDto);
        }

        /// <summary>
        /// 检查指定部门下是否存在工作人员
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public BaseOutputDto CheckEmployeeByDepartment([FromQuery] ReadEmployeeInputDto inputDto)
        {
            return workerService.CheckEmployeeByDepartment(inputDto);
        }

        /// <summary>
        /// 根据登录名称查询员工信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadEmployeeOutputDto> SelectEmployeeInfoByEmployeeId([FromQuery] ReadEmployeeInputDto inputDto)
        {
            return workerService.SelectEmployeeInfoByEmployeeId(inputDto);
        }

        /// <summary>
        /// 根据登录名称、密码查询员工信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public SingleOutputDto<ReadEmployeeOutputDto> SelectEmployeeInfoByEmployeeIdAndEmployeePwd([FromBody] ReadEmployeeInputDto inputDto)
        {
            return workerService.SelectEmployeeInfoByEmployeeIdAndEmployeePwd(inputDto);
        }

        /// <summary>
        /// 根据员工编号和密码修改密码
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdEmployeePwdByWorkNo([FromBody] UpdateEmployeeInputDto worker)
        {
            return workerService.UpdEmployeePwdByWorkNo(worker);
        }

        /// <summary>
        /// 重置员工账号密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto ResetEmployeeAccountPassword([FromBody] UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            return workerService.ResetEmployeeAccountPassword(updateEmployeeInputDto);
        }
    }
}