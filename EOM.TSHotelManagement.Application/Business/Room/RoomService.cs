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
using EOM.TSHotelManagement.Shared;
using jvncorelib.CodeLib;
using jvncorelib.EntityLib;
using SqlSugar;
using System;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 客房信息接口实现类
    /// </summary>
    public class RoomService : IRoomService
    {
        /// <summary>
        /// 客房信息
        /// </summary>
        private readonly GenericRepository<Room> roomRepository;

        /// <summary>
        /// 消费记录
        /// </summary>
        private readonly GenericRepository<Spend> spendRepository;

        /// <summary>
        /// 客房类型
        /// </summary>
        private readonly GenericRepository<RoomType> roomTypeRepository;

        /// <summary>
        /// 能耗管理
        /// </summary>
        private readonly GenericRepository<EnergyManagement> energyRepository;

        /// <summary>
        /// 客户信息
        /// </summary>
        private readonly GenericRepository<Customer> custoRepository;

        /// <summary>
        /// 唯一编码
        /// </summary>
        private readonly UniqueCode uniqueCode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomRepository"></param>
        /// <param name="spendRepository"></param>
        /// <param name="roomTypeRepository"></param>
        /// <param name="energyRepository"></param>
        /// <param name="custoRepository"></param>
        /// <param name="uniqueCode"></param>
        public RoomService(GenericRepository<Room> roomRepository, GenericRepository<Spend> spendRepository, GenericRepository<RoomType> roomTypeRepository, GenericRepository<EnergyManagement> energyRepository, GenericRepository<Customer> custoRepository, UniqueCode uniqueCode)
        {
            this.roomRepository = roomRepository;
            this.spendRepository = spendRepository;
            this.roomTypeRepository = roomTypeRepository;
            this.energyRepository = energyRepository;
            this.custoRepository = custoRepository;
            this.uniqueCode = uniqueCode;
        }

        /// <summary>
        /// 根据房间状态获取相应状态的房间信息
        /// </summary>
        /// <param name="readRoomInputDto"></param>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectRoomByRoomState(ReadRoomInputDto readRoomInputDto)
        {
            List<EnumDto> roomStates = new List<EnumDto>();
            var helper = new EnumHelper();
            roomStates = Enum.GetValues(typeof(RoomState))
            .Cast<RoomState>()
            .Select(e => new EnumDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = helper.GetEnumDescription(e)
            })
            .ToList();
            List<RoomType> roomTypes = new List<RoomType>();
            roomTypes = roomTypeRepository.GetList(a => a.IsDelete != 1);
            List<Room> rooms = new List<Room>();
            rooms = roomRepository.GetList(a => a.IsDelete != 1 && a.RoomStateId == readRoomInputDto.RoomStateId).OrderBy(a => a.RoomNumber).ToList();
            rooms.ForEach(source =>
            {
                var roomState = roomStates.SingleOrDefault(a => a.Id == source.RoomStateId);
                source.RoomState = roomState.Description.IsNullOrEmpty() ? "" : roomState.Description;
                var roomType = roomTypes.SingleOrDefault(a => a.RoomTypeId == source.RoomTypeId);
                source.RoomName = roomType.RoomTypeName.IsNullOrEmpty() ? "" : roomType.RoomTypeName;
            });

            var listSource = EntityMapper.MapList<Room, ReadRoomOutputDto>(rooms);

            return new ListOutputDto<ReadRoomOutputDto> { listSource = listSource };
        }

        /// <summary>
        /// 根据房间状态来查询可使用的房间
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectCanUseRoomAll()
        {
            List<EnumDto> roomStates = new List<EnumDto>();
            var helper = new EnumHelper();
            roomStates = Enum.GetValues(typeof(RoomState))
            .Cast<RoomState>()
            .Select(e => new EnumDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = helper.GetEnumDescription(e)
            })
            .ToList();
            List<RoomType> roomTypes = new List<RoomType>();
            roomTypes = roomTypeRepository.GetList();
            List<Room> rooms = new List<Room>();
            rooms = roomRepository.GetList(a => a.IsDelete != 1 && a.RoomStateId == (int)RoomState.Vacant).OrderBy(a => a.RoomNumber).ToList();
            rooms.ForEach(source =>
            {
                var roomState = roomStates.SingleOrDefault(a => a.Id == source.RoomStateId);
                source.RoomState = roomState.Description.IsNullOrEmpty() ? "" : roomState.Description;
                var roomType = roomTypes.SingleOrDefault(a => a.RoomTypeId == source.RoomTypeId);
                source.RoomName = roomType.RoomTypeName.IsNullOrEmpty() ? "" : roomType.RoomTypeName;
            });

            var listSource = EntityMapper.MapList<Room, ReadRoomOutputDto>(rooms);

            return new ListOutputDto<ReadRoomOutputDto> { listSource = listSource };
        }

        /// <summary>
        /// 获取所有房间信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectRoomAll(ReadRoomInputDto readRoomInputDto)
        {
            var where = Expressionable.Create<Room>();

            where = where.And(a => a.IsDelete == readRoomInputDto.IsDelete);

            if (readRoomInputDto.RoomStateId > 0)
            {
                where = where.And(a => a.RoomStateId == readRoomInputDto.RoomStateId);
            }

            List<EnumDto> roomStates = new List<EnumDto>();
            var helper = new EnumHelper();
            roomStates = Enum.GetValues(typeof(RoomState))
            .Cast<RoomState>()
            .Select(e => new EnumDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = helper.GetEnumDescription(e)
            })
            .ToList();
            List<RoomType> roomTypes = new List<RoomType>();
            roomTypes = roomTypeRepository.GetList();
            List<Room> rooms = new List<Room>();

            var count = 0;

            if (!readRoomInputDto.IgnorePaging && readRoomInputDto.Page != 0 && readRoomInputDto.PageSize != 0)
            {
                rooms = roomRepository.AsQueryable().Where(where.ToExpression()).OrderBy(a => a.RoomNumber).ToPageList(readRoomInputDto.Page, readRoomInputDto.PageSize, ref count);
            }
            else
            {
                rooms = roomRepository.GetList(a => a.IsDelete != 1).OrderBy(a => a.RoomNumber).ToList();
            }

            var listCustoNo = rooms.Select(a => a.CustomerNumber).Distinct().ToList();
            List<Customer> custos = new List<Customer>();
            custos = custoRepository.GetList(a => listCustoNo.Contains(a.CustomerNumber));

            rooms.ForEach(source =>
            {
                var roomState = roomStates.SingleOrDefault(a => a.Id == source.RoomStateId);
                source.RoomState = roomState.Description.IsNullOrEmpty() ? "" : roomState.Description;
                var roomType = roomTypes.SingleOrDefault(a => a.RoomTypeId == source.RoomTypeId);
                source.RoomName = roomType.RoomTypeName.IsNullOrEmpty() ? "" : roomType.RoomTypeName;

                var custo = custos.SingleOrDefault(a => a.CustomerNumber.Equals(source.CustomerNumber));
                source.CustomerName = custo.IsNullOrEmpty() ? "" : custo.CustomerName;

                //把入住时间格式化
                source.LastCheckInTimeFormatted = (source.LastCheckInTime + "").IsNullOrEmpty() ? ""
                : Convert.ToDateTime(source.LastCheckInTime).ToString("yyyy-MM-dd HH:mm");

            });
            var listSource = EntityMapper.MapList<Room, ReadRoomOutputDto>(rooms);

            return new ListOutputDto<ReadRoomOutputDto> { listSource = listSource, total = count };
        }

        /// <summary>
        /// 获取房间分区的信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectRoomByTypeName(ReadRoomInputDto readRoomInputDto)
        {
            List<EnumDto> roomStates = new List<EnumDto>();
            var helper = new EnumHelper();
            roomStates = Enum.GetValues(typeof(RoomState))
            .Cast<RoomState>()
            .Select(e => new EnumDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = helper.GetEnumDescription(e)
            })
            .ToList();
            List<RoomType> roomTypes = new List<RoomType>();
            roomTypes = roomTypeRepository.GetList(a => a.IsDelete != 1 && a.RoomTypeName == readRoomInputDto.RoomTypeName);
            var listTypes = roomTypes.Select(a => a.RoomTypeId).Distinct().ToList();
            List<Room> rooms = new List<Room>();
            rooms = roomRepository.GetList(a => a.IsDelete != 1 && listTypes.Contains(a.RoomTypeId)).OrderBy(a => a.RoomNumber).ToList();
            var listCustoNo = rooms.Select(a => a.CustomerNumber).Distinct().ToList();
            List<Customer> custos = new List<Customer>();
            custos = custoRepository.GetList(a => listCustoNo.Contains(a.CustomerNumber));
            rooms.ForEach(source =>
            {
                var roomState = roomStates.SingleOrDefault(a => a.Id == source.RoomStateId);
                source.RoomState = roomState.Description.IsNullOrEmpty() ? "" : roomState.Description;
                var roomType = roomTypes.SingleOrDefault(a => a.RoomTypeId == source.RoomTypeId);
                source.RoomName = roomType.RoomTypeName.IsNullOrEmpty() ? "" : roomType.RoomTypeName;

                var custo = custos.SingleOrDefault(a => a.CustomerNumber.Equals(source.CustomerNumber));
                source.CustomerName = custo.IsNullOrEmpty() ? "" : custo.CustomerName;

            });
            var listSource = EntityMapper.MapList<Room, ReadRoomOutputDto>(rooms);

            return new ListOutputDto<ReadRoomOutputDto> { listSource = listSource };
        }

        /// <summary>
        /// 根据房间编号查询房间信息
        /// </summary>
        /// <param name="readRoomInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomOutputDto> SelectRoomByRoomNo(ReadRoomInputDto readRoomInputDto)
        {
            List<EnumDto> roomStates = new List<EnumDto>();
            var helper = new EnumHelper();
            roomStates = Enum.GetValues(typeof(RoomState))
            .Cast<RoomState>()
            .Select(e => new EnumDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = helper.GetEnumDescription(e)
            })
            .ToList();
            Room room = new Room();
            room = roomRepository.GetSingle(a => a.IsDelete != 1 && a.RoomNumber == readRoomInputDto.RoomNumber);
            if (!room.IsNullOrEmpty())
            {
                var roomSate = roomStates.SingleOrDefault(a => a.Id == room.RoomStateId);
                room.RoomState = roomSate.Description.IsNullOrEmpty() ? "" : roomSate.Description;
                var roomType = roomTypeRepository.GetSingle(a => a.RoomTypeId == room.RoomTypeId);
                room.RoomName = roomType.RoomTypeName.IsNullOrEmpty() ? "" : roomType.RoomTypeName;
            }
            else
            {
                room = new Room();
            }

            var Source = EntityMapper.Map<Room, ReadRoomOutputDto>(room);

            return new SingleOutputDto<ReadRoomOutputDto>() { Source = Source };
        }

        /// <summary>
        /// 根据房间编号查询截止到今天住了多少天
        /// </summary>
        /// <param name="roomInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomOutputDto> DayByRoomNo(ReadRoomInputDto roomInputDto)
        {
            var days = Math.Abs(((TimeSpan)(roomRepository.GetSingle(a => a.RoomNumber == roomInputDto.RoomNumber).LastCheckInTime - DateTime.Now)).Days);
            return new SingleOutputDto<ReadRoomOutputDto> { Source = new ReadRoomOutputDto { StayDays = days } };
        }

        /// <summary>
        /// 根据房间编号修改房间信息（入住）
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateRoomInfo(UpdateRoomInputDto r)
        {
            try
            {
                roomRepository.Update(a => new Room()
                {
                    LastCheckInTime = r.LastCheckInTime,
                    RoomStateId = r.RoomStateId,
                    CustomerNumber = r.CustomerNumber,
                    DataChgDate = r.DataChgDate,
                    DataChgUsr = r.DataChgUsr,
                }, a => a.RoomNumber == r.RoomNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 根据房间编号修改房间信息（预约）
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateRoomInfoWithReser(UpdateRoomInputDto r)
        {
            try
            {
                roomRepository.Update(a => new Room()
                {
                    RoomStateId = r.RoomStateId,
                    DataChgUsr = r.DataChgUsr,
                    DataInsDate = r.DataInsDate,
                }, a => a.RoomNumber == r.RoomNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>  
        /// 查询可入住房间数量  
        /// </summary>  
        /// <returns></returns>  
        public SingleOutputDto<ReadRoomOutputDto> SelectCanUseRoomAllByRoomState()
        {
            try
            {
                var count = roomRepository.Count(a => a.RoomStateId == (int)RoomState.Vacant && a.IsDelete != 1);
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Source = new ReadRoomOutputDto { Vacant = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    StatusCode = StatusCodeConstants.InternalServerError
                };
            }
        }

        /// <summary>
        /// 查询已入住房间数量
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomOutputDto> SelectNotUseRoomAllByRoomState()
        {
            try
            {
                var count = roomRepository.Count(a => a.RoomStateId == (int)RoomState.Occupied && a.IsDelete != 1);
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Source = new ReadRoomOutputDto { Occupied = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    StatusCode = StatusCodeConstants.InternalServerError
                };
            }
        }

        /// <summary>
        /// 根据房间编号查询房间价格
        /// </summary>
        /// <returns></returns>
        public object SelectRoomByRoomPrice(ReadRoomInputDto r)
        {
            return roomRepository.GetSingle(a => a.RoomNumber == r.RoomNumber).RoomRent;
        }

        /// <summary>
        /// 查询脏房数量
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomOutputDto> SelectNotClearRoomAllByRoomState()
        {
            try
            {
                var count = roomRepository.Count(a => a.RoomStateId == (int)RoomState.Dirty && a.IsDelete != 1);
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Source = new ReadRoomOutputDto { Dirty = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    StatusCode = StatusCodeConstants.InternalServerError
                };
            }
        }

        /// <summary>
        /// 查询维修房数量
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomOutputDto> SelectFixingRoomAllByRoomState()
        {
            try
            {
                var count = roomRepository.Count(a => a.RoomStateId == (int)RoomState.Maintenance && a.IsDelete != 1);
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Source = new ReadRoomOutputDto { Maintenance = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    StatusCode = StatusCodeConstants.InternalServerError
                };
            }
        }

        /// <summary>
        /// 查询预约房数量
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomOutputDto> SelectReservedRoomAllByRoomState()
        {
            try
            {
                var count = roomRepository.Count(a => a.RoomStateId == (int)RoomState.Reserved && a.IsDelete != 1);
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Source = new ReadRoomOutputDto { Reserved = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    StatusCode = StatusCodeConstants.InternalServerError
                };
            }
        }

        /// <summary>
        /// 根据房间编号更改房间状态
        /// </summary>
        /// <param name="updateRoomInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateRoomStateByRoomNo(UpdateRoomInputDto updateRoomInputDto)
        {
            try
            {
                roomRepository.Update(a => new Room()
                {
                    RoomStateId = updateRoomInputDto.RoomStateId
                }, a => a.RoomNumber == updateRoomInputDto.RoomNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="rn"></param>
        /// <returns></returns>
        public BaseOutputDto InsertRoom(CreateRoomInputDto rn)
        {
            try
            {
                var isExist = roomRepository.IsAny(a => a.RoomNumber == rn.RoomNumber);
                if (isExist)
                    return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString("This room already exists.", "房间已存在。"), StatusCode = StatusCodeConstants.InternalServerError };

                roomRepository.Insert(EntityMapper.Map<CreateRoomInputDto, Room>(rn));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }

            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新房间
        /// </summary>
        /// <param name="rn"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateRoom(UpdateRoomInputDto rn)
        {
            try
            {
                var isExist = roomRepository.IsAny(a => a.RoomNumber == rn.RoomNumber);
                if (!isExist)
                    return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString("This room does not exist.", "房间不存在。"), StatusCode = StatusCodeConstants.InternalServerError };
                roomRepository.Update(EntityMapper.Map<UpdateRoomInputDto, Room>(rn));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }

            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除房间
        /// </summary>
        /// <param name="rn"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteRoom(DeleteRoomInputDto rn)
        {
            try
            {
                var isExist = roomRepository.IsAny(a => a.RoomNumber == rn.RoomNumber);
                if (!isExist)
                    return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString("This room does not exist.", "房间不存在。"), StatusCode = StatusCodeConstants.InternalServerError };
                roomRepository.Update(EntityMapper.Map<DeleteRoomInputDto, Room>(rn));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }

            return new BaseOutputDto();
        }

        /// <summary>
        /// 转房操作
        /// </summary>
        /// <param name="transferRoomDto"></param>
        /// <returns></returns>
        public BaseOutputDto TransferRoom(TransferRoomDto transferRoomDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var originalRoom = roomRepository.GetSingle(a => a.RoomNumber == transferRoomDto.OriginalRoomNumber);
                    var targetRoom = roomRepository.GetSingle(a => a.RoomNumber == transferRoomDto.TargetRoomNumber);
                    var stayDays = Math.Abs(((TimeSpan)(originalRoom.LastCheckInTime - DateTime.Now)).Days);
                    var originalRoomBill = originalRoom.RoomRent * stayDays;

                    //更新原房间状态
                    roomRepository.Update(a => new Room
                    {
                        CustomerNumber = null,
                        LastCheckInTime = null,
                        LastCheckOutTime = DateTime.Now,
                        RoomStateId = (int)RoomState.Dirty,
                        DataChgDate = transferRoomDto.DataChgDate,
                        DataChgUsr = transferRoomDto.DataChgUsr
                    }, a => a.RoomNumber == originalRoom.RoomNumber);

                    //更新目标房间状态
                    roomRepository.Update(a => new Room
                    {
                        CustomerNumber = originalRoom.CustomerNumber,
                        LastCheckInTime = originalRoom.LastCheckInTime,
                        RoomStateId = (int)RoomState.Occupied,
                        DataChgDate = transferRoomDto.DataChgDate,
                        DataChgUsr = transferRoomDto.DataChgUsr
                    }, a => a.RoomNumber == targetRoom.RoomNumber);

                    //转移原房间消费记录
                    var originalSpendNumbers = spendRepository.GetList(a => a.RoomNumber == transferRoomDto.OriginalRoomNumber
                        && a.CustomerNumber.Equals(transferRoomDto.CustomerNumber) && a.SettlementStatus.Equals(SpendConsts.UnSettle)
                        && a.IsDelete == 0).Select(a => a.SpendNumber).ToList();

                    if (originalSpendNumbers.Count > 0)
                    {
                        spendRepository.Update(a => new Spend
                        {
                            RoomNumber = transferRoomDto.TargetRoomNumber,
                            DataChgUsr = transferRoomDto.DataChgUsr,
                            DataChgDate = transferRoomDto.DataChgDate,
                        }, a => originalSpendNumbers.Contains(a.SpendNumber));
                    }

                    //添加旧房间消费记录
                    var originalSpend = new Spend
                    {
                        CustomerNumber = originalRoom.CustomerNumber,
                        RoomNumber = transferRoomDto.OriginalRoomNumber,
                        SpendNumber = uniqueCode.GetNewId("SP-"),
                        ProductNumber = transferRoomDto.OriginalRoomNumber,
                        ProductName = "居住" + transferRoomDto.OriginalRoomNumber + "共" + stayDays + "天",
                        ProductPrice = originalRoom.RoomRent,
                        ConsumptionTime = DateTime.Now,
                        SettlementStatus = SpendConsts.UnSettle,
                        ConsumptionQuantity = stayDays,
                        ConsumptionAmount = originalRoomBill,
                        IsDelete = 0,
                        DataInsDate = transferRoomDto.DataChgDate,
                        DataInsUsr = transferRoomDto.DataChgUsr
                    };
                    spendRepository.Insert(originalSpend);

                    scope.Complete();

                }
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 退房操作
        /// </summary>
        /// <param name="checkoutRoomDto"></param>
        /// <returns></returns>
        public BaseOutputDto CheckoutRoom(CheckoutRoomDto checkoutRoomDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var room = roomRepository.GetSingle(a => a.RoomNumber == checkoutRoomDto.RoomNumber);
                    //更新房间状态
                    roomRepository.Update(a => new Room
                    {
                        CustomerNumber = null,
                        LastCheckInTime = null,
                        LastCheckOutTime = DateTime.Now,
                        RoomStateId = (int)RoomState.Dirty,
                        DataChgDate = checkoutRoomDto.DataChgDate,
                        DataChgUsr = checkoutRoomDto.DataChgUsr
                    }, a => a.RoomNumber == room.RoomNumber);

                    //添加能源使用记录
                    var energy = new EnergyManagement
                    {
                        InformationId = uniqueCode.GetNewId("EM-"),
                        StartDate = (DateTime)room.LastCheckInTime,
                        EndDate = (DateTime)checkoutRoomDto.DataChgDate,
                        WaterUsage = checkoutRoomDto.WaterUsage,
                        PowerUsage = checkoutRoomDto.ElectricityUsage,
                        Recorder = checkoutRoomDto.DataChgUsr,
                        CustomerNumber = room.CustomerNumber,
                        RoomNumber = checkoutRoomDto.RoomNumber,
                        IsDelete = 0,
                        DataInsDate = checkoutRoomDto.DataChgDate,
                        DataInsUsr = checkoutRoomDto.DataChgUsr
                    };
                    energyRepository.Insert(energy);

                    //结算消费记录
                    var spendNumbers = spendRepository.GetList(a => a.RoomNumber == checkoutRoomDto.RoomNumber
                        && a.CustomerNumber.Equals(checkoutRoomDto.CustomerNumber) && a.SettlementStatus.Equals(SpendConsts.UnSettle)
                        && a.IsDelete == 0).Select(a => a.SpendNumber).ToList();
                    if (spendNumbers.Count > 0 )
                    {
                        spendRepository.Update(a => new Spend
                        {
                            SettlementStatus = SpendConsts.Settled,
                            DataChgDate = checkoutRoomDto.DataChgDate,
                            DataChgUsr = checkoutRoomDto.DataChgUsr
                        }, a => spendNumbers.Contains(a.SpendNumber));
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 查询所有可消费（已住）房间
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectRoomByStateAll()
        {
            List<EnumDto> roomStates = new List<EnumDto>();
            var helper = new EnumHelper();
            roomStates = Enum.GetValues(typeof(RoomState))
            .Cast<RoomState>()
            .Select(e => new EnumDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = helper.GetEnumDescription(e)
            })
            .ToList();
            List<RoomType> roomTypes = new List<RoomType>();
            roomTypes = roomTypeRepository.GetList(a => a.IsDelete != 1);
            List<Room> rooms = new List<Room>();
            rooms = roomRepository.GetList(a => a.IsDelete != 1 && a.RoomStateId == 1).OrderBy(a => a.RoomNumber).ToList();
            rooms.ForEach(source =>
            {
                var roomState = roomStates.SingleOrDefault(a => a.Id == source.RoomStateId);
                source.RoomState = roomState.Description.IsNullOrEmpty() ? "" : roomState.Description;
                var roomType = roomTypes.SingleOrDefault(a => a.RoomTypeId == source.RoomTypeId);
                source.RoomName = roomType.RoomTypeName.IsNullOrEmpty() ? "" : roomType.RoomTypeName;
            });

            var listSource = EntityMapper.MapList<Room, ReadRoomOutputDto>(rooms);

            return new ListOutputDto<ReadRoomOutputDto> { listSource = listSource };
        }

        /// <summary>
        /// 根据房间编号查询房间状态编号
        /// </summary>
        /// <param name="readRoomInputDto"></param>
        /// <returns></returns>
        public object SelectRoomStateIdByRoomNo(ReadRoomInputDto readRoomInputDto)
        {
            return roomRepository.GetSingle(a => a.RoomNumber == readRoomInputDto.RoomNumber).RoomStateId;
        }
    }
}
