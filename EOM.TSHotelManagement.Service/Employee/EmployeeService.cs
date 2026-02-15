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
using System.Security.Claims;

namespace EOM.TSHotelManagement.Service
{
    /// <summary>
    /// 员工信息接口实现类
    /// </summary>
    /// <remarks>
    /// 构造函数
    /// </remarks>
    /// <param name="workerRepository"></param>
    /// <param name="photoRepository"></param>
    /// <param name="educationRepository"></param>
    /// <param name="nationRepository"></param>
    /// <param name="deptRepository"></param>
    /// <param name="positionRepository"></param>
    /// <param name="passportTypeRepository"></param>
    /// <param name="jWTHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="logger"></param>
    public class EmployeeService(GenericRepository<Employee> workerRepository, GenericRepository<EmployeePhoto> photoRepository, GenericRepository<Education> educationRepository, GenericRepository<Nation> nationRepository, GenericRepository<Department> deptRepository, GenericRepository<Position> positionRepository, GenericRepository<PassportType> passportTypeRepository, JWTHelper jWTHelper, MailHelper mailHelper, DataProtectionHelper dataProtectionHelper, ILogger<EmployeeService> logger) : IEmployeeService
    {
        /// <summary>
        /// 员工信息
        /// </summary>
        private readonly GenericRepository<Employee> workerRepository = workerRepository;

        /// <summary>
        /// 员工照片
        /// </summary>
        private readonly GenericRepository<EmployeePhoto> photoRepository = photoRepository;

        /// <summary>
        /// 学历类型
        /// </summary>
        private readonly GenericRepository<Education> educationRepository = educationRepository;

        /// <summary>
        /// 民族类型
        /// </summary>
        private readonly GenericRepository<Nation> nationRepository = nationRepository;

        /// <summary>
        /// 部门
        /// </summary>
        private readonly GenericRepository<Department> deptRepository = deptRepository;

        /// <summary>
        /// 职务
        /// </summary>
        private readonly GenericRepository<Position> positionRepository = positionRepository;

        /// <summary>
        /// 证件
        /// </summary>
        private readonly GenericRepository<PassportType> passportTypeRepository = passportTypeRepository;

        /// <summary>
        /// 数据保护
        /// </summary>
        private readonly DataProtectionHelper dataProtector = dataProtectionHelper;

        /// <summary>
        /// JWT加密
        /// </summary>
        private readonly JWTHelper jWTHelper = jWTHelper;

        /// <summary>
        /// 邮件助手
        /// </summary>
        private readonly MailHelper mailHelper = mailHelper;

        private readonly ILogger<EmployeeService> logger = logger;

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseResponse UpdateEmployee(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                //加密联系方式
                var sourceTelStr = string.Empty;
                if (!updateEmployeeInputDto.PhoneNumber.IsNullOrEmpty())
                {
                    sourceTelStr = dataProtector.EncryptEmployeeData(updateEmployeeInputDto.PhoneNumber);
                }
                //加密身份证
                var sourceIdStr = string.Empty;
                if (!updateEmployeeInputDto.IdCardNumber.IsNullOrEmpty())
                {
                    sourceIdStr = dataProtector.EncryptEmployeeData(updateEmployeeInputDto.IdCardNumber);
                }
                updateEmployeeInputDto.PhoneNumber = sourceTelStr;
                updateEmployeeInputDto.IdCardNumber = sourceIdStr;

                var password = workerRepository.GetFirst(a => a.EmployeeId == updateEmployeeInputDto.EmployeeId).Password;
                updateEmployeeInputDto.Password = password;

                workerRepository.Update(EntityMapper.Map<UpdateEmployeeInputDto, Employee>(updateEmployeeInputDto));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating employee information for EmployeeId: {EmployeeId}", updateEmployeeInputDto.EmployeeId);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();

        }

