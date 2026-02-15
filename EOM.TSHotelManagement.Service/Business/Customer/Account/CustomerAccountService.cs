using EOM.TSHotelManagement.Common;
using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Data;
using EOM.TSHotelManagement.Domain;
using jvncorelib.CodeLib;
using jvncorelib.EntityLib;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Transactions;

namespace EOM.TSHotelManagement.Service
{
    public partial class CustomerAccountService : ICustomerAccountService
    {
        /// <summary>
        /// 客户账号
        /// </summary>
        private readonly GenericRepository<CustomerAccount> customerAccountRepository;

        /// <summary>
        /// 客户
        /// </summary>
        private readonly GenericRepository<Customer> customerRepository;

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

        /// <summary>
        /// JWT加密
        /// </summary>
        private readonly JWTHelper jWTHelper;


        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerAccountRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="dataProtector"></param>
        /// <param name="jWTHelper"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="accountRegex"></param>
        /// <param name="passwordRegex"></param>
        public CustomerAccountService(GenericRepository<CustomerAccount> customerAccountRepository, GenericRepository<Customer> customerRepository, GenericRepository<Role> roleRepository, GenericRepository<UserRole> userRoleRepository, DataProtectionHelper dataProtector, JWTHelper jWTHelper, IHttpContextAccessor httpContextAccessor, Regex accountRegex, Regex passwordRegex)
        {
            this.customerAccountRepository = customerAccountRepository;
            this.customerRepository = customerRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.dataProtector = dataProtector;
            this.jWTHelper = jWTHelper;
            _httpContextAccessor = httpContextAccessor;
            AccountRegex = accountRegex;
            PasswordRegex = passwordRegex;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="readCustomerAccountInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadCustomerAccountOutputDto> Login(ReadCustomerAccountInputDto readCustomerAccountInputDto)
        {
            if (readCustomerAccountInputDto.Account.IsNullOrEmpty() || readCustomerAccountInputDto.Password.IsNullOrEmpty())
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.BadRequest, Message = LocalizationHelper.GetLocalizedString("Account or Password cannot be empty", "账号或密码不能为空"), Data = new ReadCustomerAccountOutputDto() };

            var customerAccount = customerAccountRepository.AsQueryable().Single(x => x.Account == readCustomerAccountInputDto.Account);
            if (customerAccount == null)
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.NotFound, Message = LocalizationHelper.GetLocalizedString("Account not found", "账号不存在"), Data = new ReadCustomerAccountOutputDto() };

            if (!dataProtector.CompareCustomerData(customerAccount.Password, readCustomerAccountInputDto.Password))
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.Unauthorized, Message = LocalizationHelper.GetLocalizedString("Invalid account or password", "账号或密码错误"), Data = new ReadCustomerAccountOutputDto() };

            var copyCustomerAccount = customerAccount;

            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

            customerAccount.LastLoginIp = ipAddress;
            customerAccount.LastLoginTime = DateTime.Now;
            customerAccountRepository.Update(customerAccount);

            copyCustomerAccount.Password = null;

