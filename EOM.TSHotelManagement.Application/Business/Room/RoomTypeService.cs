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
using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Common.Util;
using EOM.TSHotelManagement.EntityFramework;
using jvncorelib.EntityLib;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 房间类型接口实现类
    /// </summary>
    public class RoomTypeService : IRoomTypeService
    {
        /// <summary>
        /// 客房类型
        /// </summary>
        private readonly GenericRepository<RoomType> roomTypeRepository;

        /// <summary>
        /// 客房信息
        /// </summary>
        private readonly GenericRepository<Room> roomRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomTypeRepository"></param>
        /// <param name="roomRepository"></param>
        public RoomTypeService(GenericRepository<RoomType> roomTypeRepository, GenericRepository<Room> roomRepository)
        {
            this.roomTypeRepository = roomTypeRepository;
            this.roomRepository = roomRepository;
        }

        #region 获取所有房间类型
        /// <summary>
        /// 获取所有房间类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomTypeOutputDto> SelectRoomTypesAll(ReadRoomTypeInputDto readRoomTypeInputDto)
        {
            var where = Expressionable.Create<RoomType>();

            where = where.And(a => a.IsDelete == 0);

            List<RoomType> types = new List<RoomType>();

            if (!readRoomTypeInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readRoomTypeInputDto.IsDelete);

            }

            var count = 0;

            if (!readRoomTypeInputDto.IgnorePaging && readRoomTypeInputDto.Page != 0 && readRoomTypeInputDto.PageSize != 0)
            {
                types = roomTypeRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readRoomTypeInputDto.Page, readRoomTypeInputDto.PageSize, ref count);
            }
            else
            {
                types = roomTypeRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            types.ForEach(t =>
            {
                t.DeleteMarkDescription = t.IsDelete == 0 ? "否" : "是";
            });

            var listSource = EntityMapper.MapList<RoomType, ReadRoomTypeOutputDto>(types);

            return new ListOutputDto<ReadRoomTypeOutputDto>
            {
                listSource = listSource,
                total = listSource.Count
            };
        }
        #endregion

        #region 根据房间编号查询房间类型名称
        /// <summary>
        /// 根据房间编号查询房间类型名称
        /// </summary>
        /// <param name="readRoomTypeInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomTypeOutputDto> SelectRoomTypeByRoomNo(ReadRoomTypeInputDto readRoomTypeInputDto)
        {
            RoomType roomtype = new RoomType();
            Room room = new Room();
            room = roomRepository.GetSingle(a => a.RoomNumber == readRoomTypeInputDto.RoomNumber && a.IsDelete != 1);
            roomtype.RoomTypeName = roomTypeRepository.GetSingle(a => a.RoomTypeId == room.RoomTypeId).RoomTypeName;

            var source = EntityMapper.Map<RoomType, ReadRoomTypeOutputDto>(roomtype);

            return new SingleOutputDto<ReadRoomTypeOutputDto> { Source = source };
        }
        #endregion

        /// <summary>
        /// 根据房间类型查询类型配置
        /// </summary>
        /// <param name="readRoomTypeInputDto"></param>
        /// <returns></returns>
        public ReadRoomTypeOutputDto SelectRoomTypeByType(ReadRoomTypeInputDto readRoomTypeInputDto)
        {
            var roomType = roomTypeRepository.GetSingle(a => a.RoomTypeId == readRoomTypeInputDto.RoomTypeId);

            var source = EntityMapper.Map<RoomType, ReadRoomTypeOutputDto>(roomType);

            return source;
        }

        /// <summary>
        /// 添加房间类型
        /// </summary>
        /// <param name="roomType"></param>
        /// <returns></returns>
        public BaseOutputDto InsertRoomType(CreateRoomTypeInputDto roomType)
        {
            try
            {
                var existRoomType = roomTypeRepository.IsAny(a => a.RoomTypeId == roomType.RoomTypeId);
                if (existRoomType)
                    return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString("This room type already exists.", "房间类型已存在。"), StatusCode = StatusCodeConstants.InternalServerError };
                roomTypeRepository.Insert(EntityMapper.Map<CreateRoomTypeInputDto, RoomType>(roomType));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新房间类型
        /// </summary>
        /// <param name="roomType"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateRoomType(UpdateRoomTypeInputDto roomType)
        {
            try
            {
                roomTypeRepository.Update(a => new RoomType
                {
                    RoomTypeName = roomType.RoomTypeName,
                    RoomRent = roomType.RoomRent,
                    RoomDeposit = roomType.RoomDeposit,
                    IsDelete = roomType.IsDelete,
                    DataChgUsr = roomType.DataChgUsr,
                    DataChgDate = roomType.DataChgDate
                }, a => a.RoomTypeId == roomType.RoomTypeId);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除房间类型
        /// </summary>
        /// <param name="roomType"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteRoomType(DeleteRoomTypeInputDto roomType)
        {
            try
            {
                roomTypeRepository.Update(a => new RoomType
                {
                    IsDelete = 1
                }, a => a.RoomTypeId == roomType.RoomTypeId);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
    }
}
