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
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Transactions;

namespace EOM.TSHotelManagement.Service
{
    /// <summary>
    /// 预约信息接口实现类
    /// </summary>
    public class ReserService : IReserService
    {
        /// <summary>
        /// 预约信息
        /// </summary>
        private readonly GenericRepository<Reser> reserRepository;

        /// <summary>
        /// 房间信息
        /// </summary>
        private readonly GenericRepository<Room> roomRepository;

        /// <summary>
        /// 数据保护
        /// </summary>
        private readonly DataProtectionHelper dataProtector;

        private readonly ILogger<ReserService> logger;

        public ReserService(GenericRepository<Reser> reserRepository, GenericRepository<Room> roomRepository, DataProtectionHelper dataProtector, ILogger<ReserService> logger)
        {
            this.reserRepository = reserRepository;
            this.roomRepository = roomRepository;
            this.dataProtector = dataProtector;
            this.logger = logger;
        }

        /// <summary>
        /// 获取所有预约信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadReserOutputDto> SelectReserAll(ReadReserInputDto readReserInputDto)
        {
            readReserInputDto ??= new ReadReserInputDto();

            var helper = new EnumHelper();
            var reserTypeMap = Enum.GetValues(typeof(ReserType))
                .Cast<ReserType>()
                .ToDictionary(e => e.ToString(), e => helper.GetEnumDescription(e) ?? "", StringComparer.OrdinalIgnoreCase);

            var where = SqlFilterBuilder.BuildExpression<Reser, ReadReserInputDto>(readReserInputDto, new Dictionary<string, string> 
            {
                { nameof(ReadReserInputDto.ReservationStartDateStart), nameof(Reser.ReservationStartDate) },
                { nameof(ReadReserInputDto.ReservationStartDateEnd), nameof(Reser.ReservationEndDate)  },
            });
            var query = reserRepository.AsQueryable().Where(a => a.IsDelete != 1);
            var whereExpression = where.ToExpression();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            var count = 0;
            List<Reser> data;
            if (!readReserInputDto.IgnorePaging)
            {
                var page = readReserInputDto.Page > 0 ? readReserInputDto.Page : 1;
                var pageSize = readReserInputDto.PageSize > 0 ? readReserInputDto.PageSize : 15;
                data = query.ToPageList(page, pageSize, ref count);
            }
            else
            {
                data = query.ToList();
                count = data.Count;
            }

            List<ReadReserOutputDto> mapped;
            var useParallelProjection = readReserInputDto.IgnorePaging && data.Count >= 200;
            if (useParallelProjection)
            {
                var dtoArray = new ReadReserOutputDto[data.Count];
                System.Threading.Tasks.Parallel.For(0, data.Count, i =>
                {
                    var source = data[i];
                    dtoArray[i] = new ReadReserOutputDto
                    {
                        Id = source.Id,
                        ReservationId = source.ReservationId,
                        CustomerName = source.CustomerName,
                        ReservationPhoneNumber = dataProtector.SafeDecryptReserData(source.ReservationPhoneNumber),
                        ReservationRoomNumber = source.ReservationRoomNumber,
                        ReservationChannel = source.ReservationChannel,
                        ReservationChannelDescription = reserTypeMap.TryGetValue(source.ReservationChannel ?? "", out var channelDescription) ? channelDescription : "",
                        ReservationStartDate = source.ReservationStartDate.ToDateTime(TimeOnly.MinValue),
                        ReservationEndDate = source.ReservationEndDate.ToDateTime(TimeOnly.MinValue),
                        DataInsUsr = source.DataInsUsr,
                        DataInsDate = source.DataInsDate,
                        DataChgUsr = source.DataChgUsr,
                        DataChgDate = source.DataChgDate,
                        IsDelete = source.IsDelete
                    };
                });
                mapped = dtoArray.ToList();
            }
            else
            {
                mapped = new List<ReadReserOutputDto>(data.Count);
                data.ForEach(source =>
                {
                    mapped.Add(new ReadReserOutputDto
                    {
                        Id = source.Id,
                        ReservationId = source.ReservationId,
                        CustomerName = source.CustomerName,
                        ReservationPhoneNumber = dataProtector.SafeDecryptReserData(source.ReservationPhoneNumber),
                        ReservationRoomNumber = source.ReservationRoomNumber,
                        ReservationChannel = source.ReservationChannel,
                        ReservationChannelDescription = reserTypeMap.TryGetValue(source.ReservationChannel ?? "", out var channelDescription) ? channelDescription : "",
                        ReservationStartDate = source.ReservationStartDate.ToDateTime(TimeOnly.MinValue),
                        ReservationEndDate = source.ReservationEndDate.ToDateTime(TimeOnly.MinValue),
                        DataInsUsr = source.DataInsUsr,
                        DataInsDate = source.DataInsDate,
                        DataChgUsr = source.DataChgUsr,
                        DataChgDate = source.DataChgDate,
                        IsDelete = source.IsDelete
                    });
                });
            }

            return new ListOutputDto<ReadReserOutputDto>
            {
                Data = new PagedData<ReadReserOutputDto>
                {
                    Items = mapped,
                    TotalCount = count
                }
            };
        }

