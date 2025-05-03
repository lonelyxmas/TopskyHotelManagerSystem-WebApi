using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 房间类型控制器
    /// </summary>
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            this.roomTypeService = roomTypeService;
        }

        /// <summary>
        /// 获取所有房间类型
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadRoomTypeOutputDto> SelectRoomTypesAll([FromQuery] ReadRoomTypeInputDto inputDto)
        {
            return roomTypeService.SelectRoomTypesAll(inputDto);
        }

        /// <summary>
        /// 根据房间编号查询房间类型名称
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadRoomTypeOutputDto> SelectRoomTypeByRoomNo([FromQuery] ReadRoomTypeInputDto inputDto)
        {
            return roomTypeService.SelectRoomTypeByRoomNo(inputDto);
        }

        /// <summary>
        /// 根据房间类型查询类型配置
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ReadRoomTypeOutputDto SelectRoomTypeByType([FromQuery] ReadRoomTypeInputDto inputDto)
        {
            return roomTypeService.SelectRoomTypeByType(inputDto);
        }

        /// <summary>
        /// 添加房间状态
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto InsertRoomType([FromBody] CreateRoomTypeInputDto inputDto)
        {
            return roomTypeService.InsertRoomType(inputDto);
        }

        /// <summary>
        /// 更新房间状态
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateRoomType([FromBody] UpdateRoomTypeInputDto inputDto)
        {
            return roomTypeService.UpdateRoomType(inputDto);
        }

        /// <summary>
        /// 删除房间状态
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteRoomType([FromBody] DeleteRoomTypeInputDto inputDto)
        {
            return roomTypeService.DeleteRoomType(inputDto);
        }
    }
}