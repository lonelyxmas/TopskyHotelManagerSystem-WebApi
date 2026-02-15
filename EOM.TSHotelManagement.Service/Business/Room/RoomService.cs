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
using EOM.TSHotelManagement.Common;
using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Data;
using EOM.TSHotelManagement.Domain;
using jvncorelib.CodeLib;
using jvncorelib.EntityLib;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Transactions;

namespace EOM.TSHotelManagement.Service
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
        /// 客户类型
        /// </summary>
        private readonly GenericRepository<CustoType> custoTypeRepository;

        /// <summary>
        /// 会员等级规则
        /// </summary>
        private readonly GenericRepository<VipLevelRule> vipLevelRuleRepository;

        /// <summary>
        /// 预约信息
        /// </summary>
        private readonly GenericRepository<Reser> reserRepository;

        /// <summary>
        /// 唯一编码
        /// </summary>
        private readonly UniqueCode uniqueCode;

        private readonly ILogger<RoomService> logger;

        public RoomService(GenericRepository<Room> roomRepository, GenericRepository<Spend> spendRepository, GenericRepository<RoomType> roomTypeRepository, GenericRepository<EnergyManagement> energyRepository, GenericRepository<Customer> custoRepository, GenericRepository<CustoType> custoTypeRepository, GenericRepository<VipLevelRule> vipLevelRuleRepository, GenericRepository<Reser> reserRepository, UniqueCode uniqueCode, ILogger<RoomService> logger)
        {
            this.roomRepository = roomRepository;
            this.spendRepository = spendRepository;
            this.roomTypeRepository = roomTypeRepository;
            this.energyRepository = energyRepository;
            this.custoRepository = custoRepository;
            this.custoTypeRepository = custoTypeRepository;
            this.vipLevelRuleRepository = vipLevelRuleRepository;
            this.reserRepository = reserRepository;
            this.uniqueCode = uniqueCode;
            this.logger = logger;
        }

        /// <summary>
        /// 根据房间状态获取相应状态的房间信息
        /// </summary>
        /// <param name="readRoomInputDto"></param>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectRoomByRoomState(ReadRoomInputDto readRoomInputDto)
        {
            return BuildRoomList(readRoomInputDto);
        }

        /// <summary>
        /// 根据房间状态来查询可使用的房间
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectCanUseRoomAll()
        {
            var rooms = roomRepository.GetList(a => a.RoomStateId == (int)RoomState.Vacant);
            var result = EntityMapper.MapList<Room, ReadRoomOutputDto>(rooms);
            return new ListOutputDto<ReadRoomOutputDto>
            {
                Data = new PagedData<ReadRoomOutputDto>
                {
                    Items = result,
                    TotalCount = result.Count
                }
            };
        }

        /// <summary>
        /// 获取所有房间信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectRoomAll(ReadRoomInputDto readRoomInputDto)
        {
            return BuildRoomList(readRoomInputDto);
        }

        /// <summary>
        /// 获取房间分区的信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRoomOutputDto> SelectRoomByTypeName(ReadRoomInputDto readRoomInputDto)
        {
            return BuildRoomList(readRoomInputDto);
        }

        private ListOutputDto<ReadRoomOutputDto> BuildRoomList(ReadRoomInputDto readRoomInputDto)
        {
            readRoomInputDto ??= new ReadRoomInputDto();

            var where = SqlFilterBuilder.BuildExpression<Room, ReadRoomInputDto>(readRoomInputDto);
            var query = roomRepository.AsQueryable().Where(a => a.IsDelete != 1);
            var whereExpression = where.ToExpression();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            query = query.OrderBy(a => a.RoomNumber);

            var count = 0;
            List<Room> rooms;
            if (!readRoomInputDto.IgnorePaging)
            {
                var page = readRoomInputDto.Page > 0 ? readRoomInputDto.Page : 1;
                var pageSize = readRoomInputDto.PageSize > 0 ? readRoomInputDto.PageSize : 15;
                rooms = query.ToPageList(page, pageSize, ref count);
            }
            else
            {
                rooms = query.ToList();
                count = rooms.Count;
            }

            var roomTypeMap = roomTypeRepository.AsQueryable()
                .Where(a => a.IsDelete != 1)
                .ToList()
                .GroupBy(a => a.RoomTypeId)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.RoomTypeName ?? "");

            var customerNumbers = rooms
                .Select(a => a.CustomerNumber)
                .Where(a => !a.IsNullOrEmpty())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var customerMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (customerNumbers.Count > 0)
            {
                customerMap = custoRepository.AsQueryable()
                    .Where(a => a.IsDelete != 1 && customerNumbers.Contains(a.CustomerNumber))
                    .ToList()
                    .GroupBy(a => a.CustomerNumber)
                    .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.CustomerName ?? "", StringComparer.OrdinalIgnoreCase);
            }

            var helper = new EnumHelper();
            var roomStateMap = Enum.GetValues(typeof(RoomState))
                .Cast<RoomState>()
                .ToDictionary(e => (int)e, e => helper.GetEnumDescription(e) ?? "");

            List<ReadRoomOutputDto> result;
            var useParallelProjection = readRoomInputDto.IgnorePaging && rooms.Count >= 200;
            if (useParallelProjection)
            {
                var dtoArray = new ReadRoomOutputDto[rooms.Count];
                System.Threading.Tasks.Parallel.For(0, rooms.Count, i =>
                {
                    var source = rooms[i];
                    dtoArray[i] = new ReadRoomOutputDto
                    {
                        Id = source.Id,
                        RoomNumber = source.RoomNumber,
                        RoomTypeId = source.RoomTypeId,
                        RoomName = roomTypeMap.TryGetValue(source.RoomTypeId, out var roomTypeName) ? roomTypeName : "",
                        CustomerNumber = source.CustomerNumber ?? "",
                        CustomerName = customerMap.TryGetValue(source.CustomerNumber ?? "", out var customerName) ? customerName : "",
                        LastCheckInTime = source.LastCheckInTime.HasValue ? source.LastCheckInTime.Value.ToDateTime(TimeOnly.MinValue) : null,
                        LastCheckOutTime = source.LastCheckOutTime == DateOnly.MinValue ? null : source.LastCheckOutTime.ToDateTime(TimeOnly.MinValue),
                        RoomStateId = source.RoomStateId,
                        RoomState = roomStateMap.TryGetValue(source.RoomStateId, out var roomStateName) ? roomStateName : "",
                        RoomRent = source.RoomRent,
                        RoomDeposit = source.RoomDeposit,
                        RoomLocation = source.RoomLocation,
                        DataInsUsr = source.DataInsUsr,
                        DataInsDate = source.DataInsDate,
                        DataChgUsr = source.DataChgUsr,
                        DataChgDate = source.DataChgDate,
                        IsDelete = source.IsDelete
                    };
                });
                result = dtoArray.ToList();
            }
            else
            {
                result = new List<ReadRoomOutputDto>(rooms.Count);
                rooms.ForEach(source =>
                {
                    result.Add(new ReadRoomOutputDto
                    {
                        Id = source.Id,
                        RoomNumber = source.RoomNumber,
                        RoomTypeId = source.RoomTypeId,
                        RoomName = roomTypeMap.TryGetValue(source.RoomTypeId, out var roomTypeName) ? roomTypeName : "",
                        CustomerNumber = source.CustomerNumber ?? "",
                        CustomerName = customerMap.TryGetValue(source.CustomerNumber ?? "", out var customerName) ? customerName : "",
                        LastCheckInTime = source.LastCheckInTime.HasValue ? source.LastCheckInTime.Value.ToDateTime(TimeOnly.MinValue) : null,
                        LastCheckOutTime = source.LastCheckOutTime == DateOnly.MinValue ? null : source.LastCheckOutTime.ToDateTime(TimeOnly.MinValue),
                        RoomStateId = source.RoomStateId,
                        RoomState = roomStateMap.TryGetValue(source.RoomStateId, out var roomStateName) ? roomStateName : "",
                        RoomRent = source.RoomRent,
                        RoomDeposit = source.RoomDeposit,
                        RoomLocation = source.RoomLocation,
                        DataInsUsr = source.DataInsUsr,
                        DataInsDate = source.DataInsDate,
                        DataChgUsr = source.DataChgUsr,
                        DataChgDate = source.DataChgDate,
                        IsDelete = source.IsDelete
                    });
                });
            }

            return new ListOutputDto<ReadRoomOutputDto>
            {
                Data = new PagedData<ReadRoomOutputDto>
                {
                    Items = result,
                    TotalCount = count
                }
            };
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
            room = roomRepository.GetFirst(a => a.IsDelete != 1 && a.RoomNumber == readRoomInputDto.RoomNumber);
            if (!room.IsNullOrEmpty())
            {
                var roomSate = roomStates.SingleOrDefault(a => a.Id == room.RoomStateId);
                room.RoomState = roomSate.Description.IsNullOrEmpty() ? "" : roomSate.Description;
                var roomType = roomTypeRepository.GetFirst(a => a.RoomTypeId == room.RoomTypeId);
                room.RoomName = roomType.RoomTypeName.IsNullOrEmpty() ? "" : roomType.RoomTypeName;
            }
            else
            {
                room = new Room();
            }

            var Data = EntityMapper.Map<Room, ReadRoomOutputDto>(room);

            return new SingleOutputDto<ReadRoomOutputDto>() { Data = Data };
        }

        /// <summary>
        /// 根据房间编号查询截止到今天住了多少天
        /// </summary>
        /// <param name="roomInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadRoomOutputDto> DayByRoomNo(ReadRoomInputDto roomInputDto)
        {
            var room = roomRepository.GetFirst(a => a.RoomNumber == roomInputDto.RoomNumber);
            if (room?.LastCheckInTime != null)
            {
                var days = Math.Abs((room.LastCheckInTime.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days);
                return new SingleOutputDto<ReadRoomOutputDto> { Data = new ReadRoomOutputDto { StayDays = days } };
            }
            return new SingleOutputDto<ReadRoomOutputDto> { Data = new ReadRoomOutputDto { StayDays = 0 } };
        }

        /// <summary>
        /// 根据房间编号修改房间信息（入住）
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public BaseResponse UpdateRoomInfo(UpdateRoomInputDto r)
        {
            try
            {
                var room = this.roomRepository.GetFirst(a => a.RoomNumber == r.RoomNumber);
                room.RoomStateId = r.RoomStateId;
                room.CustomerNumber = r.CustomerNumber;
                room.LastCheckInTime = r.LastCheckInTime;
                room.DataChgDate = r.DataChgDate;
                room.DataChgUsr = r.DataChgUsr;
                roomRepository.Update(room);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating room info for room number {RoomNumber}", r.RoomNumber);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary> 
        /// 根据房间编号修改房间信息（预约）
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public BaseResponse UpdateRoomInfoWithReser(UpdateRoomInputDto r)
        {
            try
            {
                var room = this.roomRepository.GetFirst(a => a.RoomNumber == r.RoomNumber);
                room.RoomStateId = r.RoomStateId;
                room.DataChgDate = r.DataChgDate;
                room.DataChgUsr = r.DataChgUsr;
                roomRepository.Update(room);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating room info with reservation for room number {RoomNumber}", r.RoomNumber);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
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
                    Data = new ReadRoomOutputDto { Vacant = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    Code = BusinessStatusCode.InternalServerError
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
                    Data = new ReadRoomOutputDto { Occupied = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    Code = BusinessStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// 根据房间编号查询房间价格
        /// </summary>
        /// <returns></returns>
        public object SelectRoomByRoomPrice(ReadRoomInputDto r)
        {
            return roomRepository.GetFirst(a => a.RoomNumber == r.RoomNumber).RoomRent;
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
                    Data = new ReadRoomOutputDto { Dirty = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    Code = BusinessStatusCode.InternalServerError
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
                    Data = new ReadRoomOutputDto { Maintenance = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    Code = BusinessStatusCode.InternalServerError
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
                    Data = new ReadRoomOutputDto { Reserved = count }
                };
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadRoomOutputDto>
                {
                    Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message),
                    Code = BusinessStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// 根据房间编号更改房间状态
        /// </summary>
        /// <param name="updateRoomInputDto"></param>
        /// <returns></returns>
        public BaseResponse UpdateRoomStateByRoomNo(UpdateRoomInputDto updateRoomInputDto)
        {
            try
            {
                var room = roomRepository.GetFirst(a => a.RoomNumber == updateRoomInputDto.RoomNumber);
                room.RoomStateId = updateRoomInputDto.RoomStateId;
                roomRepository.Update(room);
            }
            catch (Exception ex)
            {
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="rn"></param>
        /// <returns></returns>
        public BaseResponse InsertRoom(CreateRoomInputDto rn)
        {
            try
            {
                var isExist = roomRepository.IsAny(a => a.RoomNumber == rn.RoomNumber && a.IsDelete != 1);
                if (isExist)
                    return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("This room already exists.", "房间已存在。"), Code = BusinessStatusCode.InternalServerError };

                var entity = EntityMapper.Map<CreateRoomInputDto, Room>(rn);
                entity.LastCheckInTime = DateOnly.MinValue;
                entity.LastCheckOutTime = DateOnly.MinValue;
                roomRepository.Insert(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inserting room with room number {RoomNumber}", rn.RoomNumber);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }

            return new BaseResponse();
        }

        /// <summary>
        /// 更新房间
        /// </summary>
        /// <param name="rn"></param>
        /// <returns></returns>
        public BaseResponse UpdateRoom(UpdateRoomInputDto rn)
        {
            try
            {
                var isExist = roomRepository.IsAny(a => a.RoomNumber == rn.RoomNumber);
                if (!isExist)
                    return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("This room does not exist.", "房间不存在。"), Code = BusinessStatusCode.InternalServerError };

                var entity = EntityMapper.Map<UpdateRoomInputDto, Room>(rn);
                roomRepository.Update(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating room with room number {RoomNumber}", rn.RoomNumber);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("Failed to update room.", "更新房间失败。"), Code = BusinessStatusCode.InternalServerError };
            }

            return new BaseResponse();
        }

        /// <summary>
        /// 删除房间
        /// </summary>
        /// <param name="rn"></param>
        /// <returns></returns>
        public BaseResponse DeleteRoom(DeleteRoomInputDto rn)
        {
            try
            {
                if (rn?.DelIds == null || !rn.DelIds.Any())
                {
                    return new BaseResponse
                    {
                        Code = BusinessStatusCode.BadRequest,
                        Message = LocalizationHelper.GetLocalizedString("Parameters Invalid", "参数错误")
                    };
                }

                var rooms = roomRepository.GetList(a => rn.DelIds.Contains(a.Id));

                if (!rooms.Any())
                {
                    return new BaseResponse
                    {
                        Code = BusinessStatusCode.NotFound,
                        Message = LocalizationHelper.GetLocalizedString("Room Information Not Found", "房间信息未找到")
                    };
                }

                // 如果房间存在预约信息，则不允许删除
                var roomNumbers = rooms.Select(a => a.RoomNumber).ToList();
                var hasReservation = reserRepository.IsAny(a => roomNumbers.Contains(a.ReservationRoomNumber) && a.IsDelete != 1 && a.ReservationEndDate >= DateOnly.FromDateTime(DateTime.Today));
                if (hasReservation)
                {
                    return new BaseResponse
                    {
                        Code = BusinessStatusCode.Conflict,
                        Message = LocalizationHelper.GetLocalizedString("Cannot delete rooms with active reservations", "无法删除存在有效预约的房间")
                    };
                }

                var result = roomRepository.SoftDeleteRange(rooms);

                return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Delete Room Success", "房间信息删除成功"));
            }
            catch (Exception ex)
            {
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// 转房操作
        /// </summary>
        /// <param name="transferRoomDto"></param>
        /// <returns></returns>
        public BaseResponse TransferRoom(TransferRoomDto transferRoomDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var customer = custoRepository.GetFirst(a => a.CustomerNumber == transferRoomDto.CustomerNumber && a.IsDelete != 1);
                    if (customer.IsNullOrEmpty())
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("The customer does not exist", "客户不存在"), Code = BusinessStatusCode.InternalServerError };

                    var originalSpends = spendRepository.GetList(a => a.RoomNumber == transferRoomDto.OriginalRoomNumber
                        && a.CustomerNumber == transferRoomDto.CustomerNumber && a.SettlementStatus == ConsumptionConstant.UnSettle.Code
                        && a.IsDelete == 0).ToList();

                    var vipRules = vipLevelRuleRepository.GetList(a => a.IsDelete != 1).ToList();

                    var originalRoom = roomRepository.GetFirst(a => a.RoomNumber == transferRoomDto.OriginalRoomNumber);
                    if (originalRoom.CustomerNumber != transferRoomDto.CustomerNumber)
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("The customer does not match the original room", "客户与原房间不匹配"), Code = BusinessStatusCode.InternalServerError };

                    if (!originalRoom.LastCheckInTime.HasValue)
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("The original room lacks check-in time", "原房间缺少入住时间"), Code = BusinessStatusCode.InternalServerError };

                    var targetRoom = roomRepository.GetFirst(a => a.RoomNumber == transferRoomDto.TargetRoomNumber);
                    if (targetRoom.IsNullOrEmpty())
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("The room does not exist", "房间不存在"), Code = BusinessStatusCode.InternalServerError };
                    if (targetRoom.RoomStateId != (int)RoomState.Vacant)
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("The room is not vacant", "房间不处于空房状态"), Code = BusinessStatusCode.InternalServerError };

                    var staySpan = DateTime.Now - originalRoom.LastCheckInTime.Value.ToDateTime(TimeOnly.MinValue);
                    var stayDays = Math.Max((int)Math.Ceiling(staySpan.TotalDays), 1);

                    var originalSpendNumbers = originalSpends.Select(a => a.SpendNumber).ToList();
                    var TotalCountSpent = originalSpends.Sum(a => a.ConsumptionAmount);

                    var newLevelId = vipRules
                            .Where(vipRule => TotalCountSpent >= vipRule.RuleValue)
                            .OrderByDescending(vipRule => vipRule.RuleValue)
                            .ThenByDescending(vipRule => vipRule.VipLevelId)
                            .FirstOrDefault()?.VipLevelId ?? 0;

                    if (newLevelId != 0)
                    {
                        custoRepository.Update(a => new Customer
                        {
                            CustomerType = newLevelId
                        }, a => a.CustomerNumber == transferRoomDto.CustomerNumber);
                    }

                    var customerType = custoTypeRepository.GetFirst(a => a.CustomerType == customer.CustomerType);
                    if (customerType.IsNullOrEmpty())
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("The customer type does not exist", "客户类型不存在"), Code = BusinessStatusCode.InternalServerError };

                    decimal discount = (customerType != null && customerType.Discount > 0 && customerType.Discount < 100)
                            ? customerType.Discount / 100M
                            : 1M;
                    decimal originalRoomBill = originalRoom.RoomRent * stayDays * discount;

                    //更新目标房间状态
                    targetRoom.CustomerNumber = originalRoom.CustomerNumber;
                    targetRoom.RoomStateId = (int)RoomState.Occupied;
                    targetRoom.LastCheckInTime = DateOnly.FromDateTime(DateTime.Now);
                    roomRepository.Update(targetRoom);

                    //更新原房间状态
                    originalRoom.CustomerNumber = string.Empty;
                    originalRoom.RoomStateId = (int)RoomState.Dirty;
                    originalRoom.LastCheckInTime = DateOnly.MinValue;
                    originalRoom.LastCheckOutTime = DateOnly.MinValue;
                    roomRepository.Update(originalRoom);

                    //转移原房间消费记录
                    if (originalSpendNumbers.Count > 0)
                    {
                        var originalSpendList = spendRepository.AsQueryable().Where(a => originalSpendNumbers.Contains(a.SpendNumber)).ToList();
                        var spends = new List<Spend>();

                        foreach (var spend in originalSpendList)
                        {
                            spend.SpendNumber = spend.SpendNumber;
                            spend.RoomNumber = transferRoomDto.TargetRoomNumber;
                        }

                        spendRepository.UpdateRange(spends);
                    }

                    //添加旧房间消费记录
                    var originalSpend = new Spend
                    {
                        CustomerNumber = transferRoomDto.CustomerNumber,
                        RoomNumber = transferRoomDto.TargetRoomNumber,
                        SpendNumber = uniqueCode.GetNewId("SP-"),
                        ProductNumber = transferRoomDto.OriginalRoomNumber,
                        ProductName = "居住" + transferRoomDto.OriginalRoomNumber + "共" + stayDays + "天",
                        ProductPrice = originalRoom.RoomRent,
                        ConsumptionTime = DateTime.Now,
                        SettlementStatus = ConsumptionConstant.UnSettle.Code,
                        ConsumptionQuantity = stayDays,
                        ConsumptionAmount = originalRoomBill,
                        ConsumptionType = SpendTypeConstant.Room.Code,
                        IsDelete = 0
                    };
                    spendRepository.Insert(originalSpend);

                    scope.Complete();

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error transferring room from {OriginalRoomNumber} to {TargetRoomNumber} for customer {CustomerNumber}", transferRoomDto.OriginalRoomNumber, transferRoomDto.TargetRoomNumber, transferRoomDto.CustomerNumber);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary>
        /// 退房操作
        /// </summary>
        /// <param name="checkoutRoomDto"></param>
        /// <returns></returns>
        public BaseResponse CheckoutRoom(CheckoutRoomDto checkoutRoomDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var customer = custoRepository.AsQueryable().Where(a => a.CustomerNumber == checkoutRoomDto.CustomerNumber && a.IsDelete != 1);
                    if (!customer.Any())
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("The customer does not exist", "客户不存在"), Code = BusinessStatusCode.InternalServerError };

                    var room = roomRepository.GetFirst(a => a.RoomNumber == checkoutRoomDto.RoomNumber);
                    //更新房间状态
                    room.CustomerNumber = string.Empty;
                    room.LastCheckOutTime = DateOnly.FromDateTime(DateTime.Now);
                    room.RoomStateId = (int)RoomState.Dirty;
                    roomRepository.Update(room);

                    //添加能源使用记录
                    var energy = new EnergyManagement
                    {
                        InformationId = uniqueCode.GetNewId("EM-"),
                        StartDate = (DateOnly)room.LastCheckInTime,
                        EndDate = DateOnly.FromDateTime((DateTime)checkoutRoomDto.DataChgDate),
                        WaterUsage = checkoutRoomDto.WaterUsage,
                        PowerUsage = checkoutRoomDto.ElectricityUsage,
                        Recorder = checkoutRoomDto.DataChgUsr,
                        CustomerNumber = room.CustomerNumber,
                        RoomNumber = checkoutRoomDto.RoomNumber,
                        IsDelete = 0
                    };
                    energyRepository.Insert(energy);

                    //结算消费记录
                    var spendNumbers = spendRepository.GetList(a => a.RoomNumber == checkoutRoomDto.RoomNumber
                        && a.CustomerNumber.Equals(checkoutRoomDto.CustomerNumber) && a.SettlementStatus == ConsumptionConstant.UnSettle.Code
                        && a.IsDelete == 0).ToList();
                    if (spendNumbers.Count > 0)
                    {
                        var spends = new List<Spend>();
                        foreach (var spend in spendNumbers)
                        {
                            spend.SettlementStatus = ConsumptionConstant.Settled.Code;
                            spends.Add(spend);
                        }

                        spendRepository.UpdateRange(spends);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking out room number {RoomNumber} for customer {CustomerNumber}", checkoutRoomDto.RoomNumber, checkoutRoomDto.CustomerNumber);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary>
        /// 根据预约信息办理入住
        /// </summary>
        /// <param name="checkinRoomByReservationDto"></param>
        /// <returns></returns>
        public BaseResponse CheckinRoomByReservation(CheckinRoomByReservationDto checkinRoomByReservationDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var customer = new Customer
                    {
                        CustomerNumber = checkinRoomByReservationDto.CustomerNumber,
                        CustomerName = checkinRoomByReservationDto.CustomerName,
                        CustomerGender = checkinRoomByReservationDto.CustomerGender,
                        CustomerPhoneNumber = checkinRoomByReservationDto.CustomerPhoneNumber,
                        PassportId = checkinRoomByReservationDto.PassportId,
                        IdCardNumber = checkinRoomByReservationDto.IdCardNumber,
                        CustomerAddress = checkinRoomByReservationDto.CustomerAddress,
                        DateOfBirth = checkinRoomByReservationDto.DateOfBirth,
                        CustomerType = checkinRoomByReservationDto.CustomerType,
                        IsDelete = 0,
                        DataInsUsr = checkinRoomByReservationDto.DataInsUsr,
                        DataInsDate = checkinRoomByReservationDto.DataInsDate
                    };
                    var customerResult = custoRepository.Insert(customer);
                    if (!customerResult)
                    {
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("Failed to add customer.", "添加客户失败。"), Code = BusinessStatusCode.InternalServerError };
                    }

                    var room = roomRepository.GetFirst(a => a.RoomNumber == checkinRoomByReservationDto.RoomNumber && a.IsDelete != 1);
                    room.LastCheckInTime = DateOnly.FromDateTime(DateTime.Now);
                    room.CustomerNumber = customer.CustomerNumber;
                    room.RoomStateId = new EnumHelper().GetEnumValue(RoomState.Occupied);
                    var roomUpdateResult = roomRepository.Update(room);

                    if (!roomUpdateResult)
                    {
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("Failed to update room.", "更新房间失败。"), Code = BusinessStatusCode.InternalServerError };
                    }

                    var reser = reserRepository.GetFirst(a => a.ReservationId == checkinRoomByReservationDto.ReservationId && a.IsDelete != 1);
                    reser.IsDelete = 1;
                    var reserUpdateResult = reserRepository.Update(reser);

                    if (!reserUpdateResult)
                    {
                        return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("Failed to update reservation.", "更新预约失败。"), Code = BusinessStatusCode.InternalServerError };
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking in room number {RoomNumber} by reservation ID {ReservationId}", checkinRoomByReservationDto.RoomNumber, checkinRoomByReservationDto.ReservationId);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }
    }
}