        /// <summary>
        /// 员工账号禁/启用
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseResponse ManagerEmployeeAccount(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                workerRepository.Update(a => new Employee()
                {
                    IsEnable = updateEmployeeInputDto.IsEnable,
                }, a => a.EmployeeId == updateEmployeeInputDto.EmployeeId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error managing employee account for EmployeeId: {EmployeeId}", updateEmployeeInputDto.EmployeeId);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="createEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseResponse AddEmployee(CreateEmployeeInputDto createEmployeeInputDto)
        {
            try
            {
                //加密联系方式
                var sourceTelStr = string.Empty;
                if (!createEmployeeInputDto.PhoneNumber.IsNullOrEmpty())
                {
                    sourceTelStr = dataProtector.EncryptEmployeeData(createEmployeeInputDto.PhoneNumber);
                }
                //加密身份证
                var sourceIdStr = string.Empty;
                if (!createEmployeeInputDto.IdCardNumber.IsNullOrEmpty())
                {
                    sourceIdStr = dataProtector.EncryptEmployeeData(createEmployeeInputDto.IdCardNumber);
                }
                // 加密密码
                var sourcePwdStr = string.Empty;
                var newPassword = new RandomStringGenerator().GenerateSecurePassword();
                sourcePwdStr = dataProtector.EncryptEmployeeData(newPassword);

                var emailTemplate = EmailTemplate.GetNewRegistrationTemplate(createEmployeeInputDto.EmployeeName, newPassword);
                var result = mailHelper.SendMail(new List<string> { createEmployeeInputDto.EmailAddress }, emailTemplate.Subject, emailTemplate.Body, new List<string> { createEmployeeInputDto.EmailAddress });
                if (!result)
                {
                    return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("E-Mail Config Invaild, Add Employee Faild", "电子邮件配置无效，添加员工失败"), Code = BusinessStatusCode.InternalServerError };
                }

                createEmployeeInputDto.PhoneNumber = sourceTelStr;
                createEmployeeInputDto.IdCardNumber = sourceIdStr;
                createEmployeeInputDto.Password = sourcePwdStr;

                workerRepository.Insert(EntityMapper.Map<CreateEmployeeInputDto, Employee>(createEmployeeInputDto));

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding employee: {Message}", ex.Message);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }
            return new BaseResponse();
        }

        /// <summary>
        /// 获取所有工作人员信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadEmployeeOutputDto> SelectEmployeeAll(ReadEmployeeInputDto readEmployeeInputDto)
        {
            readEmployeeInputDto ??= new ReadEmployeeInputDto();

            var where = SqlFilterBuilder.BuildExpression<Employee, ReadEmployeeInputDto>(readEmployeeInputDto, nameof(Employee.HireDate));
            var query = workerRepository.AsQueryable().Where(a => a.IsDelete != 1);
            var whereExpression = where.ToExpression();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            query = query.OrderBy(a => a.EmployeeId);

            var count = 0;
            List<Employee> employees;
            if (!readEmployeeInputDto.IgnorePaging)
            {
                var page = readEmployeeInputDto.Page > 0 ? readEmployeeInputDto.Page : 1;
                var pageSize = readEmployeeInputDto.PageSize > 0 ? readEmployeeInputDto.PageSize : 15;
                employees = query.ToPageList(page, pageSize, ref count);
            }
            else
            {
                employees = query.ToList();
                count = employees.Count;
            }

            var educations = educationRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.EducationNumber)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.EducationName ?? "");
            var nations = nationRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.NationNumber)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.NationName ?? "");
            var departments = deptRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.DepartmentNumber)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.DepartmentName ?? "");
            var positions = positionRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.PositionNumber)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.PositionName ?? "");
            var passports = passportTypeRepository.GetList(a => a.IsDelete != 1)
                .GroupBy(a => a.PassportId)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.PassportName ?? "");

            var helper = new EnumHelper();
            var genderMap = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .ToDictionary(e => (int)e, e => helper.GetEnumDescription(e) ?? "");
            var politicalAffiliationMap = Enum.GetValues(typeof(PoliticalAffiliation))
                .Cast<PoliticalAffiliation>()
                .ToDictionary(e => e.ToString(), e => helper.GetEnumDescription(e) ?? "", StringComparer.OrdinalIgnoreCase);

            List<ReadEmployeeOutputDto> data;
            var useParallelProjection = readEmployeeInputDto.IgnorePaging && employees.Count >= 200;
            if (useParallelProjection)
            {
                var dtoArray = new ReadEmployeeOutputDto[employees.Count];
                System.Threading.Tasks.Parallel.For(0, employees.Count, i =>
                {
                    var source = employees[i];
                    dtoArray[i] = new ReadEmployeeOutputDto
                    {
                        EmployeeId = source.EmployeeId,
                        EmployeeName = source.EmployeeName,
                        Gender = source.Gender,
                        GenderName = genderMap.TryGetValue(source.Gender, out var genderName) ? genderName : "",
                        DateOfBirth = source.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                        Ethnicity = source.Ethnicity,
                        EthnicityName = nations.TryGetValue(source.Ethnicity, out var ethnicityName) ? ethnicityName : "",
                        PhoneNumber = dataProtector.SafeDecryptEmployeeData(source.PhoneNumber),
                        Department = source.Department,
                        DepartmentName = departments.TryGetValue(source.Department, out var departmentName) ? departmentName : "",
                        Address = source.Address ?? "",
                        Position = source.Position,
                        PositionName = positions.TryGetValue(source.Position, out var positionName) ? positionName : "",
                        IdCardType = source.IdCardType,
                        IdCardTypeName = passports.TryGetValue(source.IdCardType, out var passportName) ? passportName : "",
                        IdCardNumber = dataProtector.SafeDecryptEmployeeData(source.IdCardNumber),
                        HireDate = source.HireDate.ToDateTime(TimeOnly.MinValue),
                        PoliticalAffiliation = source.PoliticalAffiliation,
                        PoliticalAffiliationName = politicalAffiliationMap.TryGetValue(source.PoliticalAffiliation ?? "", out var politicalAffiliationName) ? politicalAffiliationName : "",
                        EducationLevel = source.EducationLevel,
                        EducationLevelName = educations.TryGetValue(source.EducationLevel, out var educationName) ? educationName : "",
                        IsEnable = source.IsEnable,
                        IsInitialize = source.IsInitialize,
                        Password = source.Password,
                        EmailAddress = source.EmailAddress,
                        PhotoUrl = string.Empty,
                        DataInsUsr = source.DataInsUsr,
                        DataInsDate = source.DataInsDate,
                        DataChgUsr = source.DataChgUsr,
                        DataChgDate = source.DataChgDate,
                        IsDelete = source.IsDelete
                    };
                });
                data = dtoArray.ToList();
            }
            else
            {
                data = new List<ReadEmployeeOutputDto>(employees.Count);
                employees.ForEach(source =>
                {
                    data.Add(new ReadEmployeeOutputDto
                    {
                        EmployeeId = source.EmployeeId,
                        EmployeeName = source.EmployeeName,
                        Gender = source.Gender,
                        GenderName = genderMap.TryGetValue(source.Gender, out var genderName) ? genderName : "",
                        DateOfBirth = source.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                        Ethnicity = source.Ethnicity,
                        EthnicityName = nations.TryGetValue(source.Ethnicity, out var ethnicityName) ? ethnicityName : "",
                        PhoneNumber = dataProtector.SafeDecryptEmployeeData(source.PhoneNumber),
                        Department = source.Department,
                        DepartmentName = departments.TryGetValue(source.Department, out var departmentName) ? departmentName : "",
                        Address = source.Address ?? "",
                        Position = source.Position,
                        PositionName = positions.TryGetValue(source.Position, out var positionName) ? positionName : "",
                        IdCardType = source.IdCardType,
                        IdCardTypeName = passports.TryGetValue(source.IdCardType, out var passportName) ? passportName : "",
                        IdCardNumber = dataProtector.SafeDecryptEmployeeData(source.IdCardNumber),
                        HireDate = source.HireDate.ToDateTime(TimeOnly.MinValue),
                        PoliticalAffiliation = source.PoliticalAffiliation,
                        PoliticalAffiliationName = politicalAffiliationMap.TryGetValue(source.PoliticalAffiliation ?? "", out var politicalAffiliationName) ? politicalAffiliationName : "",
                        EducationLevel = source.EducationLevel,
                        EducationLevelName = educations.TryGetValue(source.EducationLevel, out var educationName) ? educationName : "",
                        IsEnable = source.IsEnable,
                        IsInitialize = source.IsInitialize,
                        Password = source.Password,
                        EmailAddress = source.EmailAddress,
                        PhotoUrl = string.Empty,
                        DataInsUsr = source.DataInsUsr,
                        DataInsDate = source.DataInsDate,
                        DataChgUsr = source.DataChgUsr,
                        DataChgDate = source.DataChgDate,
                        IsDelete = source.IsDelete
                    });
                });
            }

            return new ListOutputDto<ReadEmployeeOutputDto>
            {
                Data = new PagedData<ReadEmployeeOutputDto>
                {
                    Items = data,
                    TotalCount = count
                }
            };
        }

        /// <summary>
        /// 根据登录名称查询员工信息
        /// </summary>
        /// <param name="readEmployeeInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadEmployeeOutputDto> SelectEmployeeInfoByEmployeeId(ReadEmployeeInputDto readEmployeeInputDto)
        {
            Employee w = new Employee();
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
            w = workerRepository.GetFirst(a => a.EmployeeId == readEmployeeInputDto.EmployeeId);
            //解密身份证号码
            var sourceStr = w.IdCardNumber.IsNullOrEmpty() ? "" : dataProtector.SafeDecryptEmployeeData(w.IdCardNumber);
            w.IdCardNumber = sourceStr;
            //解密联系方式
            var sourceTelStr = w.PhoneNumber.IsNullOrEmpty() ? "" : dataProtector.SafeDecryptEmployeeData(w.PhoneNumber);
            w.PhoneNumber = sourceTelStr;
            //性别类型
            var sexType = genders.SingleOrDefault(a => a.Id == w.Gender);
            w.GenderName = sexType.Description.IsNullOrEmpty() ? "" : sexType.Description;
            //教育程度
            var eduction = educationRepository.GetFirst(a => a.EducationNumber == w.EducationLevel);
            w.EducationLevelName = eduction.EducationName.IsNullOrEmpty() ? "" : eduction.EducationName;
            //民族类型
            var nation = nationRepository.GetFirst(a => a.NationNumber == w.Ethnicity);
            w.EthnicityName = nation.NationName.IsNullOrEmpty() ? "" : nation.NationName;
            //部门
            var dept = deptRepository.GetFirst(a => a.DepartmentNumber == w.Department);
            w.DepartmentName = dept.DepartmentName.IsNullOrEmpty() ? "" : dept.DepartmentName;
            //职位
            var position = positionRepository.GetFirst(a => a.PositionNumber == w.Position);
            w.PositionName = position.PositionName.IsNullOrEmpty() ? "" : position.PositionName;
            var passport = passportTypeRepository.GetFirst(a => a.PassportId == w.IdCardType);
            w.IdCardTypeName = passport.IsNullOrEmpty() ? "" : passport.PassportName;
            //面貌
            w.PoliticalAffiliationName = new EnumHelper().GetDescriptionByName<PoliticalAffiliation>(w.PoliticalAffiliation);

            var source = EntityMapper.Map<Employee, ReadEmployeeOutputDto>(w);

            var employeePhoto = photoRepository.GetFirst(a => a.EmployeeId.Equals(source.EmployeeId));
            if (employeePhoto != null && !string.IsNullOrEmpty(employeePhoto.PhotoPath))
                source.PhotoUrl = employeePhoto.PhotoPath ?? string.Empty;

            return new SingleOutputDto<ReadEmployeeOutputDto> { Data = source };
        }

        /// <summary>
        /// 员工端登录
        /// </summary>
        /// <param name="employeeLoginDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadEmployeeOutputDto> EmployeeLogin(EmployeeLoginDto employeeLoginDto)
        {
            Employee w = new Employee();
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
            w = workerRepository.GetFirst(a => a.EmployeeId == employeeLoginDto.EmployeeId || a.EmailAddress == employeeLoginDto.EmailAddress);
            if (w == null)
            {
                w = null;
                return new SingleOutputDto<ReadEmployeeOutputDto> { Data = null, Message = LocalizationHelper.GetLocalizedString("Employee does not exist or entered incorrectly", "员工不存在或输入有误") };
            }
            var correctPassword = dataProtector.CompareEmployeeData(w.Password, employeeLoginDto.Password);

            if (!correctPassword)
            {
                return new SingleOutputDto<ReadEmployeeOutputDto> { Data = null, Message = LocalizationHelper.GetLocalizedString("Invalid account or password", "账号或密码错误") };
            }
            w.Password = "";
            //性别类型
            var sexType = genders.SingleOrDefault(a => a.Id == w.Gender);
            w.GenderName = sexType.Description.IsNullOrEmpty() ? "" : sexType.Description;
            //教育程度
            var eduction = educationRepository.GetFirst(a => a.EducationNumber == w.EducationLevel);
            w.EducationLevelName = eduction.EducationName.IsNullOrEmpty() ? "" : eduction.EducationName;
            //民族类型
            var nation = nationRepository.GetFirst(a => a.NationNumber == w.Ethnicity);
            w.EthnicityName = nation.NationName.IsNullOrEmpty() ? "" : nation.NationName;
            //部门
            var dept = deptRepository.GetFirst(a => a.DepartmentNumber == w.Department);
            w.DepartmentName = dept.DepartmentName.IsNullOrEmpty() ? "" : dept.DepartmentName;
            //职位
            var position = positionRepository.GetFirst(a => a.PositionNumber == w.Position);
            w.PositionName = position.PositionName.IsNullOrEmpty() ? "" : position.PositionName;

            w.UserToken = jWTHelper.GenerateJWT(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, w.EmployeeName),
                new Claim(ClaimTypes.SerialNumber, w.EmployeeId)
            }));
            return new SingleOutputDto<ReadEmployeeOutputDto> { Data = EntityMapper.Map<Employee, ReadEmployeeOutputDto>(w) };
        }

        /// <summary>
        /// 修改员工账号密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseResponse UpdateEmployeeAccountPassword(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                var employee = workerRepository.GetFirst(a => a.EmployeeId == updateEmployeeInputDto.EmployeeId);

                if (employee.IsNullOrEmpty())
                {
                    return new BaseResponse()
                    {
                        Message = LocalizationHelper.GetLocalizedString("This employee does not exists", "员工不存在"),
                        Code = BusinessStatusCode.InternalServerError
                    };
                }

                var currentPassword = dataProtector.SafeDecryptEmployeeData(employee.Password);

                if (!updateEmployeeInputDto.OldPassword.Equals(currentPassword))
                {
                    return new BaseResponse()
                    {
                        Message = LocalizationHelper.GetLocalizedString("The old password is incorrect", "旧密码不正确"),
                        Code = BusinessStatusCode.InternalServerError
                    };
                }

                if (updateEmployeeInputDto.Password.Equals(currentPassword))
                {
                    return new BaseResponse()
                    {
                        Message = LocalizationHelper.GetLocalizedString("The new password cannot be the same as the old password", "新密码不能与旧密码相同"),
                        Code = BusinessStatusCode.InternalServerError
                    };
                }

                var newPwd = updateEmployeeInputDto.Password;
                string encrypted = dataProtector.EncryptEmployeeData(newPwd);

                if (!employee.EmailAddress.IsNullOrEmpty())
                {
                    var mailTemplate = EmailTemplate.GetUpdatePasswordTemplate(employee.EmployeeName, newPwd);
                    mailHelper.SendMail(new List<string> { employee.EmailAddress }, mailTemplate.Subject, mailTemplate.Body, new List<string> { employee.EmailAddress });
                }

                employee.Password = encrypted;
                employee.IsInitialize = 1;
                workerRepository.Update(employee);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating employee password for EmployeeId: {EmployeeId}", updateEmployeeInputDto.EmployeeId);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }

            return new BaseResponse();
        }

        /// <summary>
        /// 重置员工账号密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseResponse ResetEmployeeAccountPassword(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                var newPwd = new RandomStringGenerator().GenerateSecurePassword();
                string encrypted = dataProtector.EncryptEmployeeData(newPwd);

                var employee = workerRepository.GetFirst(a => a.EmployeeId == updateEmployeeInputDto.EmployeeId);

                var emailAddress = employee.EmailAddress;

                if (emailAddress.IsNullOrEmpty())
                {
                    return new BaseResponse()
                    {
                        Message = LocalizationHelper.GetLocalizedString("No bound email address was found for the employee. Password reset cannot be completed."
                        , "未找到员工绑定的电子邮箱，无法重置密码。"),
                        Code = BusinessStatusCode.InternalServerError
                    };
                }

                var mailTemplate = EmailTemplate.GetResetPasswordTemplate(employee.EmployeeName, newPwd);

                var result = mailHelper.SendMail(new List<string> { emailAddress },
                    mailTemplate.Subject, mailTemplate.Body,
                    new List<string> { emailAddress });
                if (!result)
                {
                    return new BaseResponse { Message = LocalizationHelper.GetLocalizedString("E-Mail Config Invaild, Reset Password Faild", "电子邮件配置无效，重置密码失败"), Code = BusinessStatusCode.InternalServerError };
                }

                employee.Password = encrypted;

                workerRepository.Update(employee);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error resetting employee password for EmployeeId: {EmployeeId}", updateEmployeeInputDto.EmployeeId);
                return new BaseResponse { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), Code = BusinessStatusCode.InternalServerError };
            }

            return new BaseResponse();
        }
    }
}
