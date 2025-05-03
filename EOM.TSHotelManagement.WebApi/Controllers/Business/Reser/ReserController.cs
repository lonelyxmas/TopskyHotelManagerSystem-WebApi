using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 预约信息控制器
    /// </summary>
    public class ReserController : ControllerBase
    {
        /// <summary>
        /// 预约信息
        /// </summary>
        private readonly IReserService reserService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reserService"></param>
        public ReserController(IReserService reserService)
        {
            this.reserService = reserService;
        }

        /// <summary>
        /// 获取所有预约信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadReserOutputDto> SelectReserAll(ReadReserInputDto readReserInputDto)
        {
            return reserService.SelectReserAll(readReserInputDto);
        }

        /// <summary>
        /// 根据房间编号获取预约信息
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadReserOutputDto> SelectReserInfoByRoomNo([FromQuery] ReadReserInputDto readReserInputDto)
        {
            return reserService.SelectReserInfoByRoomNo(readReserInputDto);
        }

        /// <summary>
        /// 删除预约信息
        /// </summary>
        /// <param name="reser"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteReserInfo([FromBody] DeleteReserInputDto reser)
        {
            return reserService.DeleteReserInfo(reser);
        }

        /// <summary>
        /// 更新预约信息
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateReserInfo([FromBody] UpdateReserInputDto r)
        {
            return reserService.UpdateReserInfo(r);
        }

        /// <summary>
        /// 添加预约信息
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto InserReserInfo([FromBody] CreateReserInputDto r)
        {
            return reserService.InserReserInfo(r);
        }
    }
}
