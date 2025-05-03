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
using jvncorelib.EntityLib;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 商品消费接口实现类
    /// </summary>
    public class SpendService : ISpendService
    {
        /// <summary>
        /// 商品消费
        /// </summary>
        private readonly GenericRepository<Spend> spendRepository;

        /// <summary>
        /// 商品
        /// </summary>
        private readonly GenericRepository<SellThing> sellThingRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spendRepository"></param>
        /// <param name="sellThingRepository"></param>
        public SpendService(GenericRepository<Spend> spendRepository, GenericRepository<SellThing> sellThingRepository)
        {
            this.spendRepository = spendRepository;
            this.sellThingRepository = sellThingRepository;
        }

        #region 添加消费信息
        /// <summary>
        /// 添加消费信息
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public BaseOutputDto InsertSpendInfo(CreateSpendInputDto s)
        {
            try
            {
                spendRepository.Insert(EntityMapper.Map<CreateSpendInputDto, Spend>(s));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
        #endregion

        #region 根据客户编号查询消费信息
        /// <summary>
        /// 根据客户编号查询消费信息
        /// </summary>
        /// <param name="readSpendInputDto"></param>
        /// <returns></returns>
        public ListOutputDto<ReadSpendOutputDto> SelectSpendByCustoNo(ReadSpendInputDto readSpendInputDto)
        {
            List<Spend> ls = new List<Spend>();
            ls = spendRepository.GetList(a => a.CustomerNumber == readSpendInputDto.CustomerNumber && a.SettlementStatus.Equals(SpendConsts.UnSettle) && a.IsDelete != 1);
            ls.ForEach(source =>
            {
                source.SettlementStatusDescription = source.SettlementStatus.IsNullOrEmpty() ? ""
                : source.SettlementStatus.Equals(SpendConsts.Settled) ? "已结算" : "未结算";

                source.ProductPriceFormatted = (source.ProductPrice + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ProductPrice.ToString()).ToString("#,##0.00").ToString();

                source.ConsumptionAmountFormatted = (source.ConsumptionAmount + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ConsumptionAmount.ToString()).ToString("#,##0.00").ToString();
            });

            var listSource = EntityMapper.MapList<Spend, ReadSpendOutputDto>(ls);

            return new ListOutputDto<ReadSpendOutputDto> { listSource = listSource };
        }
        #endregion

        #region 根据客户编号查询历史消费信息
        /// <summary>
        /// 根据客户编号查询历史消费信息
        /// </summary>
        /// <param name="readSpendInputDto"></param>
        /// <returns></returns>
        public ListOutputDto<ReadSpendOutputDto> SeletHistorySpendInfoAll(ReadSpendInputDto readSpendInputDto)
        {
            List<Spend> ls = new List<Spend>();
            ls = spendRepository.GetList(a => a.CustomerNumber == readSpendInputDto.CustomerNumber && a.SettlementStatus.Equals(SpendConsts.Settled) && a.IsDelete != 1);
            ls.ForEach(source =>
            {
                source.SettlementStatusDescription = source.SettlementStatus.IsNullOrEmpty() ? ""
                : source.SettlementStatus.Equals(SpendConsts.Settled) ? "已结算" : "未结算";

                source.ProductPriceFormatted = (source.ProductPrice + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ProductPrice.ToString()).ToString("#,##0.00").ToString();

                source.ConsumptionAmountFormatted = (source.ConsumptionAmount + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ConsumptionAmount.ToString()).ToString("#,##0.00").ToString();
            });

            var listSource = EntityMapper.MapList<Spend, ReadSpendOutputDto>(ls);

            return new ListOutputDto<ReadSpendOutputDto> { listSource = listSource };
        }
        #endregion

        #region 根据房间编号查询消费信息
        /// <summary>
        /// 根据房间编号查询消费信息
        /// </summary>
        /// <param name="readSpendInputDto"></param>
        /// <returns></returns>
        public ListOutputDto<ReadSpendOutputDto> SelectSpendByRoomNo(ReadSpendInputDto readSpendInputDto)
        {
            List<Spend> ls = new List<Spend>();
            ls = spendRepository.GetList(a => a.RoomNumber == readSpendInputDto.RoomNumber && a.SettlementStatus.Equals(SpendConsts.UnSettle) && a.IsDelete != 1);
            var spendNames = ls.Select(a => a.ProductName).ToList();
            var spendInfos = sellThingRepository.AsQueryable().Where(a => spendNames.Contains(a.ProductName)).ToList();
            ls.ForEach(source =>
            {
                if (source.ProductNumber.IsNullOrEmpty())
                {
                    var spendInfo = spendInfos.SingleOrDefault(a => a.ProductName.Equals(source.ProductName));
                    if (spendInfo != null)
                    {
                        source.ProductNumber = spendInfo.ProductNumber;
                    }
                }

                source.SettlementStatusDescription = source.SettlementStatus.IsNullOrEmpty() ? ""
                : source.SettlementStatus.Equals(SpendConsts.Settled) ? "已结算" : "未结算";

                source.ProductPriceFormatted = (source.ProductPrice + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ProductPrice.ToString()).ToString("#,##0.00").ToString();

                source.ConsumptionAmountFormatted = (source.ConsumptionAmount + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ConsumptionAmount.ToString()).ToString("#,##0.00").ToString();
            });

            var listSource = EntityMapper.MapList<Spend, ReadSpendOutputDto>(ls);

            return new ListOutputDto<ReadSpendOutputDto> { listSource = listSource };
        }
        #endregion

        #region 查询消费的所有信息
        /// <summary>
        /// 查询消费的所有信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadSpendOutputDto> SelectSpendInfoAll(ReadSpendInputDto readSpendInputDto)
        {
            var where = Expressionable.Create<Spend>();

            if (!readSpendInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readSpendInputDto.IsDelete);
            }

            List<Spend> ls = new List<Spend>();

            var count = 0;

            if (!readSpendInputDto.IgnorePaging && readSpendInputDto.Page != 0 && readSpendInputDto.PageSize != 0)
            {
                ls = spendRepository.AsQueryable().Where(where.ToExpression()).OrderByDescending(a => a.ConsumptionTime).ToPageList(readSpendInputDto.Page, readSpendInputDto.PageSize, ref count);
            }
            else
            {
                ls = spendRepository.AsQueryable().Where(where.ToExpression()).OrderByDescending(a => a.ConsumptionTime).ToList();
            }

            ls.ForEach(source =>
            {
                source.SettlementStatusDescription = source.SettlementStatus.IsNullOrEmpty() ? ""
                : source.SettlementStatus.Equals(SpendConsts.Settled) ? "已结算" : "未结算";

                source.ProductPriceFormatted = (source.ProductPrice + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ProductPrice.ToString()).ToString("#,##0.00").ToString();

                source.ConsumptionAmountFormatted = (source.ConsumptionAmount + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ConsumptionAmount.ToString()).ToString("#,##0.00").ToString();
            });

            var listSource = EntityMapper.MapList<Spend, ReadSpendOutputDto>(ls);

            return new ListOutputDto<ReadSpendOutputDto> { listSource = listSource, total = count };
        }
        #endregion

        #region 根据房间号查询消费的所有信息
        /// <summary>
        /// 根据房间号查询消费的所有信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadSpendOutputDto> SelectSpendInfoRoomNo(ReadSpendInputDto readSpendInputDto)
        {
            List<Spend> ls = new List<Spend>();
            ls = spendRepository.GetList(a => a.RoomNumber == readSpendInputDto.RoomNumber && a.IsDelete != 1 && a.SettlementStatus.Equals(SpendConsts.UnSettle));
            var spendNames = ls.Select(a => a.ProductName).ToList();
            var spendInfos = sellThingRepository.AsQueryable().Where(a => spendNames.Contains(a.ProductName)).ToList();
            ls.ForEach(source =>
            {
                if (source.ProductNumber.IsNullOrEmpty())
                {
                    var spendInfo = spendInfos.SingleOrDefault(a => a.ProductName.Equals(source.ProductName));
                    if (spendInfo != null)
                    {
                        source.ProductNumber = spendInfo.ProductNumber;
                    }
                }

                source.SettlementStatusDescription = source.SettlementStatus.IsNullOrEmpty() ? ""
                : source.SettlementStatus.Equals(SpendConsts.Settled) ? "已结算" : "未结算";

                source.ProductPriceFormatted = (source.ProductPrice + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ProductPrice.ToString()).ToString("#,##0.00").ToString();

                source.ConsumptionAmountFormatted = (source.ConsumptionAmount + "").IsNullOrEmpty() ? ""
                : Decimal.Parse(source.ConsumptionAmount.ToString()).ToString("#,##0.00").ToString();
            });

            var listSource = EntityMapper.MapList<Spend, ReadSpendOutputDto>(ls);

            return new ListOutputDto<ReadSpendOutputDto> { listSource = listSource };
        }
        #endregion

        #region 根据房间编号、入住时间到当前时间查询消费总金额
        /// <summary>
        /// 根据房间编号、入住时间到当前时间查询消费总金额
        /// </summary>
        /// <param name="readSpendInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadSpendInputDto> SumConsumptionAmount(ReadSpendInputDto readSpendInputDto)
        {
            var totalAmount = spendRepository.GetList(a => a.RoomNumber == readSpendInputDto.RoomNumber && a.CustomerNumber == readSpendInputDto.CustomerNumber && a.SettlementStatus.Equals(SpendConsts.UnSettle)).Sum(a => a.ConsumptionAmount);
            return new SingleOutputDto<ReadSpendInputDto> { Source = new ReadSpendInputDto { ConsumptionAmount = totalAmount } };
        }
        #endregion

        /// <summary>
        /// 撤回客户消费信息
        /// </summary>
        /// <param name="updateSpendInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UndoCustomerSpend(UpdateSpendInputDto updateSpendInputDto)
        {
            try
            {
                spendRepository.Update(a => new Spend()
                {
                    IsDelete = 1,
                    DataChgDate = updateSpendInputDto.DataChgDate,
                    DataChgUsr = updateSpendInputDto.DataChgUsr
                }, a => a.SpendNumber.Equals(updateSpendInputDto.SpendNumber));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新消费信息
        /// </summary>
        /// <param name="spend"></param>
        /// <returns></returns>
        public BaseOutputDto UpdSpenInfo(UpdateSpendInputDto spend)
        {
            try
            {
                spendRepository.Update(a => new Spend()
                {
                    ConsumptionQuantity = spend.ConsumptionQuantity,
                    ConsumptionAmount = spend.ConsumptionAmount,
                    DataChgDate = spend.DataChgDate,
                    DataChgUsr = spend.DataChgUsr
                }, a => a.SettlementStatus.Equals(SpendConsts.UnSettle)
                && a.RoomNumber.Equals(spend.RoomNumber)
                && a.CustomerNumber.Equals(spend.CustomerNumber)
                && a.ProductName.Equals(spend.ProductName));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

    }
}
