using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 监管统计控制器
    /// </summary>
    public class SupervisionStatisticsController : ControllerBase
    {
        private readonly ISupervisionStatisticsService checkInfoService;

        public SupervisionStatisticsController(ISupervisionStatisticsService checkInfoService)
        {
            this.checkInfoService = checkInfoService;
        }

        /// <summary>
        /// 查询所有监管统计信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("supervisioninfo.view")]
        [HttpGet]
        public ListOutputDto<ReadSupervisionStatisticsOutputDto> SelectSupervisionStatisticsAll([FromQuery] ReadSupervisionStatisticsInputDto inputDto)
        {
            return checkInfoService.SelectSupervisionStatisticsAll(inputDto);
        }

        /// <summary>
        /// 插入监管统计信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("supervisioninfo.create")]
        [HttpPost]
        public BaseResponse InsertSupervisionStatistics([FromBody] CreateSupervisionStatisticsInputDto inputDto)
        {
            return checkInfoService.InsertSupervisionStatistics(inputDto);
        }

        /// <summary>
        /// 更新监管统计信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("supervisioninfo.update")]
        [HttpPost]
        public BaseResponse UpdateSupervisionStatistics([FromBody] UpdateSupervisionStatisticsInputDto inputDto)
        {
            return checkInfoService.UpdateSupervisionStatistics(inputDto);
        }

        /// <summary>
        /// 删除监管统计信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("supervisioninfo.delete")]
        [HttpPost]
        public BaseResponse DeleteSupervisionStatistics([FromBody] DeleteSupervisionStatisticsInputDto inputDto)
        {
            return checkInfoService.DeleteSupervisionStatistics(inputDto);
        }
    }
}