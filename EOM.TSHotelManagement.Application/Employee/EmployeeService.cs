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
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;

namespace EOM.TSHotelManagement.Application
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
    /// <param name="dataProtectionProvider"></param>
    /// <param name="jWTHelper"></param>
    /// <param name="mailHelper"></param>
    public class EmployeeService(GenericRepository<Employee> workerRepository, GenericRepository<EmployeePhoto> photoRepository, GenericRepository<Education> educationRepository, GenericRepository<Nation> nationRepository, GenericRepository<Department> deptRepository, GenericRepository<Position> positionRepository, GenericRepository<PassportType> passportTypeRepository, IDataProtectionProvider dataProtectionProvider, JWTHelper jWTHelper, MailHelper mailHelper) : IEmployeeService
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
        private readonly IDataProtector dataProtector = dataProtectionProvider.CreateProtector("EmployeeInfoProtector");

        /// <summary>
        /// JWT加密
        /// </summary>
        private readonly JWTHelper jWTHelper = jWTHelper;

        /// <summary>
        /// 邮件助手
        /// </summary>
        private readonly MailHelper mailHelper = mailHelper;

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateEmployee(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                //加密联系方式
                var sourceTelStr = string.Empty;
                if (!updateEmployeeInputDto.PhoneNumber.IsNullOrEmpty())
                {
                    sourceTelStr = dataProtector.Protect(updateEmployeeInputDto.PhoneNumber);
                }
                //加密身份证
                var sourceIdStr = string.Empty;
                if (!updateEmployeeInputDto.IdCardNumber.IsNullOrEmpty())
                {
                    sourceIdStr = dataProtector.Protect(updateEmployeeInputDto.IdCardNumber);
                }
                updateEmployeeInputDto.PhoneNumber = sourceTelStr;
                updateEmployeeInputDto.IdCardNumber = sourceIdStr;

                var password = workerRepository.GetSingle(a => a.EmployeeId == updateEmployeeInputDto.EmployeeId).Password;
                updateEmployeeInputDto.Password = password;

                workerRepository.Update(EntityMapper.Map<UpdateEmployeeInputDto, Employee>(updateEmployeeInputDto));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();

        }

        /// <summary>
        /// 员工账号禁/启用
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto ManagerEmployeeAccount(UpdateEmployeeInputDto updateEmployeeInputDto)
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
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新员工职位和部门
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>

        public BaseOutputDto UpdateEmployeePositionAndClub(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                workerRepository.Update(a => new Employee()
                {
                    Department = updateEmployeeInputDto.Department,
                    Position = updateEmployeeInputDto.Position,
                    DataChgUsr = updateEmployeeInputDto.DataChgUsr,
                    DataChgDate = updateEmployeeInputDto.DataChgDate
                }, a => a.EmployeeId == updateEmployeeInputDto.EmployeeId);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="createEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto AddEmployee(CreateEmployeeInputDto createEmployeeInputDto)
        {
            try
            {
                //加密联系方式
                var sourceTelStr = string.Empty;
                if (!createEmployeeInputDto.PhoneNumber.IsNullOrEmpty())
                {
                    sourceTelStr = dataProtector.Protect(createEmployeeInputDto.PhoneNumber);
                }
                //加密身份证
                var sourceIdStr = string.Empty;
                if (!createEmployeeInputDto.IdCardNumber.IsNullOrEmpty())
                {
                    sourceIdStr = dataProtector.Protect(createEmployeeInputDto.IdCardNumber);
                }
                // 加密密码
                var sourcePwdStr = string.Empty;
                var newPassword = new RandomStringGenerator().GenerateSecurePassword();
                sourcePwdStr = dataProtector.Protect(newPassword);

                createEmployeeInputDto.PhoneNumber = sourceTelStr;
                createEmployeeInputDto.IdCardNumber = sourceIdStr;
                createEmployeeInputDto.Password = sourcePwdStr;

                workerRepository.Insert(EntityMapper.Map<CreateEmployeeInputDto, Employee>(createEmployeeInputDto));

                var Subject = LocalizationHelper.GetLocalizedString("New Registration Notification", "​新注册通知");
                var Body = $@"<h1>{LocalizationHelper.GetLocalizedString("Dear User,", "尊敬的用户：")}</h1>
                            <p>{LocalizationHelper.GetLocalizedString(
                                $"You have successfully registered to the system on {DateTime.Now:yyyy/MM/dd}. Your account credentials are as follows:​",
                                $"您已于<strong>{DateTime.Now:yyyy/MM/dd}</strong>新注册系统成功，账号密码如下：")}
                            </p>
                            <p style='color: #666;'>{newPassword}</p>
                            <p>{LocalizationHelper.GetLocalizedString(
                                "Please keep your password secure and change it after login.",
                                "请妥善保管密码，并在成功登录后修改为你能记住的密码！")}</p>";

                mailHelper.SendMail(new List<string> { createEmployeeInputDto.EmailAddress }, Subject, Body, new List<string> { createEmployeeInputDto.EmailAddress });

            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 获取所有工作人员信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadEmployeeOutputDto> SelectEmployeeAll(ReadEmployeeInputDto readEmployeeInputDto)
        {
            var where = Expressionable.Create<Employee>();

            where = where.And(a => a.IsDelete != 1);

            //查询所有教育程度信息
            List<Education> educations = new List<Education>();
            educations = educationRepository.GetList(a => a.IsDelete != 1);
            //查询所有性别类型信息
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
            //查询所有民族类型信息
            List<Nation> nations = new List<Nation>();
            nations = nationRepository.GetList(a => a.IsDelete != 1);
            //查询所有部门信息
            List<Department> depts = new List<Department>();
            depts = deptRepository.GetList(a => a.IsDelete != 1);
            //查询所有职位信息
            List<Position> positions = new List<Position>();
            positions = positionRepository.GetList(a => a.IsDelete != 1);
            //查询所有证件类型
            List<PassportType> passportTypes = new List<PassportType>();
            passportTypes = passportTypeRepository.GetList(a => a.IsDelete != 1);
            //查询所有员工信息
            List<Employee> employees = new List<Employee>();

            var count = 0;

            if (!readEmployeeInputDto.IgnorePaging && readEmployeeInputDto.Page != 0 && readEmployeeInputDto.PageSize != 0)
            {
                employees = workerRepository.AsQueryable().Where(where.ToExpression())
                    .OrderBy(a => a.EmployeeId)
                    .ToPageList(readEmployeeInputDto.Page, readEmployeeInputDto.PageSize, ref count);
            }
            else
            {
                employees = workerRepository.AsQueryable().Where(where.ToExpression())
                    .OrderBy(a => a.EmployeeId)
                    .ToList();
            }

            employees.ForEach(source =>
            {
                try
                {
                    //解密身份证号码
                    var sourceStr = dataProtector.Unprotect(source.IdCardNumber);
                    source.IdCardNumber = sourceStr;
                    //解密联系方式
                    var sourceTelStr = dataProtector.Unprotect(source.PhoneNumber);
                    source.PhoneNumber = sourceTelStr;
                }
                catch (Exception)
                {
                    source.IdCardNumber = source.IdCardNumber;
                    source.PhoneNumber = source.PhoneNumber;
                }
                //性别类型
                var sexType = genders.SingleOrDefault(a => a.Id == source.Gender);
                source.GenderName = sexType.IsNullOrEmpty() ? "" : sexType.Description;
                //教育程度
                var eduction = educations.SingleOrDefault(a => a.EducationNumber == source.EducationLevel);
                source.EducationLevelName = eduction.IsNullOrEmpty() ? "" : eduction.EducationName;
                //民族类型
                var nation = nations.SingleOrDefault(a => a.NationNumber == source.Ethnicity);
                source.EthnicityName = nation.IsNullOrEmpty() ? "" : nation.NationName;
                //部门
                var dept = depts.SingleOrDefault(a => a.DepartmentNumber == source.Department);
                source.DepartmentName = dept.IsNullOrEmpty() ? "" : dept.DepartmentName;
                //职位
                var position = positions.SingleOrDefault(a => a.PositionNumber == source.Position);
                source.PositionName = position.IsNullOrEmpty() ? "" : position.PositionName;
                var passport = passportTypes.SingleOrDefault(a => a.PassportId == source.IdCardType);
                source.IdCardTypeName = passport.IsNullOrEmpty() ? "" : passport.PassportName;
                //面貌
                source.PoliticalAffiliationName = GetDescriptionByName(source.PoliticalAffiliation);
            });

            var listSource = EntityMapper.MapList<Employee, ReadEmployeeOutputDto>(employees);

            return new ListOutputDto<ReadEmployeeOutputDto> { listSource = listSource, total = count };
        }

        /// <summary>
        /// 根据部门ID获取工作人员信息
        /// </summary>
        /// <param name="readEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto CheckEmployeeByDepartment(ReadEmployeeInputDto readEmployeeInputDto)
        {
            var workers = workerRepository.Count(a => a.Department.Equals(readEmployeeInputDto.Department));

            return new BaseOutputDto { Message = workers > 0 ? true.ToString() : false.ToString(), StatusCode = StatusCodeConstants.Success };
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
            w = workerRepository.GetSingle(a => a.EmployeeId == readEmployeeInputDto.EmployeeId);
            //解密身份证号码
            var sourceStr = w.IdCardNumber.IsNullOrEmpty() ? "" : dataProtector.Unprotect(w.IdCardNumber);
            w.IdCardNumber = sourceStr;
            //解密联系方式
            var sourceTelStr = w.PhoneNumber.IsNullOrEmpty() ? "" : dataProtector.Unprotect(w.PhoneNumber);
            w.PhoneNumber = sourceTelStr;
            //性别类型
            var sexType = genders.SingleOrDefault(a => a.Id == w.Gender);
            w.GenderName = sexType.Description.IsNullOrEmpty() ? "" : sexType.Description;
            //教育程度
            var eduction = educationRepository.GetSingle(a => a.EducationNumber == w.EducationLevel);
            w.EducationLevelName = eduction.EducationName.IsNullOrEmpty() ? "" : eduction.EducationName;
            //民族类型
            var nation = nationRepository.GetSingle(a => a.NationNumber == w.Ethnicity);
            w.EthnicityName = nation.NationName.IsNullOrEmpty() ? "" : nation.NationName;
            //部门
            var dept = deptRepository.GetSingle(a => a.DepartmentNumber == w.Department);
            w.DepartmentName = dept.DepartmentName.IsNullOrEmpty() ? "" : dept.DepartmentName;
            //职位
            var position = positionRepository.GetSingle(a => a.PositionNumber == w.Position);
            w.PositionName = position.PositionName.IsNullOrEmpty() ? "" : position.PositionName;
            var passport = passportTypeRepository.GetSingle(a => a.PassportId == w.IdCardType);
            w.IdCardTypeName = passport.IsNullOrEmpty() ? "" : passport.PassportName;
            //面貌
            w.PoliticalAffiliationName = GetDescriptionByName(w.PoliticalAffiliation);

            var source = EntityMapper.Map<Employee, ReadEmployeeOutputDto>(w);

            var employeePhoto = photoRepository.GetSingle(a => a.EmployeeId.Equals(source.EmployeeId));
            if (employeePhoto != null && !string.IsNullOrEmpty(employeePhoto.PhotoPath))
                source.PhotoUrl = employeePhoto.PhotoPath ?? string.Empty;

            return new SingleOutputDto<ReadEmployeeOutputDto> { Source = source };
        }

        /// <summary>
        /// 根据登录名称、密码查询员工信息
        /// </summary>
        /// <param name="readEmployeeInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadEmployeeOutputDto> SelectEmployeeInfoByEmployeeIdAndEmployeePwd(ReadEmployeeInputDto readEmployeeInputDto)
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
            w = workerRepository.GetSingle(a => a.EmployeeId == readEmployeeInputDto.EmployeeId);
            if (w == null)
            {
                w = null;
                return new SingleOutputDto<ReadEmployeeOutputDto> { Source = null };
            }

            var dbPwd = dataProtector.Unprotect(w.Password);

            if (dbPwd != readEmployeeInputDto.Password)
            {
                w = null;
                return new SingleOutputDto<ReadEmployeeOutputDto> { Source = EntityMapper.Map<Employee, ReadEmployeeOutputDto>(w) };
            }
            w.Password = "";
            //性别类型
            var sexType = genders.SingleOrDefault(a => a.Id == w.Gender);
            w.GenderName = sexType.Description.IsNullOrEmpty() ? "" : sexType.Description;
            //教育程度
            var eduction = educationRepository.GetSingle(a => a.EducationNumber == w.EducationLevel);
            w.EducationLevelName = eduction.EducationName.IsNullOrEmpty() ? "" : eduction.EducationName;
            //民族类型
            var nation = nationRepository.GetSingle(a => a.NationNumber == w.Ethnicity);
            w.EthnicityName = nation.NationName.IsNullOrEmpty() ? "" : nation.NationName;
            //部门
            var dept = deptRepository.GetSingle(a => a.DepartmentNumber == w.Department);
            w.DepartmentName = dept.DepartmentName.IsNullOrEmpty() ? "" : dept.DepartmentName;
            //职位
            var position = positionRepository.GetSingle(a => a.PositionNumber == w.Position);
            w.PositionName = position.PositionName.IsNullOrEmpty() ? "" : position.PositionName;

            w.UserToken = jWTHelper.GenerateJWT(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, w.EmployeeName), new Claim(ClaimTypes.SerialNumber, w.EmployeeId) }));

            return new SingleOutputDto<ReadEmployeeOutputDto> { Source = EntityMapper.Map<Employee, ReadEmployeeOutputDto>(w) };
        }

        /// <summary>
        /// 根据员工编号和密码修改密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdEmployeePwdByWorkNo(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                string NewPwd = dataProtector.Protect(updateEmployeeInputDto.Password);

                workerRepository.Update(a => new Employee()
                {
                    Password = NewPwd,
                    DataChgUsr = updateEmployeeInputDto.DataChgUsr
                }, a => a.EmployeeId == updateEmployeeInputDto.EmployeeId);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }

            return new BaseOutputDto();
        }

        /// <summary>
        /// 重置员工账号密码
        /// </summary>
        /// <param name="updateEmployeeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto ResetEmployeeAccountPassword(UpdateEmployeeInputDto updateEmployeeInputDto)
        {
            try
            {
                var newPwd = new RandomStringGenerator().GenerateSecurePassword();
                string encrypted = dataProtector.Protect(newPwd);

                var employeeMailAddress = workerRepository.GetSingle(a => a.EmployeeId == updateEmployeeInputDto.EmployeeId).EmailAddress;

                if (employeeMailAddress.IsNullOrEmpty())
                {
                    return new BaseOutputDto()
                    {
                        Message = LocalizationHelper.GetLocalizedString("No bound email address was found for the employee. Password reset cannot be completed."
                        , "未找到员工绑定的电子邮箱，无法重置密码。"),
                        StatusCode = StatusCodeConstants.InternalServerError
                    };
                }

                var Subject = LocalizationHelper.GetLocalizedString("Reset Password Notice", "重置密码通知");
                var Body = $@"<h1>{LocalizationHelper.GetLocalizedString("Dear User,", "尊敬的用户：")}</h1>
                            <p>{LocalizationHelper.GetLocalizedString(
                                $"Your password was reset at <strong>{DateTime.Now:yyyy/MM/dd}</strong>. New password:",
                                $"系统已于<strong>{DateTime.Now:yyyy/MM/dd}</strong>为你重置密码成功，新密码如下：")}
                            </p>
                            <p style='color: #666;'>{newPwd}</p>
                            <p>{LocalizationHelper.GetLocalizedString(
                                "Please keep your password secure and change it after login.",
                                "请妥善保管密码，并在成功登录后修改为你能记住的密码！")}</p>";

                mailHelper.SendMail(new List<string> { employeeMailAddress }, Subject, Body, new List<string> { employeeMailAddress });
                workerRepository.Update(a => new Employee()
                {
                    Password = encrypted,
                    DataChgUsr = updateEmployeeInputDto.DataChgUsr
                }, a => a.EmployeeId == updateEmployeeInputDto.EmployeeId);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }

            return new BaseOutputDto();
        }

        public static string GetDescriptionByName(string enumName)
        {
            Type enumType = typeof(PoliticalAffiliation);

            if (!Enum.IsDefined(enumType, enumName))
            {
                return null;
            }

            FieldInfo field = enumType.GetField(enumName);
            DescriptionAttribute attribute = field?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            string description = attribute?.Description ?? enumName;

            return description;
        }

    }
}
