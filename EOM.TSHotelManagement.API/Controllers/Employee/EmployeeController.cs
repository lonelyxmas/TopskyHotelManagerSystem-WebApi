using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
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
        [RequirePermission("staffmanagement.update")]
        [HttpPost]
        public BaseResponse UpdateEmployee([FromBody] UpdateEmployeeInputDto worker)
        {
            return workerService.UpdateEmployee(worker);
        }

        /// <summary>
        /// 员工账号禁/启用
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.status")]
        [HttpPost]
        public BaseResponse ManagerEmployeeAccount([FromBody] UpdateEmployeeInputDto worker)
        {
            return workerService.ManagerEmployeeAccount(worker);
        }

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.create")]
        [HttpPost]
        public BaseResponse AddEmployee([FromBody] CreateEmployeeInputDto worker)
        {
            return workerService.AddEmployee(worker);
        }

        /// <summary>
        /// 获取所有工作人员信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.view")]
        [HttpGet]
        public ListOutputDto<ReadEmployeeOutputDto> SelectEmployeeAll([FromQuery] ReadEmployeeInputDto inputDto)
        {
            return workerService.SelectEmployeeAll(inputDto);
        }

        /// <summary>
        /// 根据登录名称查询员工信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.view")]
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
        [IgnoreAntiforgeryToken]
        public SingleOutputDto<ReadEmployeeOutputDto> EmployeeLogin([FromBody] EmployeeLoginDto inputDto)
        {
            return workerService.EmployeeLogin(inputDto);
        }

        /// <summary>
        /// 修改员工账号密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.reset")]
        [HttpPost]
        public BaseResponse UpdateEmployeeAccountPassword([FromBody] UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            return workerService.UpdateEmployeeAccountPassword(updateEmployeeInputDto);
        }
        /// <summary>
        /// 重置员工账号密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.update")]
        [HttpPost]
        public BaseResponse ResetEmployeeAccountPassword([FromBody] UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            return workerService.ResetEmployeeAccountPassword(updateEmployeeInputDto);
        }
    }
}