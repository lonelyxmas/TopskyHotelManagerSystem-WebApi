using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 消费信息控制器
    /// </summary>
    public class SpendController : ControllerBase
    {
        private readonly ISpendService spendService;

        public SpendController(ISpendService spendService)
        {
            this.spendService = spendService;
        }

        /// <summary>
        /// 添加消费信息
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto InsertSpendInfo([FromBody] CreateSpendInputDto s)
        {
            return spendService.InsertSpendInfo(s);
        }

        /// <summary>
        /// 根据客户编号查询消费信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadSpendOutputDto> SelectSpendByCustoNo([FromQuery] ReadSpendInputDto inputDto)
        {
            return spendService.SelectSpendByCustoNo(inputDto);
        }

        /// <summary>
        /// 根据房间编号查询消费信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadSpendOutputDto> SelectSpendByRoomNo([FromQuery] ReadSpendInputDto inputDto)
        {
            return spendService.SelectSpendByRoomNo(inputDto);
        }

        /// <summary>
        /// 根据客户编号查询历史消费信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadSpendOutputDto> SeletHistorySpendInfoAll([FromQuery] ReadSpendInputDto inputDto)
        {
            return spendService.SeletHistorySpendInfoAll(inputDto);
        }

        /// <summary>
        /// 查询消费的所有信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadSpendOutputDto> SelectSpendInfoAll([FromQuery] ReadSpendInputDto readSpendInputDto)
        {
            return spendService.SelectSpendInfoAll(readSpendInputDto);
        }

        /// <summary>
        /// 根据房间号查询消费的所有信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadSpendOutputDto> SelectSpendInfoRoomNo([FromQuery] ReadSpendInputDto inputDto)
        {
            return spendService.SelectSpendInfoRoomNo(inputDto);
        }

        /// <summary>
        /// 根据房间编号、入住时间到当前时间查询消费总金额
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadSpendInputDto> SumConsumptionAmount([FromQuery] ReadSpendInputDto inputDto)
        {
            return spendService.SumConsumptionAmount(inputDto);
        }

        /// <summary>
        /// 撤回客户消费信息
        /// </summary>
        /// <param name="updateSpendInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UndoCustomerSpend([FromBody] UpdateSpendInputDto updateSpendInputDto)
        {
            return spendService.UndoCustomerSpend(updateSpendInputDto);
        }

        /// <summary>
        /// 更新消费信息
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdSpenInfo([FromBody] UpdateSpendInputDto inputDto)
        {
            return spendService.UpdSpenInfo(inputDto);
        }
    }
}