            copyCustomerAccount.UserToken = jWTHelper.GenerateJWT(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, customerAccount.Name),
                new Claim(ClaimTypes.SerialNumber, customerAccount.CustomerNumber),
                new Claim("account", customerAccount.Account),
                new Claim(ClaimTypes.UserData, JsonSerializer.Serialize(customerAccount)),
            }), 10080); // 7天有效期

            return new SingleOutputDto<ReadCustomerAccountOutputDto>()
            {
                Code = BusinessStatusCode.Success,
                Message = LocalizationHelper.GetLocalizedString("Login successful", "登录成功"),
                Data = new ReadCustomerAccountOutputDto
                {
                    Account = copyCustomerAccount.Account,
                    EmailAddress = copyCustomerAccount.EmailAddress,
                    Name = copyCustomerAccount.Name,
                    LastLoginIp = copyCustomerAccount.LastLoginIp,
                    LastLoginTime = copyCustomerAccount.LastLoginTime,
                    Status = copyCustomerAccount.Status,
                    UserToken = copyCustomerAccount.UserToken
                },
            };
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="readCustomerAccountInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadCustomerAccountOutputDto> Register(ReadCustomerAccountInputDto readCustomerAccountInputDto)
        {

            if (readCustomerAccountInputDto.Account.IsNullOrEmpty() || readCustomerAccountInputDto.Password.IsNullOrEmpty())
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.BadRequest, Message = LocalizationHelper.GetLocalizedString("Account or Password cannot be empty", "账号或密码不能为空"), Data = new ReadCustomerAccountOutputDto() };

            if (readCustomerAccountInputDto.Account.Length < 3 || readCustomerAccountInputDto.Account.Length > 20)
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.BadRequest, Message = LocalizationHelper.GetLocalizedString("Account length must be between 3 and 20 characters", "账号长度必须在3到20个字符之间"), Data = new ReadCustomerAccountOutputDto() };

            if (!AccountRegex.IsMatch(readCustomerAccountInputDto.Account))
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.BadRequest, Message = LocalizationHelper.GetLocalizedString("Account can only contain letters, numbers, and underscores", "账号只能包含字母、数字和下划线"), Data = new ReadCustomerAccountOutputDto() };

            if (!PasswordRegex.IsMatch(readCustomerAccountInputDto.Password))
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.BadRequest, Message = LocalizationHelper.GetLocalizedString("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number", "密码必须至少8个字符，并且包含至少一个大写字母、一个小写字母和一个数字"), Data = new ReadCustomerAccountOutputDto() };

            var customerAccount = customerAccountRepository.AsQueryable().Single(x => x.Account == readCustomerAccountInputDto.Account);

            if (customerAccount != null)
                return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.Conflict, Message = LocalizationHelper.GetLocalizedString("Account already exists", "账号已存在"), Data = new ReadCustomerAccountOutputDto() };

            var password = dataProtector.EncryptCustomerData(readCustomerAccountInputDto.Password);
            var customerNumber = new UniqueCode().GetNewId("TS-");

            using (TransactionScope scope = new TransactionScope())
            {
                customerAccount = new CustomerAccount
                {
                    CustomerNumber = customerNumber,
                    Account = readCustomerAccountInputDto.Account,
                    EmailAddress = string.Empty,
                    Name = readCustomerAccountInputDto.Account,
                    Password = password,
                    Status = 0,
                    LastLoginIp = string.Empty,
                    LastLoginTime = DateTime.Now,
                    DataInsUsr = readCustomerAccountInputDto.Account,
                    DataInsDate = DateTime.Now
                };

                var accountResult = customerAccountRepository.Insert(customerAccount);
                if (!accountResult)
                {
                    return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.InternalServerError, Message = LocalizationHelper.GetLocalizedString("Account insert failed", "账号插入失败"), Data = new ReadCustomerAccountOutputDto() };
                }

                var customerResult = customerRepository.Insert(new Customer
                {
                    CustomerNumber = customerNumber,
                    CustomerName = string.Empty,
                    CustomerGender = 0,
                    CustomerType = 0,
                    CustomerPhoneNumber = string.Empty,
                    CustomerAddress = string.Empty,
                    DateOfBirth = DateOnly.MinValue,
                    IdCardNumber = string.Empty,
                    PassportId = 0,
                    IsDelete = 0,
                    DataInsUsr = readCustomerAccountInputDto.Account,
                    DataInsDate = DateTime.Now
                });
                if (!customerResult)
                {
                    return new SingleOutputDto<ReadCustomerAccountOutputDto>() { Code = BusinessStatusCode.InternalServerError, Message = LocalizationHelper.GetLocalizedString("Customer insert failed", "客户插入失败"), Data = new ReadCustomerAccountOutputDto() };
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
                        DataInsUsr = readCustomerAccountInputDto.Account,
                        DataInsDate = DateTime.Now
                    });
                }

                // 绑定客户到客户组角色
                if (!userRoleRepository.AsQueryable().Any(ur => ur.UserNumber == customerNumber && ur.RoleNumber == customerRoleNumber && ur.IsDelete != 1))
                {
                    userRoleRepository.Insert(new UserRole
                    {
                        UserNumber = customerNumber,
                        RoleNumber = customerRoleNumber,
                        IsDelete = 0,
                        DataInsUsr = readCustomerAccountInputDto.Account,
                        DataInsDate = DateTime.Now
                    });
                }

                customerAccount.UserToken = jWTHelper.GenerateJWT(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, customerAccount.Name),
                    new Claim(ClaimTypes.SerialNumber, customerNumber),
                    new Claim("account", customerAccount.Account),
                    new Claim(ClaimTypes.UserData, JsonSerializer.Serialize(customerAccount)),
                }), 10080); // 7天有效期

                scope.Complete();

                return new SingleOutputDto<ReadCustomerAccountOutputDto>()
                {
                    Code = BusinessStatusCode.Success,
                    Message = LocalizationHelper.GetLocalizedString("Register successful", "注册成功，稍后将自动登录"),
                    Data = new ReadCustomerAccountOutputDto
                    {
                        Account = customerAccount.Account,
                        EmailAddress = customerAccount.EmailAddress,
                        Name = customerAccount.Name,
                        LastLoginIp = customerAccount.LastLoginIp,
                        LastLoginTime = customerAccount.LastLoginTime,
                        Status = customerAccount.Status,
                        UserToken = customerAccount.UserToken
                    },
                };
            }
        }

        private readonly Regex AccountRegex = new Regex(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled);
        private readonly Regex PasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", RegexOptions.Compiled);

    }
}
