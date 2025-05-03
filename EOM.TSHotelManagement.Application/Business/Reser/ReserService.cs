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
using Microsoft.AspNetCore.DataProtection;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
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
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reserRepository"></param>
        /// <param name="roomRepository"></param>
        /// <param name="dataProtector"></param>
        public ReserService(GenericRepository<Reser> reserRepository, GenericRepository<Room> roomRepository, IDataProtectionProvider dataProtectionProvider)
        {
            this.reserRepository = reserRepository;
            this.roomRepository = roomRepository;
            this.dataProtector = dataProtectionProvider.CreateProtector("ReserInfoProtector");
        }

        /// <summary>
        /// 获取所有预约信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadReserOutputDto> SelectReserAll(ReadReserInputDto readReserInputDto)
        {
            ListOutputDto<ReadReserOutputDto> rss = new ListOutputDto<ReadReserOutputDto>();

            var where = Expressionable.Create<Reser>();

            where = where.And(a => a.IsDelete == readReserInputDto.IsDelete);

            var count = 0;

            var listSource = new List<Reser>();

            if (!readReserInputDto.IgnorePaging && readReserInputDto.Page != 0 && readReserInputDto.PageSize != 0)
            {
                listSource = reserRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readReserInputDto.Page, readReserInputDto.PageSize, ref count);
            }
            else
            {
                listSource = reserRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            listSource.ForEach(source =>
            {
                try
                {
                    //解密联系方式
                    var sourceTelStr = dataProtector.Unprotect(source.ReservationPhoneNumber);
                    source.ReservationPhoneNumber = sourceTelStr;
                }
                catch (Exception)
                {
                    source.ReservationPhoneNumber = source.ReservationPhoneNumber;
                }
            });

            rss.listSource = EntityMapper.MapList<Reser, ReadReserOutputDto>(listSource);
            rss.total = count;
            return rss;
        }

        /// <summary>
        /// 根据房间编号获取预约信息
        /// </summary>
        /// <param name="readReserInputDt"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadReserOutputDto> SelectReserInfoByRoomNo(ReadReserInputDto readReserInputDt)
        {
            Reser res = null;
            res = reserRepository.GetSingle(a => a.ReservationRoomNumber == readReserInputDt.ReservationRoomNumber && a.IsDelete != 1);
            //解密联系方式
            var sourceTelStr = dataProtector.Unprotect(res.ReservationPhoneNumber);
            res.ReservationPhoneNumber = sourceTelStr;

            var outputReser = EntityMapper.Map<Reser, ReadReserOutputDto>(res);

            return new SingleOutputDto<ReadReserOutputDto> { Source = outputReser };
        }

        /// <summary>
        /// 删除预约信息
        /// </summary>
        /// <param name="reser"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteReserInfo(DeleteReserInputDto reser)
        {
            var reserInfo = reserRepository.GetSingle(a => a.ReservationId == reser.ReservationId);
            var result = reserRepository.Update(a => new Reser()
            {
                IsDelete = reser.IsDelete,
                DataChgUsr = reser.DataChgUsr,
                DataChgDate = reser.DataChgDate
            }, a => a.ReservationId == reser.ReservationId);

            if (result)
            {
                roomRepository.Update(a => new Room { RoomStateId = (int)RoomState.Vacant }, a => a.RoomNumber.Equals(reserInfo.ReservationRoomNumber));
                return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Delete Reser Success", "预约信息删除成功"));
            }
            else
            {
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Reser Failed", "预约信息删除失败"));
            }
        }

        /// <summary>
        /// 更新预约信息
        /// </summary>
        /// <param name="reser"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateReserInfo(UpdateReserInputDto reser)
        {
            string NewTel = dataProtector.Protect(reser.ReservationPhoneNumber);
            reser.ReservationPhoneNumber = NewTel;
            try
            {
                var result = reserRepository.Update(EntityMapper.Map<UpdateReserInputDto, Reser>(reser));

                if (result)
                {
                    return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Update Customer Success", "预约信息更新成功"));
                }
                else
                {
                    return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Failed", "预约信息更新失败"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Update Customer Failed", "预约信息添加失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Failed", "预约信息更新失败"));
            }
        }

        /// <summary>
        /// 添加预约信息
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public BaseOutputDto InserReserInfo(CreateReserInputDto r)
        {
            string NewTel = dataProtector.Protect(r.ReservationPhoneNumber);
            r.ReservationPhoneNumber = NewTel;
            try
            {
                var result = reserRepository.Insert(EntityMapper.Map<CreateReserInputDto, Reser>(r));

                if (result)
                {
                    roomRepository.Update(a => new Room()
                    {
                        RoomStateId = new EnumHelper().GetEnumValue(RoomState.Reserved),
                        DataChgUsr = r.DataChgUsr,
                        DataChgDate = r.DataChgDate
                    }, a => a.RoomNumber == r.ReservationRoomNumber);
                    return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Add Customer Success", "预约信息添加成功"));
                }
                else
                {
                    return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Add Customer Failed", "预约信息添加失败"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Add Customer Failed", "预约信息添加失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Add Customer Failed", "预约信息添加失败"));
            }
        }
    }
}
