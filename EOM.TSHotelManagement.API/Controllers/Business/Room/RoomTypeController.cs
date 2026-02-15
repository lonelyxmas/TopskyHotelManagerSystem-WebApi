using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
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
        [RequirePermission("roomconfig.view")]
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
        [RequirePermission("roomconfig.view")]
        [HttpGet]
        public SingleOutputDto<ReadRoomTypeOutputDto> SelectRoomTypeByRoomNo([FromQuery] ReadRoomTypeInputDto inputDto)
        {
            return roomTypeService.SelectRoomTypeByRoomNo(inputDto);
        }

        /// <summary>
        /// 添加房间状态
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("roomconfig.create")]
        [HttpPost]
        public BaseResponse InsertRoomType([FromBody] CreateRoomTypeInputDto inputDto)
        {
            return roomTypeService.InsertRoomType(inputDto);
        }

        /// <summary>
        /// 更新房间状态
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("roomconfig.update")]
        [HttpPost]
        public BaseResponse UpdateRoomType([FromBody] UpdateRoomTypeInputDto inputDto)
        {
            return roomTypeService.UpdateRoomType(inputDto);
        }

        /// <summary>
        /// 删除房间状态
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [RequirePermission("roomconfig.delete")]
        [HttpPost]
        public BaseResponse DeleteRoomType([FromBody] DeleteRoomTypeInputDto inputDto)
        {
            return roomTypeService.DeleteRoomType(inputDto);
        }
    }
}