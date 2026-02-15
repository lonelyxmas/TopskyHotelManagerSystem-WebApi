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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Transactions;

namespace EOM.TSHotelManagement.Service
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
        /// 房间
        /// </summary>

        private readonly GenericRepository<Room> roomRepository;

        /// <summary>
        /// 客户
        /// </summary>

        private readonly GenericRepository<Customer> customerRepository;

        /// <summary>
        /// 客户类型
        /// </summary>

        private readonly GenericRepository<CustoType> custoTypeRepository;

        /// <summary>
        /// 操作日志
        /// </summary>

        private readonly GenericRepository<OperationLog> operationLogRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<SpendService> logger;

        public SpendService(GenericRepository<Spend> spendRepository, GenericRepository<SellThing> sellThingRepository, GenericRepository<Room> roomRepository, GenericRepository<Customer> customerRepository, GenericRepository<CustoType> custoTypeRepository, GenericRepository<OperationLog> operationLogRepository, IHttpContextAccessor httpContextAccessor, ILogger<SpendService> logger)
        {
            this.spendRepository = spendRepository;
            this.sellThingRepository = sellThingRepository;
            this.roomRepository = roomRepository;
            this.customerRepository = customerRepository;
            this.custoTypeRepository = custoTypeRepository;
            this.operationLogRepository = operationLogRepository;
            _httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        #region 根据客户编号查询历史消费信息
        /// <summary>
        /// 根据客户编号查询历史消费信息
        /// </summary>
        /// <param name="readSpendInputDto"></param>
        /// <returns></returns>
        public ListOutputDto<ReadSpendOutputDto> SeletHistorySpendInfoAll(ReadSpendInputDto readSpendInputDto)
        {
            readSpendInputDto ??= new ReadSpendInputDto();

            var where = SqlFilterBuilder.BuildExpression<Spend, ReadSpendInputDto>(readSpendInputDto);
            var query = spendRepository.AsQueryable().Where(a => a.IsDelete != 1);
            var whereExpression = where.ToExpression();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            var count = 0;
            List<Spend> spends;
            if (!readSpendInputDto.IgnorePaging)
            {
                var page = readSpendInputDto.Page > 0 ? readSpendInputDto.Page : 1;
                var pageSize = readSpendInputDto.PageSize > 0 ? readSpendInputDto.PageSize : 15;
                spends = query.ToPageList(page, pageSize, ref count);
            }
            else
            {
                spends = query.ToList();
                count = spends.Count;
            }
            var result = EntityMapper.MapList<Spend, ReadSpendOutputDto>(spends);
            var useParallelProjection = readSpendInputDto.IgnorePaging && result.Count >= 200;
            if (useParallelProjection)
            {
                System.Threading.Tasks.Parallel.For(0, result.Count, i => FillSpendDerivedFields(result[i]));
            }
            else
            {
                result.ForEach(FillSpendDerivedFields);
            }

            return new ListOutputDto<ReadSpendOutputDto>
            {
                Data = new PagedData<ReadSpendOutputDto>
                {
                    Items = result,
                    TotalCount = count
                }
            };
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
            readSpendInputDto ??= new ReadSpendInputDto();

            var where = SqlFilterBuilder.BuildExpression<Spend, ReadSpendInputDto>(readSpendInputDto);
            var query = spendRepository.AsQueryable().Where(a => a.IsDelete != 1);
            var whereExpression = where.ToExpression();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            var count = 0;
            List<Spend> spends;
            if (!readSpendInputDto.IgnorePaging)
            {
                var page = readSpendInputDto.Page > 0 ? readSpendInputDto.Page : 1;
                var pageSize = readSpendInputDto.PageSize > 0 ? readSpendInputDto.PageSize : 15;
                spends = query.ToPageList(page, pageSize, ref count);
            }
            else
            {
                spends = query.ToList();
                count = spends.Count;
            }
            var result = EntityMapper.MapList<Spend, ReadSpendOutputDto>(spends);
            var useParallelProjection = readSpendInputDto.IgnorePaging && result.Count >= 200;
            if (useParallelProjection)
            {
                System.Threading.Tasks.Parallel.For(0, result.Count, i => FillSpendDerivedFields(result[i]));
            }
            else
            {
                result.ForEach(FillSpendDerivedFields);
            }

            return new ListOutputDto<ReadSpendOutputDto>
            {
                Data = new PagedData<ReadSpendOutputDto>
                {
                    Items = result,
                    TotalCount = count
                }
            };
        }
        #endregion

        #region 查询消费的所有信息
        /// <summary>
        /// 查询消费的所有信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadSpendOutputDto> SelectSpendInfoAll(ReadSpendInputDto readSpendInputDto)
        {
            readSpendInputDto ??= new ReadSpendInputDto();

            var where = SqlFilterBuilder.BuildExpression<Spend, ReadSpendInputDto>(readSpendInputDto, nameof(Spend.ConsumptionTime));
            var query = spendRepository.AsQueryable().Where(a => a.IsDelete != 1);
            var whereExpression = where.ToExpression();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            var count = 0;
            List<Spend> spends;
            if (!readSpendInputDto.IgnorePaging)
            {
                var page = readSpendInputDto.Page > 0 ? readSpendInputDto.Page : 1;
                var pageSize = readSpendInputDto.PageSize > 0 ? readSpendInputDto.PageSize : 15;
                spends = query.ToPageList(page, pageSize, ref count);
            }
            else
            {
                spends = query.ToList();
                count = spends.Count;
            }
            var result = EntityMapper.MapList<Spend, ReadSpendOutputDto>(spends);
            var useParallelProjection = readSpendInputDto.IgnorePaging && result.Count >= 200;
            if (useParallelProjection)
            {
                System.Threading.Tasks.Parallel.For(0, result.Count, i => FillSpendDerivedFields(result[i]));
            }
            else
            {
                result.ForEach(FillSpendDerivedFields);
            }

            return new ListOutputDto<ReadSpendOutputDto>
            {
                Data = new PagedData<ReadSpendOutputDto>
                {
                    Items = result,
                    TotalCount = count
                }
            };
        }
        #endregion

        private static void FillSpendDerivedFields(ReadSpendOutputDto item)
        {
            item.SettlementStatusDescription = item.SettlementStatus.IsNullOrEmpty() ? ""
                : item.SettlementStatus.Equals(ConsumptionConstant.Settled.Code) ? "已结算" : "未结算";

            item.ProductPriceFormatted = item.ProductPrice.ToString("#,##0.00");
            item.ConsumptionAmountFormatted = item.ConsumptionAmount.ToString("#,##0.00");

            item.ConsumptionTypeDescription = item.ConsumptionType == SpendTypeConstant.Product.Code ? SpendTypeConstant.Product.Description
                : item.ConsumptionType == SpendTypeConstant.Room.Code ? SpendTypeConstant.Room.Description
                : SpendTypeConstant.Other.Description;
        }

        #region 根据房间编号、入住时间到当前时间查询消费总金额
        /// <summary>
        /// 根据房间编号、入住时间到当前时间查询消费总金额
        /// </summary>
        /// <param name="readSpendInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadSpendInputDto> SumConsumptionAmount(ReadSpendInputDto readSpendInputDto)
        {
            var TotalCountAmount = spendRepository.GetList(a => a.RoomNumber == readSpendInputDto.RoomNumber && a.CustomerNumber == readSpendInputDto.CustomerNumber && a.SettlementStatus == ConsumptionConstant.UnSettle.Code).Sum(a => a.ConsumptionAmount);
            return new SingleOutputDto<ReadSpendInputDto> { Data = new ReadSpendInputDto { ConsumptionAmount = TotalCountAmount } };
        }
        #endregion

        /// <summary>
        /// 撤回客户消费信息
        /// </summary>
        /// <param name="updateSpendInputDto"></param>
        /// <returns></returns>
        public BaseResponse UndoCustomerSpend(UpdateSpendInputDto updateSpendInputDto)
        {
            try
            {
                var existingSpend = spendRepository.GetFirst(a => a.SpendNumber == updateSpendInputDto.SpendNumber && a.IsDelete != 1);
                existingSpend.IsDelete = 1;
                spendRepository.Update(existingSpend);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "撤回客户消费信息失败");
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary>
        /// 添加客户消费信息
        /// </summary>
        /// <param name="addCustomerSpendInputDto"></param>
        /// <returns></returns>
        public BaseResponse AddCustomerSpend(AddCustomerSpendInputDto addCustomerSpendInputDto)
        {
            if (addCustomerSpendInputDto?.Quantity <= 0 || addCustomerSpendInputDto.Price <= 0)
            {
                return new BaseResponse() { Message = "商品数量和价格必须大于零", Code = BusinessStatusCode.BadRequest };
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var room = roomRepository.AsQueryable().Single(a => a.RoomNumber == addCustomerSpendInputDto.RoomNumber);
                if (room == null)
                {
                    return new BaseResponse() { Message = $"房间 '{addCustomerSpendInputDto.RoomNumber}' 不存在", Code = BusinessStatusCode.BadRequest };
                }

                var customer = customerRepository.AsQueryable().Single(a => a.CustomerNumber == room.CustomerNumber);
                if (customer == null)
                {
                    return new BaseResponse() { Message = $"客户 '{room.CustomerNumber}' 不存在", Code = BusinessStatusCode.BadRequest };
                }

                var customerType = custoTypeRepository.AsQueryable().Single(a => a.CustomerType == customer.CustomerType);
                decimal discount = (customerType != null && customerType.Discount > 0 && customerType.Discount < 100)
                            ? customerType.Discount / 100M
                            : 1M;

                decimal realAmount = addCustomerSpendInputDto.Price * addCustomerSpendInputDto.Quantity * discount;

                var existingSpend = spendRepository.AsQueryable().Single(a => a.RoomNumber == addCustomerSpendInputDto.RoomNumber && a.ProductNumber == addCustomerSpendInputDto.ProductNumber && a.IsDelete != 1 && a.SettlementStatus == ConsumptionConstant.UnSettle.Code);

                if (existingSpend != null)
                {
                    existingSpend.ConsumptionType = SpendTypeConstant.Product.Code;
                    existingSpend.ConsumptionQuantity += addCustomerSpendInputDto.Quantity;
                    existingSpend.ConsumptionAmount += realAmount;
                    existingSpend.DataChgDate = DateTime.Now;
                    existingSpend.DataChgUsr = addCustomerSpendInputDto.WorkerNo;

                    var result = spendRepository.Update(existingSpend);
                    if (!result)
                    {
                        return new BaseResponse() { Message = "更新消费记录失败", Code = BusinessStatusCode.InternalServerError };
                    }
                }
                else
                {
                    var newSpend = new Spend
                    {
                        SpendNumber = new UniqueCode().GetNewId("SP-"),
                        RoomNumber = addCustomerSpendInputDto.RoomNumber,
                        ProductNumber = addCustomerSpendInputDto.ProductNumber,
                        ProductName = addCustomerSpendInputDto.ProductName,
                        ConsumptionQuantity = addCustomerSpendInputDto.Quantity,
                        CustomerNumber = room.CustomerNumber,
                        ProductPrice = addCustomerSpendInputDto.Price,
                        ConsumptionAmount = realAmount,
                        ConsumptionTime = DateTime.Now,
                        ConsumptionType = SpendTypeConstant.Product.Code,
                        SettlementStatus = ConsumptionConstant.UnSettle.Code,
                        DataInsUsr = addCustomerSpendInputDto.WorkerNo,
                        DataInsDate = DateTime.Now
                    };

                    var result = spendRepository.Insert(newSpend);
                    if (!result)
                    {
                        return new BaseResponse() { Message = "添加消费记录失败", Code = BusinessStatusCode.InternalServerError };
                    }
                }

                var product = sellThingRepository.AsQueryable().Single(a => a.ProductNumber == addCustomerSpendInputDto.ProductNumber);
                product.Stock = product.Stock - addCustomerSpendInputDto.Quantity;
                var updateResult = sellThingRepository.Update(product);
                if (!updateResult)
                {
                    return new BaseResponse() { Message = "商品库存更新失败", Code = BusinessStatusCode.InternalServerError };
                }

                var logContent = $"{addCustomerSpendInputDto.WorkerNo} 添加了消费记录: " +
                                 $"房间 {addCustomerSpendInputDto.RoomNumber}, " +
                                 $"商品 {addCustomerSpendInputDto.ProductName}, " +
                                 $"数量 {addCustomerSpendInputDto.Quantity}, " +
                                 $"金额 {realAmount.ToString("#,##0.00")}";

                var context = _httpContextAccessor.HttpContext;

                var log = new OperationLog
                {
                    OperationId = new UniqueCode().GetNewId("OP-"),
                    OperationTime = Convert.ToDateTime(DateTime.Now),
                    LogContent = logContent,
                    LoginIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                    OperationAccount = addCustomerSpendInputDto.WorkerNo,
                    LogLevel = (int)Common.LogLevel.Warning,
                    SoftwareVersion = addCustomerSpendInputDto.SoftwareVersion,
                    IsDelete = 0,
                    DataInsUsr = addCustomerSpendInputDto.WorkerNo,
                    DataInsDate = Convert.ToDateTime(DateTime.Now)
                };
                operationLogRepository.Insert(log);

                scope.Complete();

                return new BaseResponse();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "添加客户消费信息失败");
                return new BaseResponse() { Message = $"添加消费记录失败，请稍后重试。{ex.Message}", Code = BusinessStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// 更新消费信息
        /// </summary>
        /// <param name="spend"></param>
        /// <returns></returns>
        public BaseResponse UpdSpendInfo(UpdateSpendInputDto spend)
        {
            try
            {
                var dbSpend = spendRepository.GetFirst(a => a.SpendNumber == spend.SpendNumber && a.IsDelete != 1);
                dbSpend.SettlementStatus = spend.SettlementStatus;
                dbSpend.RoomNumber = spend.RoomNumber;
                dbSpend.CustomerNumber = spend.CustomerNumber;
                dbSpend.ProductName = spend.ProductName;
                dbSpend.ConsumptionQuantity = spend.ConsumptionQuantity;
                dbSpend.ConsumptionAmount = spend.ConsumptionAmount;
                spendRepository.Update(dbSpend);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "更新消费信息失败");
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

    }
}
