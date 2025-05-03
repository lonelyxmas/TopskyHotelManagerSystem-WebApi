using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 员工履历控制器
    /// </summary>
    public class EmployeeHistoryController : ControllerBase
    {
        private readonly IEmployeeHistoryService workerHistoryService;

        public EmployeeHistoryController(IEmployeeHistoryService workerHistoryService)
        {
            this.workerHistoryService = workerHistoryService;
        }

        /// <summary>
        /// 根据工号添加员工履历
        /// </summary>
        /// <param name="workerHistory"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto AddHistoryByEmployeeId([FromBody] CreateEmployeeHistoryInputDto workerHistory)
        {
            return workerHistoryService.AddHistoryByEmployeeId(workerHistory);
        }

        /// <summary>
        /// 根据工号查询履历信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadEmployeeHistoryOutputDto> SelectHistoryByEmployeeId([FromQuery] ReadEmployeeHistoryInputDto inputDto)
        {
            return workerHistoryService.SelectHistoryByEmployeeId(inputDto);
        }
    }
}