        /// <summary>
        /// 根据房间编号获取预约信息
        /// </summary>
        /// <param name="readReserInputDt"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadReserOutputDto> SelectReserInfoByRoomNo(ReadReserInputDto readReserInputDt)
        {
            var helper = new EnumHelper();
            var reserType = Enum.GetValues(typeof(ReserType))
                .Cast<ReserType>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToList();

            Reser res = null;
            res = reserRepository.GetFirst(a => a.ReservationRoomNumber == readReserInputDt.ReservationRoomNumber && a.IsDelete != 1);
            //解密联系方式
            var sourceTelStr = dataProtector.SafeDecryptReserData(res.ReservationPhoneNumber);
            res.ReservationPhoneNumber = sourceTelStr;

            var outputReser = EntityMapper.Map<Reser, ReadReserOutputDto>(res);

            outputReser.ReservationChannelDescription = reserType.Where(a => a.Name == outputReser.ReservationChannel).Single().Description;

            return new SingleOutputDto<ReadReserOutputDto> { Data = outputReser };
        }

        /// <summary>
        /// 删除预约信息
        /// </summary>
        /// <param name="reser"></param>
        /// <returns></returns>
        public BaseResponse DeleteReserInfo(DeleteReserInputDto reser)
        {

            if (reser?.DelIds == null || !reser.DelIds.Any())
            {
                return new BaseResponse
                {
                    Code = BusinessStatusCode.BadRequest,
                    Message = LocalizationHelper.GetLocalizedString("Parameters Invalid", "参数错误")
                };
            }

            var resers = reserRepository.GetList(a => reser.DelIds.Contains(a.Id));

            if (!resers.Any())
            {
                return new BaseResponse
                {
                    Code = BusinessStatusCode.NotFound,
                    Message = LocalizationHelper.GetLocalizedString("Reservation Information Not Found", "预约信息未找到")
                };
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var roomNumbers = resers.Select(a => a.ReservationRoomNumber).ToList();

                    var result = reserRepository.SoftDeleteRange(resers);

                    if (result)
                    {
                        var rooms = roomRepository.AsQueryable().Where(a => roomNumbers.Contains(a.RoomNumber)).ToList();
                        rooms = rooms.Select(a =>
                        {
                            a.RoomStateId = (int)RoomState.Vacant;
                            return a;
                        }).ToList();
                        roomRepository.UpdateRange(rooms);
                        scope.Complete();
                        return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Delete Reser Success", "预约信息删除成功"));
                    }
                    else
                    {
                        return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Reser Failed", "预约信息删除失败"));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LocalizationHelper.GetLocalizedString("Delete Reser Failed", "预约信息删除失败"));
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Reser Failed", "预约信息删除失败"));
            }
        }

        /// <summary>
        /// 更新预约信息（支持恢复功能）
        /// </summary>
        /// <param name="reser"></param>
        /// <returns></returns>
        public BaseResponse UpdateReserInfo(UpdateReserInputDto reser)
        {
            string NewTel = dataProtector.EncryptReserData(reser.ReservationPhoneNumber);
            reser.ReservationPhoneNumber = NewTel;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    // 获取原预约（包括软删除的记录）
                    var originalReser = reserRepository.GetFirst(a => a.Id == reser.Id);

                    if (originalReser == null)
                    {
                        return new BaseResponse(BusinessStatusCode.NotFound,
                            LocalizationHelper.GetLocalizedString("Reservation not found", "预约信息不存在"));
                    }

                    bool isRestoring = originalReser.IsDelete == 1 && reser.IsDelete == 0;

                    // 如果是恢复操作
                    if (isRestoring)
                    {
                        // 检查原房间的当前状态
                        var room = roomRepository.GetFirst(a => a.RoomNumber == originalReser.ReservationRoomNumber);

                        if (room == null)
                        {
                            return new BaseResponse(BusinessStatusCode.Conflict,
                                LocalizationHelper.GetLocalizedString("Room does not exist, cannot restore reservation",
                                "关联的房间不存在，无法恢复预约"));
                        }

                        // 检查房间是否可用
                        if (room.RoomStateId != (int)RoomState.Vacant)
                        {
                            return new BaseResponse(BusinessStatusCode.Conflict,
                                string.Format(LocalizationHelper.GetLocalizedString(
                                    "Room {0} is currently occupied, cannot restore reservation",
                                    "房间{0}当前已被占用，无法恢复预约"),
                                room.RoomNumber));
                        }

                        // 检查时间段冲突（如果有时间字段）
                        var conflictingReservation = reserRepository.GetFirst(r =>
                                r.Id != originalReser.Id &&
                                r.IsDelete == 0 &&
                                r.ReservationRoomNumber == originalReser.ReservationRoomNumber &&
                                r.ReservationStartDate < originalReser.ReservationEndDate &&
                                r.ReservationEndDate > originalReser.ReservationStartDate);

                        if (conflictingReservation != null)
                        {
                            return new BaseResponse(BusinessStatusCode.Conflict,
                                LocalizationHelper.GetLocalizedString(
                                    "Room is already reserved during this period, cannot restore reservation",
                                    "该时间段内房间已被预订，无法恢复预约"));
                        }

                        // 恢复预约并更新房间状态
                        var entity = EntityMapper.Map<UpdateReserInputDto, Reser>(reser);
                        reserRepository.Update(entity);

                        room.RoomStateId = (int)RoomState.Reserved;
                        roomRepository.Update(room);
                    }
                    else
                    {
                        // 普通更新逻辑
                        var entity = EntityMapper.Map<UpdateReserInputDto, Reser>(reser);
                        reserRepository.Update(entity);
                    }

                    scope.Complete();
                    return new BaseResponse(BusinessStatusCode.Success,
                        LocalizationHelper.GetLocalizedString("Update Reservation Success", "预约信息更新成功"));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LocalizationHelper.GetLocalizedString("Update Customer Failed", "预约信息更新失败"));
                return new BaseResponse(BusinessStatusCode.InternalServerError,
                    LocalizationHelper.GetLocalizedString("Update Customer Failed", "预约信息更新失败"));
            }
        }

