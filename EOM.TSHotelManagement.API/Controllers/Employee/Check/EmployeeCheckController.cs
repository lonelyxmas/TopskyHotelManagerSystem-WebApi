using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 员工打卡控制器
    /// </summary>
    public class EmployeeCheckController : ControllerBase
    {
        private readonly IEmployeeCheckService workerCheckService;

        public EmployeeCheckController(IEmployeeCheckService workerCheckService)
        {
            this.workerCheckService = workerCheckService;
        }

        /// <summary>
        /// 根据员工编号查询其所有的打卡记录
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.view")]
        [HttpGet]
        public ListOutputDto<ReadEmployeeCheckOutputDto> SelectCheckInfoByEmployeeId([FromQuery] ReadEmployeeCheckInputDto inputDto)
        {
            return workerCheckService.SelectCheckInfoByEmployeeId(inputDto);
        }

        /// <summary>
        /// 查询员工签到天数
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.view")]
        [HttpGet]
        public SingleOutputDto<ReadEmployeeCheckOutputDto> SelectWorkerCheckDaySumByEmployeeId([FromQuery] ReadEmployeeCheckInputDto inputDto)
        {
            return workerCheckService.SelectWorkerCheckDaySumByEmployeeId(inputDto);
        }

        /// <summary>
        /// 查询今天员工是否已签到
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.view")]
        [HttpGet]
        public SingleOutputDto<ReadEmployeeCheckOutputDto> SelectToDayCheckInfoByWorkerNo([FromQuery] ReadEmployeeCheckInputDto inputDto)
        {
            return workerCheckService.SelectToDayCheckInfoByWorkerNo(inputDto);
        }

        /// <summary>
        /// 添加员工打卡数据
        /// </summary>
        /// <param name="workerCheck"></param>
        /// <returns></returns>
        [RequirePermission("staffmanagement.create")]
        [HttpPost]
        public BaseResponse AddCheckInfo([FromBody] CreateEmployeeCheckInputDto workerCheck)
        {
            return workerCheckService.AddCheckInfo(workerCheck);
        }
    }
}



