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
using Microsoft.AspNetCore.DataProtection;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 客户信息接口实现类
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// 客户信息
        /// </summary>
        private readonly GenericRepository<Customer> custoRepository;

        /// <summary>
        /// 消费情况
        /// </summary>
        private readonly GenericRepository<Spend> spendRepository;

        /// <summary>
        /// 证件类型
        /// </summary>
        private readonly GenericRepository<PassportType> passPortTypeRepository;

        /// <summary>
        /// 客户类型
        /// </summary>
        private readonly GenericRepository<CustoType> custoTypeRepository;

        /// <summary>
        /// 数据保护
        /// </summary>
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custoRepository"></param>
        /// <param name="spendRepository"></param>
        /// <param name="passPortTypeRepository"></param>
        /// <param name="custoTypeRepository"></param>
        /// <param name="encrypt"></param>
        /// <param name="dataProtectionProvider"></param>
        public CustomerService(GenericRepository<Customer> custoRepository, GenericRepository<Spend> spendRepository, GenericRepository<PassportType> passPortTypeRepository, GenericRepository<CustoType> custoTypeRepository, IDataProtectionProvider dataProtectionProvider)
        {
            this.custoRepository = custoRepository;
            this.spendRepository = spendRepository;
            this.passPortTypeRepository = passPortTypeRepository;
            this.custoTypeRepository = custoTypeRepository;
            this.dataProtector = dataProtectionProvider.CreateProtector("CustomerInfoProtector");
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="custo"></param>
        public BaseOutputDto InsertCustomerInfo(CreateCustomerInputDto custo)
        {
            string NewID = dataProtector.Protect(custo.IdCardNumber);
            string NewTel = dataProtector.Protect(custo.CustomerPhoneNumber);
            custo.IdCardNumber = NewID;
            custo.CustomerPhoneNumber = NewTel;
            try
            {
                if (custoRepository.IsAny(a => a.CustomerNumber == custo.CustomerNumber))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("customer number already exist.", "客户编号已存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                var result = custoRepository.Insert(EntityMapper.Map<CreateCustomerInputDto, Customer>(custo));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Insert Customer Failed", "客户信息添加失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Insert Customer Failed", "客户信息添加失败"));
            }

            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Insert Customer Success", "客户信息添加成功"));
        }

        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="custo"></param>
        /// <returns></returns>
        public BaseOutputDto UpdCustomerInfo(UpdateCustomerInputDto custo)
        {
            string NewID = dataProtector.Protect(custo.IdCardNumber);
            string NewTel = dataProtector.Protect(custo.CustomerPhoneNumber);
            custo.IdCardNumber = NewID;
            custo.CustomerPhoneNumber = NewTel;
            try
            {
                if (!custoRepository.IsAny(a => a.CustomerNumber == custo.CustomerNumber))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("customer number does not exist.", "客户编号不存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                var result = custoRepository.Update(EntityMapper.Map<UpdateCustomerInputDto, Customer>(custo));

                if (result)
                {
                    return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Update Customer Success", "客户信息更新成功"));
                }
                else
                {
                    return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Failed", "客户信息更新失败"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Update Customer Failed", "客户信息更新失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Failed", "客户信息更新失败"));
            }
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="custo"></param>
        /// <returns></returns>
        public BaseOutputDto DelCustomerInfo(DeleteCustomerInputDto custo)
        {
            if (!custoRepository.IsAny(a => a.CustomerNumber == custo.CustomerNumber))
            {
                return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("customer number does not exist.", "客户编号不存在"), StatusCode = StatusCodeConstants.InternalServerError };
            }
            var occupied = Convert.ToInt32(RoomState.Occupied);
            var isOccupied = custoRepository.Change<Room>().IsAny(a => a.CustomerNumber == custo.CustomerNumber && a.RoomStateId == occupied);
            var haveUnSettle = custoRepository.Change<Spend>().IsAny(a => a.CustomerNumber == custo.CustomerNumber && a.SettlementStatus == SpendConsts.UnSettle);
            if (isOccupied)
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Customer is currently occupying a room", "客户当前正在占用房间"));
            if (haveUnSettle)
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Customer has unsettled bills", "客户有未结算的账单"));

            var result = custoRepository.Update(a => new Customer()
            {
                IsDelete = custo.IsDelete,
                DataChgUsr = custo.DataChgUsr,
                DataChgDate = custo.DataChgDate
            }, a => a.CustomerNumber == custo.CustomerNumber);

            if (result)
            {
                return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Delete Customer Success", "客户信息删除成功"));
            }
            else
            {
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Customer Failed", "客户信息删除失败"));
            }
        }

        /// <summary>
        /// 更新客户类型(即会员等级)
        /// </summary>
        /// <param name="updateCustomerInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdCustomerTypeByCustoNo(UpdateCustomerInputDto updateCustomerInputDto)
        {
            try
            {
                if (!custoRepository.IsAny(a => a.CustomerNumber == updateCustomerInputDto.CustomerNumber))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("customer number does not exist.", "客户编号不存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                var result = custoRepository.Update(a=>new Customer { CustomerType = updateCustomerInputDto.CustomerType },a => a.CustomerNumber == updateCustomerInputDto.CustomerNumber);

                if (result)
                {
                    return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Update Customer Type Success", "客户类型更新成功"));
                }
                else
                {
                    return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Type Failed", "客户类型更新失败"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Update Customer Type Failed", "客户类型更新失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Type Failed", "客户类型更新失败"));
            }
        }

        /// <summary>
        /// 查询酒店盈利情况（用于报表）
        /// </summary>
        /// <returns></returns>
        public List<CustoSpend> SelectAllMoney()
        {
            List<CustoSpend> custoSpends = new List<CustoSpend>();
            var listSource = spendRepository.GetList(a => a.SettlementStatus.Equals(SpendConsts.Settled)).OrderBy(a => a.ConsumptionTime).ToList();
            var listDates = new List<CustoSpend>();
            listSource.ForEach(source =>
            {
                var year = Convert.ToDateTime(source.ConsumptionTime).ToString("yyyy");
                if (!custoSpends.Select(a => a.Years).ToList().Contains(year))
                {
                    var startDate = new DateTime(Convert.ToDateTime(source.ConsumptionTime).Year, 1, 1, 0, 0, 0);
                    var endDate = new DateTime(Convert.ToDateTime(source.ConsumptionTime).Year, 12, 31, 23, 59, 59);
                    custoSpends.Add(new CustoSpend
                    {
                        Years = year,
                        Money = listSource.Where(a => a.ConsumptionTime >= startDate && a.ConsumptionTime <= endDate).Sum(a => a.ConsumptionAmount)
                    });
                }
            });

            custoSpends = custoSpends.OrderBy(a => a.Years).ToList();
            return custoSpends;
        }

        /// <summary>
        /// 查询所有客户信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadCustomerOutputDto> SelectCustomers(ReadCustomerInputDto readCustomerInputDto)
        {
            ListOutputDto<ReadCustomerOutputDto> oSelectCustoAllDto = new ListOutputDto<ReadCustomerOutputDto>();

            //查询出所有性别类型
            var helper = new EnumHelper();
            var genders = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToList();
            //查询出所有证件类型
            List<PassportType> passPortTypes = new List<PassportType>();
            passPortTypes = passPortTypeRepository.GetList();
            //查询出所有客户类型
            List<CustoType> custoTypes = new List<CustoType>();
            custoTypes = custoTypeRepository.GetList();
            //查询出所有客户信息
            List<Customer> custos = new List<Customer>();

            var where = Expressionable.Create<Customer>();

            where = where.And(a => a.IsDelete == readCustomerInputDto.IsDelete);

            if (!readCustomerInputDto.CustomerNumber.IsNullOrEmpty())
            {
                where = where.And(a => a.CustomerNumber.Equals(readCustomerInputDto.CustomerNumber));
            }
            if (!readCustomerInputDto.CustomerName.IsNullOrEmpty())
            {
                where = where.And(a => a.CustomerName.Contains(readCustomerInputDto.CustomerName));
            }
            if (!readCustomerInputDto.CustomerPhoneNumber.IsNullOrEmpty())
            {
                where = where.And(a => a.CustomerPhoneNumber.Contains(readCustomerInputDto.CustomerPhoneNumber));
            }
            if (!readCustomerInputDto.IdCardNumber.IsNullOrEmpty())
            {
                where = where.And(a => a.IdCardNumber.Contains(readCustomerInputDto.IdCardNumber));
            }

            var count = 0;

            if (!readCustomerInputDto.IgnorePaging && readCustomerInputDto.Page != 0 && readCustomerInputDto.PageSize != 0)
            {
                custos = custoRepository.AsQueryable().Where(where.ToExpression()).OrderBy(a => a.CustomerNumber)
                    .ToPageList((int)readCustomerInputDto.Page, (int)readCustomerInputDto.PageSize, ref count);
            }
            else
            {
                custos = custoRepository.AsQueryable().Where(where.ToExpression()).OrderBy(a => a.CustomerNumber).ToList();
            }

            var customerOutputDtos = EntityMapper.MapList<Customer, ReadCustomerOutputDto>(custos);

            customerOutputDtos.ForEach(source =>
            {
                try
                {
                    //解密身份证号码
                    var sourceStr = dataProtector.Unprotect(source.IdCardNumber);
                    source.IdCardNumber = sourceStr;
                    //解密联系方式
                    var sourceTelStr = dataProtector.Unprotect(source.CustomerPhoneNumber);
                    source.CustomerPhoneNumber = sourceTelStr;
                }
                catch (Exception)
                {
                    source.IdCardNumber = source.IdCardNumber;
                    source.CustomerPhoneNumber = source.CustomerPhoneNumber;
                }

                //性别类型
                var sexType = genders.SingleOrDefault(a => a.Id == source.CustomerGender);
                source.GenderName = sexType?.Description ?? "";

                //证件类型
                var passPortType = passPortTypes.SingleOrDefault(a => a.PassportId == source.PassportId);
                source.PassportName = passPortType?.PassportName ?? "";

                //客户类型
                var custoType = custoTypes.SingleOrDefault(a => a.CustomerType == source.CustomerType);
                source.CustomerTypeName = custoType?.CustomerTypeName ?? "";
            });

            oSelectCustoAllDto.listSource = customerOutputDtos;
            oSelectCustoAllDto.total = count;

            return oSelectCustoAllDto;
        }

        /// <summary>
        /// 查询指定客户信息
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadCustomerOutputDto> SelectCustoByInfo(ReadCustomerInputDto custo)
        {
            //查询出所有性别类型
            var helper = new EnumHelper();
            var genders = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToList();
            //查询出所有证件类型
            List<PassportType> passPortTypes = new List<PassportType>();
            passPortTypes = passPortTypeRepository.GetList();
            //查询出所有客户类型
            List<CustoType> custoTypes = new List<CustoType>();
            custoTypes = custoTypeRepository.GetList();
            //查询出所有客户信息
            SingleOutputDto<ReadCustomerOutputDto> singleOutputDto = new SingleOutputDto<ReadCustomerOutputDto>();

            var where = Expressionable.Create<Customer>();

            if (!custo.CustomerNumber.IsNullOrEmpty())
            {
                where = where.And(a => a.CustomerNumber.Contains(custo.CustomerNumber));
            }
            if (!custo.CustomerName.IsNullOrEmpty())
            {
                where = where.And(a => a.CustomerName.Contains(custo.CustomerName));
            }

            var customer = custoRepository.AsQueryable().Where(where.ToExpression()).Single();

            if (customer.IsNullOrEmpty())
            {
                return new SingleOutputDto<ReadCustomerOutputDto> { StatusCode = StatusCodeConstants.InternalServerError, Message = "该用户不存在" };
            }

            try
            {
                //解密身份证号码
                var sourceStr = dataProtector.Unprotect(customer.IdCardNumber);
                customer.IdCardNumber = sourceStr;
                //解密联系方式
                var sourceTelStr = dataProtector.Unprotect(customer.CustomerPhoneNumber);
                customer.CustomerPhoneNumber = sourceTelStr;
            }
            catch (Exception)
            {
                customer.IdCardNumber = customer.IdCardNumber;
                customer.CustomerPhoneNumber = customer.CustomerPhoneNumber;
            }
            //性别类型
            var sexType = genders.SingleOrDefault(a => a.Id == customer.CustomerGender);
            customer.GenderName = sexType.Description.IsNullOrEmpty() ? "" : sexType.Description;
            //证件类型
            var passPortType = passPortTypes.SingleOrDefault(a => a.PassportId == customer.PassportId);
            customer.PassportName = passPortType.PassportName.IsNullOrEmpty() ? "" : passPortType.PassportName;
            //客户类型
            var custoType = custoTypes.SingleOrDefault(a => a.CustomerType == customer.CustomerType);
            customer.CustomerTypeName = custoType.CustomerTypeName.IsNullOrEmpty() ? "" : custoType.CustomerTypeName;

            singleOutputDto.Source = EntityMapper.Map<Customer, ReadCustomerOutputDto>(customer);

            return singleOutputDto;
        }

    }
}
