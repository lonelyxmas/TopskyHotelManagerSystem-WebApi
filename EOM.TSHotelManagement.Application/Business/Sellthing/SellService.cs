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
    /// 商品信息接口实现类
    /// </summary>
    public class SellService : ISellService
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        private readonly GenericRepository<SellThing> sellThingRepository;

        /// <summary>
        /// 消费情况
        /// </summary>
        private readonly GenericRepository<Spend> spendRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sellThingRepository"></param>
        /// <param name="spendRepository"></param>
        public SellService(GenericRepository<SellThing> sellThingRepository, GenericRepository<Spend> spendRepository)
        {
            this.sellThingRepository = sellThingRepository;
            this.spendRepository = spendRepository;
        }

        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadSellThingOutputDto> SelectSellThingAll(ReadSellThingInputDto sellThing)
        {
            List<SellThing> sellThings = new List<SellThing>();
            var exp = Expressionable.Create<SellThing>();

            exp = exp.And(a => a.Stock > 0 && a.IsDelete == sellThing.IsDelete);

            //商品编号
            if (!sellThing.ProductNumber.IsNullOrEmpty())
            {
                exp = exp.And(a => a.ProductNumber.Contains(sellThing.ProductNumber));
            }

            //商品名称
            if (!sellThing.ProductName.IsNullOrEmpty())
            {
                exp = exp.Or(a => a.ProductName.Contains(sellThing.ProductName));
            }

            //商品规格
            if (!sellThing.Specification.IsNullOrEmpty())
            {
                exp = exp.And(a => a.Specification.Contains(sellThing.Specification));
            }

            var count = 0;

            if (!sellThing.IgnorePaging && sellThing.Page != 0 && sellThing.PageSize != 0)
            {
                sellThings = sellThingRepository.AsQueryable().Where(exp.ToExpression()).ToPageList(sellThing.Page, sellThing.PageSize, ref count);
            }
            else
            {
                sellThings = sellThingRepository.GetList(exp.ToExpression());
            }

            sellThings.ForEach(_sellThing =>
            {
                _sellThing.ProductPriceFormatted = Decimal.Parse(_sellThing.ProductPrice.ToString()).ToString("#,##0.00").ToString();
            });

            var listSource = EntityMapper.MapList<SellThing, ReadSellThingOutputDto>(sellThings);

            return new ListOutputDto<ReadSellThingOutputDto>
            {
                listSource = listSource,
                total = count
            };
        }

        /// <summary>
        /// 查询所有商品(包括库存为0/已删除)
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadSellThingOutputDto> GetSellThings(ReadSellThingInputDto sellThing)
        {
            List<SellThing> sellThings = new List<SellThing>();
            var exp = Expressionable.Create<SellThing>();
            if (sellThing.IsNullOrEmpty())
            {
                sellThings = sellThingRepository.GetList(exp.ToExpression());
                sellThings.ForEach(_sellThing =>
                {
                    _sellThing.ProductPriceFormatted = Decimal.Parse(_sellThing.ProductPrice.ToString()).ToString("#,##0.00").ToString();
                });
            }
            else
            {
                //商品编号
                if (!sellThing.ProductNumber.IsNullOrEmpty())
                {
                    exp = exp.And(a => a.ProductNumber.Contains(sellThing.ProductNumber));
                }
                //商品名称
                if (!sellThing.ProductName.IsNullOrEmpty())
                {
                    exp = exp.Or(a => a.ProductName.Contains(sellThing.ProductName));
                }
                sellThings = sellThingRepository.GetList(exp.ToExpression());
                sellThings.ForEach(_sellThing =>
                {
                    _sellThing.ProductPriceFormatted = Decimal.Parse(_sellThing.ProductPrice.ToString()).ToString("#,##0.00").ToString();
                });
            }

            var listSource = EntityMapper.MapList<SellThing, ReadSellThingOutputDto>(sellThings);

            return new ListOutputDto<ReadSellThingOutputDto>
            {
                listSource = listSource,
                total = sellThings.Count
            };
        }

        /// <summary>
        /// 更新商品数量
        /// </summary>
        /// <param name="updateSellThingInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateSellThing(UpdateSellThingInputDto updateSellThingInputDto)
        {
            try
            {
                sellThingRepository.Update(a => new SellThing()
                {
                    Stock = Convert.ToInt32(updateSellThingInputDto.Stock),
                    DataChgDate = DateTime.Now
                }, a => a.ProductNumber == updateSellThingInputDto.ProductNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="sellThing"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateSellthingInfo(UpdateSellThingInputDto sellThing)
        {
            try
            {
                sellThingRepository.Update(a => new SellThing()
                {
                    ProductName = sellThing.ProductName,
                    ProductPrice = sellThing.ProductPrice,
                    Stock = sellThing.Stock,
                    Specification = sellThing.Specification,
                }, a => a.ProductNumber == sellThing.ProductNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <param name="deleteSellThingInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteSellthing(DeleteSellThingInputDto deleteSellThingInputDto)
        {
            try
            {
                sellThingRepository.Update(a => new SellThing()
                {
                    IsDelete = 1,
                }, a => a.ProductNumber == deleteSellThingInputDto.ProductNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 根据商品编号删除商品信息
        /// </summary>
        /// <param name="deleteSellThingInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteSellThingBySellNo(DeleteSellThingInputDto deleteSellThingInputDto)
        {
            try
            {
                sellThingRepository.Update(a => new SellThing()
                {
                    IsDelete = 1
                }, a => a.ProductNumber == deleteSellThingInputDto.ProductNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 根据商品名称和价格查询商品编号
        /// </summary>
        /// <param name="readSellThingInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadSellThingOutputDto> SelectSellThingByNameAndPrice(ReadSellThingInputDto readSellThingInputDto)
        {
            SellThing sellThing = null;
            sellThing = sellThingRepository.GetSingle(a => a.ProductNumber == readSellThingInputDto.ProductNumber || (a.ProductName == readSellThingInputDto.ProductName
            && a.ProductPrice == Convert.ToDecimal(readSellThingInputDto.ProductPrice)));

            var source = EntityMapper.Map<SellThing, ReadSellThingOutputDto>(sellThing);

            return new SingleOutputDto<ReadSellThingOutputDto>() { Source = source };
        }


        /// <summary>
        /// 根据商品编号查询商品信息
        /// </summary>
        /// <param name="readSellThingInputDto"></param>
        /// <returns></returns>
        public ReadSellThingOutputDto SelectSellInfoBySellNo(ReadSellThingInputDto readSellThingInputDto)
        {
            SellThing st = null;
            st = sellThingRepository.GetSingle(a => a.ProductNumber == readSellThingInputDto.ProductNumber && a.IsDelete != 1);

            var source = EntityMapper.Map<SellThing, ReadSellThingOutputDto>(st);

            return source;
        }

        #region 添加商品
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public BaseOutputDto InsertSellThing(CreateSellThingInputDto st)
        {
            try
            {
                sellThingRepository.Insert(EntityMapper.Map<CreateSellThingInputDto, SellThing>(st));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
        #endregion
    }
}
