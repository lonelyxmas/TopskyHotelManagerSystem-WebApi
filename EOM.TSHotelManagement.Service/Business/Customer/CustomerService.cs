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
using jvncorelib.EntityLib;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace EOM.TSHotelManagement.Service
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
        /// 房间信息
        /// </summary>
        private readonly GenericRepository<Room> roomRepository;

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
        /// 角色仓储（用于客户组角色）
        /// </summary>
        private readonly GenericRepository<Role> roleRepository;

        /// <summary>
        /// 用户-角色映射仓储（用于绑定客户至客户组）
        /// </summary>
        private readonly GenericRepository<UserRole> userRoleRepository;

        /// <summary>
        /// 数据保护工具
        /// </summary>
        private readonly DataProtectionHelper dataProtector;

        private readonly ILogger<CustomerService> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custoRepository"></param>
        /// <param name="roomRepository"></param>
        /// <param name="spendRepository"></param>
        /// <param name="passPortTypeRepository"></param>
        /// <param name="custoTypeRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="userRoleRepository"></param>
        /// <param name="dataProtector"></param>
        /// <param name="logger"></param>
        public CustomerService(GenericRepository<Customer> custoRepository, GenericRepository<Room> roomRepository, GenericRepository<Spend> spendRepository, GenericRepository<PassportType> passPortTypeRepository, GenericRepository<CustoType> custoTypeRepository, GenericRepository<Role> roleRepository, GenericRepository<UserRole> userRoleRepository, DataProtectionHelper dataProtector, ILogger<CustomerService> logger)
        {
            this.custoRepository = custoRepository;
            this.roomRepository = roomRepository;
            this.spendRepository = spendRepository;
            this.passPortTypeRepository = passPortTypeRepository;
            this.custoTypeRepository = custoTypeRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.dataProtector = dataProtector;
            this.logger = logger;
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="custo"></param>
        public BaseResponse InsertCustomerInfo(CreateCustomerInputDto custo)
        {
            string NewID = dataProtector.EncryptCustomerData(custo.IdCardNumber);
            string NewTel = dataProtector.EncryptCustomerData(custo.CustomerPhoneNumber);
            custo.IdCardNumber = NewID;
            custo.CustomerPhoneNumber = NewTel;
            try
            {
                if (custoRepository.IsAny(a => a.CustomerNumber == custo.CustomerNumber))
                {
                    return new BaseResponse() { Message = LocalizationHelper.GetLocalizedString("customer number already exist.", "客户编号已存在"), Code = BusinessStatusCode.InternalServerError };
                }
                var customer = EntityMapper.Map<CreateCustomerInputDto, Customer>(custo);
                var result = custoRepository.Insert(customer);
                if (!result)
                {
                    logger.LogError(LocalizationHelper.GetLocalizedString("Insert Customer Failed", "客户信息添加失败"));
                    return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Insert Customer Failed", "客户信息添加失败"));
                }

                // 将客户加入“客户组”角色，便于与管理员一样进行权限配置
                const string customerRoleNumber = "R-CUSTOMER";

                // 确保客户组角色存在
                if (!roleRepository.AsQueryable().Any(r => r.RoleNumber == customerRoleNumber && r.IsDelete != 1))
                {
                    roleRepository.Insert(new Role
                    {
                        RoleNumber = customerRoleNumber,
                        RoleName = LocalizationHelper.GetLocalizedString("Customer Group", "客户组"),
                        RoleDescription = LocalizationHelper.GetLocalizedString("Unified permission group for customers", "客户统一权限组"),
                        IsDelete = 0,
                        DataInsUsr = customer.DataInsUsr,
                        DataInsDate = DateTime.Now
                    });
                }

                // 绑定客户到客户组角色
                if (!userRoleRepository.AsQueryable().Any(ur => ur.UserNumber == customer.CustomerNumber && ur.RoleNumber == customerRoleNumber && ur.IsDelete != 1))
                {
                    userRoleRepository.Insert(new UserRole
                    {
                        UserNumber = customer.CustomerNumber,
                        RoleNumber = customerRoleNumber,
                        IsDelete = 0,
                        DataInsUsr = customer.DataInsUsr,
                        DataInsDate = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inserting customer information for customer number {CustomerNumber}", custo.CustomerNumber);
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Insert Customer Failed", "客户信息添加失败"));
            }

            return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Insert Customer Success", "客户信息添加成功"));
        }

        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="custo"></param>
        /// <returns></returns>
        public BaseResponse UpdCustomerInfo(UpdateCustomerInputDto custo)
        {
            string NewID = dataProtector.EncryptCustomerData(custo.IdCardNumber);
            string NewTel = dataProtector.EncryptCustomerData(custo.CustomerPhoneNumber);
            custo.IdCardNumber = NewID;
            custo.CustomerPhoneNumber = NewTel;
            try
            {
                if (!custoRepository.IsAny(a => a.CustomerNumber == custo.CustomerNumber))
                {
                    return new BaseResponse() { Message = LocalizationHelper.GetLocalizedString("customer number does not exist.", "客户编号不存在"), Code = BusinessStatusCode.InternalServerError };
                }
                var customer = EntityMapper.Map<UpdateCustomerInputDto, Customer>(custo);
                var result = custoRepository.Update(customer);
                if (!result)
                {
                    return new BaseResponse() { Message = LocalizationHelper.GetLocalizedString("Update Customer Failed", "客户信息更新失败"), Code = BusinessStatusCode.InternalServerError };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating customer information for customer number {CustomerNumber}", custo.CustomerNumber);
                return new BaseResponse() { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="custo"></param>
        /// <returns></returns>
        public BaseResponse DelCustomerInfo(DeleteCustomerInputDto custo)
        {
            try
            {
                if (custo?.DelIds == null || !custo.DelIds.Any())
                {
                    return new BaseResponse
                    {
                        Code = BusinessStatusCode.BadRequest,
                        Message = LocalizationHelper.GetLocalizedString("Parameters Invalid", "参数错误")
                    };
                }

                var customers = custoRepository.GetList(a => custo.DelIds.Contains(a.Id));

                if (!customers.Any())
                {
                    return new BaseResponse
                    {
                        Code = BusinessStatusCode.NotFound,
                        Message = LocalizationHelper.GetLocalizedString("Customer Information Not Found", "客户信息未找到")
                    };
                }

                var occupied = Convert.ToInt32(RoomState.Occupied);
                foreach (var customer in customers)
                {
                    var isOccupied = roomRepository.IsAny(a => a.CustomerNumber == customer.CustomerNumber && a.RoomStateId == occupied);
                    var haveUnSettle = spendRepository.IsAny(a => a.CustomerNumber == customer.CustomerNumber && a.SettlementStatus == ConsumptionConstant.UnSettle.Code);

                    if (isOccupied)
                        return new BaseResponse(BusinessStatusCode.InternalServerError,
                            string.Format(LocalizationHelper.GetLocalizedString("Customer {0} is currently occupying a room", "客户{0}当前正在占用房间"), customer.CustomerNumber));

                    if (haveUnSettle)
                        return new BaseResponse(BusinessStatusCode.InternalServerError,
                            string.Format(LocalizationHelper.GetLocalizedString("Customer {0} has unsettled bills", "客户{0}有未结算的账单"), customer.CustomerNumber));
                }

                // 批量软删除
                custoRepository.SoftDeleteRange(customers);

                return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Delete Customer Success", "客户信息删除成功"));
            }
            catch (Exception)
            {
                logger.LogError("Error deleting customer information for customer IDs {CustomerIds}", string.Join(", ", custo.DelIds));
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Customer Failed", "客户信息删除失败"));
            }
        }

        /// <summary>
        /// 更新客户类型(即会员等级)
        /// </summary>
        /// <param name="updateCustomerInputDto"></param>
        /// <returns></returns>
        public BaseResponse UpdCustomerTypeByCustoNo(UpdateCustomerInputDto updateCustomerInputDto)
        {
            try
            {
                if (!custoRepository.IsAny(a => a.CustomerNumber == updateCustomerInputDto.CustomerNumber))
                {
                    return new BaseResponse() { Message = LocalizationHelper.GetLocalizedString("customer number does not exist.", "客户编号不存在"), Code = BusinessStatusCode.InternalServerError };
                }
                var result = custoRepository.Update(a => new Customer { CustomerType = updateCustomerInputDto.CustomerType }, a => a.CustomerNumber == updateCustomerInputDto.CustomerNumber);

                if (result)
                {
                    return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Update Customer Type Success", "客户类型更新成功"));
                }
                else
                {
                    return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Type Failed", "客户类型更新失败"));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating customer type for customer number {CustomerNumber}", updateCustomerInputDto.CustomerNumber);
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Update Customer Type Failed", "客户类型更新失败"));
            }
        }

        /// <summary>
        /// 查询所有客户信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadCustomerOutputDto> SelectCustomers(ReadCustomerInputDto readCustomerInputDto)
        {
            readCustomerInputDto ??= new ReadCustomerInputDto();

            var where = SqlFilterBuilder.BuildExpression<Customer, ReadCustomerInputDto>(readCustomerInputDto, nameof(Customer.DateOfBirth));
            var query = custoRepository.AsQueryable().Where(a => a.IsDelete != 1);
            var whereExpression = where.ToExpression();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            query = query.OrderBy(a => a.CustomerNumber);

            var count = 0;
            List<Customer> custos;
            if (!readCustomerInputDto.IgnorePaging)
            {
                var page = readCustomerInputDto.Page > 0 ? readCustomerInputDto.Page : 1;
                var pageSize = readCustomerInputDto.PageSize > 0 ? readCustomerInputDto.PageSize : 15;
                custos = query.ToPageList(page, pageSize, ref count);
            }
            else
            {
                custos = query.ToList();
                count = custos.Count;
            }

            var passPortTypeMap = passPortTypeRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.PassportId)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.PassportName ?? "");

            var custoTypeMap = custoTypeRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.CustomerType)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.CustomerTypeName ?? "");

            var helper = new EnumHelper();
            var genderMap = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToDictionary(x => x.Id, x => x.Description ?? "");

            List<ReadCustomerOutputDto> customerOutputDtos;
            var useParallelProjection = readCustomerInputDto.IgnorePaging && custos.Count >= 200;
            if (useParallelProjection)
            {
                var dtoArray = new ReadCustomerOutputDto[custos.Count];
                System.Threading.Tasks.Parallel.For(0, custos.Count, i =>
                {
                    var source = custos[i];
                    dtoArray[i] = new ReadCustomerOutputDto
                    {
                        Id = source.Id,
                        CustomerNumber = source.CustomerNumber,
                        CustomerName = source.CustomerName,
                        CustomerGender = source.CustomerGender,
                        PassportId = source.PassportId,
                        GenderName = genderMap.TryGetValue(source.CustomerGender ?? 0, out var genderName) ? genderName : "",
                        CustomerPhoneNumber = dataProtector.SafeDecryptCustomerData(source.CustomerPhoneNumber),
                        DateOfBirth = source.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                        CustomerType = source.CustomerType,
                    CustomerTypeName = custoTypeMap.TryGetValue(source.CustomerType, out var customerTypeName) ? customerTypeName : "",
                    PassportName = passPortTypeMap.TryGetValue(source.PassportId, out var passportName) ? passportName : "",
                    IdCardNumber = dataProtector.SafeDecryptCustomerData(source.IdCardNumber),
                    CustomerAddress = source.CustomerAddress ?? "",
                    DataInsUsr = source.DataInsUsr,
                    DataInsDate = source.DataInsDate,
                    DataChgUsr = source.DataChgUsr,
                    DataChgDate = source.DataChgDate,
                    IsDelete = source.IsDelete
                };
            });
                customerOutputDtos = dtoArray.ToList();
            }
            else
            {
                customerOutputDtos = new List<ReadCustomerOutputDto>(custos.Count);
                custos.ForEach(source =>
                {
                    customerOutputDtos.Add(new ReadCustomerOutputDto
                    {
                        Id = source.Id,
                        CustomerNumber = source.CustomerNumber,
                        CustomerName = source.CustomerName,
                        CustomerGender = source.CustomerGender,
                        PassportId = source.PassportId,
                        GenderName = genderMap.TryGetValue(source.CustomerGender ?? 0, out var genderName) ? genderName : "",
                        CustomerPhoneNumber = dataProtector.SafeDecryptCustomerData(source.CustomerPhoneNumber),
                        DateOfBirth = source.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                        CustomerType = source.CustomerType,
                        CustomerTypeName = custoTypeMap.TryGetValue(source.CustomerType, out var customerTypeName) ? customerTypeName : "",
                        PassportName = passPortTypeMap.TryGetValue(source.PassportId, out var passportName) ? passportName : "",
                        IdCardNumber = dataProtector.SafeDecryptCustomerData(source.IdCardNumber),
                        CustomerAddress = source.CustomerAddress ?? "",
                        DataInsUsr = source.DataInsUsr,
                        DataInsDate = source.DataInsDate,
                        DataChgUsr = source.DataChgUsr,
                        DataChgDate = source.DataChgDate,
                        IsDelete = source.IsDelete
                    });
                });
            }

            return new ListOutputDto<ReadCustomerOutputDto>
            {
                Data = new PagedData<ReadCustomerOutputDto>
                {
                    Items = customerOutputDtos,
                    TotalCount = count
                }
            };
        }

        /// <summary>
        /// 查询指定客户信息
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadCustomerOutputDto> SelectCustoByInfo(ReadCustomerInputDto custo)
        {
            //查询出所有性别类型
            var helper = new EnumHelper();
            var genderMap = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToDictionary(x => x.Id, x => x.Description ?? "");
            //查询出所有证件类型
            var passPortTypeMap = passPortTypeRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.PassportId)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.PassportName ?? "");
            //查询出所有客户类型
            var custoTypeMap = custoTypeRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.CustomerType)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.CustomerTypeName ?? "");
            //查询出所有客户信息
            SingleOutputDto<ReadCustomerOutputDto> singleOutputDto = new SingleOutputDto<ReadCustomerOutputDto>();

            var where = SqlFilterBuilder.BuildExpression<Customer, ReadCustomerInputDto>(custo);

            var customer = custoRepository.AsQueryable().Where(where.ToExpression()).Single();

            if (customer.IsNullOrEmpty())
            {
                return new SingleOutputDto<ReadCustomerOutputDto> { Code = BusinessStatusCode.InternalServerError, Message = "该用户不存在" };
            }

            //解密身份证号码/联系方式（失败时回退原值）
            customer.IdCardNumber = dataProtector.SafeDecryptCustomerData(customer.IdCardNumber);
            customer.CustomerPhoneNumber = dataProtector.SafeDecryptCustomerData(customer.CustomerPhoneNumber);
            //性别类型
            customer.GenderName = genderMap.TryGetValue((int)customer.CustomerGender!, out var genderName) ? genderName : "";
            //证件类型
            customer.PassportName = passPortTypeMap.TryGetValue(customer.PassportId, out var passportName) ? passportName : "";
            //客户类型
            customer.CustomerTypeName = custoTypeMap.TryGetValue(customer.CustomerType, out var customerTypeName) ? customerTypeName : "";

            singleOutputDto.Data = EntityMapper.Map<Customer, ReadCustomerOutputDto>(customer);

            return singleOutputDto;
        }

    }
}