        /// <summary>
        /// 添加预约信息
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public BaseResponse InserReserInfo(CreateReserInputDto r)
        {
            string NewTel = dataProtector.EncryptReserData(r.ReservationPhoneNumber);
            r.ReservationPhoneNumber = NewTel;
            try
            {
                var entity = EntityMapper.Map<CreateReserInputDto, Reser>(r);
                reserRepository.Insert(entity);

                var room = roomRepository.GetFirst(a => a.RoomNumber == r.ReservationRoomNumber);
                room.RoomStateId = new EnumHelper().GetEnumValue(RoomState.Reserved);
                var updateResult = roomRepository.Update(room);

                if (!updateResult)
                {
                    return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Add Customer Failed", "预约信息添加失败"));
                }
                return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Add Customer Success", "预约信息添加成功"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LocalizationHelper.GetLocalizedString("Add Customer Failed", "预约信息添加失败"));
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Add Customer Failed", "预约信息添加失败"));
            }
        }

        /// <summary>
        /// 查询所有预约类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<EnumDto> SelectReserTypeAll()
        {
            var helper = new EnumHelper();
            var enumList = Enum.GetValues(typeof(ReserType))
                .Cast<ReserType>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToList();

            return new ListOutputDto<EnumDto>
            {
                Data = new PagedData<EnumDto>
                {
                    Items = enumList,
                    TotalCount = enumList.Count
                }
            };
        }

    }
}
