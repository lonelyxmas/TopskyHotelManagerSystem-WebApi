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
using jvncorelib.EncryptorLib;
using jvncorelib.EntityLib;
using Microsoft.AspNetCore.DataProtection;
using SqlSugar;
using System.Security.Claims;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 管理员数据访问层
    /// </summary>
    public class AdminService : IAdminService
    {
        /// <summary>
        /// 管理员
        /// </summary>
        private readonly GenericRepository<Administrator> adminRepository;

        /// <summary>
        /// 管理员类型
        /// </summary>
        private readonly GenericRepository<AdministratorType> adminTypeRepository;

        /// <summary>
        /// 数据保护
        /// </summary>
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// 加密
        /// </summary>
        private readonly EncryptLib encrypt;

        /// <summary>
        /// JWT加密
        /// </summary>
        private readonly JWTHelper jWTHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminRepository"></param>
        /// <param name="adminTypeRepository"></param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="encrypt"></param>
        /// <param name="jWTHelper"></param>
        public AdminService(GenericRepository<Administrator> adminRepository, GenericRepository<AdministratorType> adminTypeRepository, IDataProtectionProvider dataProtectionProvider, EncryptLib encrypt, JWTHelper jWTHelper)
        {
            this.adminRepository = adminRepository;
            this.adminTypeRepository = adminTypeRepository;
            this.dataProtector = dataProtectionProvider.CreateProtector("AdminInfoProtector");
            this.encrypt = encrypt;
            this.jWTHelper = jWTHelper;
        }

        /// <summary>
        /// 根据超管密码查询员工类型和权限
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadAdministratorOutputDto> SelectManagerByPass(ReadAdministratorInputDto readAdministratorInputDto)
        {
            if (readAdministratorInputDto == null)
            {
                throw new ArgumentNullException(nameof(readAdministratorInputDto));
            }

            var existingAdmin = adminRepository.GetSingle(a => a.Account == readAdministratorInputDto.Account);

            if (existingAdmin == null)
            {
                return null;
            }

            string existingAdminPassword = existingAdmin.Password;

            if (existingAdminPassword.Contains("·"))
            {
                if (!encrypt.Compare(readAdministratorInputDto.Password, existingAdminPassword))
                {
                    return null;
                }
            }
            else if (!readAdministratorInputDto.Password.Equals(existingAdminPassword))
            {
                return null;
            }

            if (readAdministratorInputDto.IsDelete == 1)
            {
                return null;
            }

            existingAdmin.Password = string.Empty;

            existingAdmin.UserToken = jWTHelper.GenerateJWT(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, existingAdmin.Name),
                new Claim(ClaimTypes.SerialNumber, existingAdmin.Number)
            }));

            var source = EntityMapper.Map<Administrator, ReadAdministratorOutputDto>(existingAdmin);

            return new SingleOutputDto<ReadAdministratorOutputDto>
            {
                Source = source
            };
        }

        /// <summary>
        /// 后台系统登录
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadAdministratorOutputDto> Login(ReadAdministratorInputDto readAdministratorInputDto)
        {
            if (readAdministratorInputDto == null)
            {
                throw new ArgumentNullException(nameof(readAdministratorInputDto));
            }

            var existingAdmin = adminRepository.GetSingle(a => a.Account == readAdministratorInputDto.Account);

            if (existingAdmin == null)
            {
                return null;
            }

            if (existingAdmin.IsDelete == 1)
            {
                return null;
            }


            var encrtptPwd = string.Empty;
            try
            {
                encrtptPwd = encrypt.Encryption(readAdministratorInputDto.Password, EncryptionLevel.Enhanced);
                var passed = encrypt.Compare(encrtptPwd, existingAdmin.Password);
                if (!passed)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                var originalPwd = dataProtector.Unprotect(existingAdmin.Password);
                var passed = originalPwd == existingAdmin.Password;
                if (!passed)
                {
                    return null;
                }
            }

            if (encrtptPwd.IsNullOrEmpty() || existingAdmin.Password.IsNullOrEmpty())
            {
                return null;
            }

            existingAdmin.Password = string.Empty;
            existingAdmin.UserToken = jWTHelper.GenerateJWT(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, existingAdmin.Name),
                new Claim(ClaimTypes.SerialNumber, existingAdmin.Account)
            }));

            var source = EntityMapper.Map<Administrator, ReadAdministratorOutputDto>(existingAdmin);

            return new SingleOutputDto<ReadAdministratorOutputDto>
            {
                Source = source
            };
        }

        /// <summary>
        /// 根据超管账号查询对应的密码
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadAdministratorOutputDto> SelectAdminPwdByAccount(ReadAdministratorInputDto readAdministratorInputDto)
        {
            Administrator admin = new Administrator();
            admin = adminRepository.GetSingle(a => a.Account == readAdministratorInputDto.Account);

            var source = EntityMapper.Map<Administrator, ReadAdministratorOutputDto>(admin);

            return new SingleOutputDto<ReadAdministratorOutputDto>
            {
                Source = source
            };
        }

        /// <summary>
        /// 获取所有管理员列表
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadAdministratorOutputDto> GetAllAdminList(ReadAdministratorInputDto readAdministratorInputDto)
        {
            var where = Expressionable.Create<Administrator>();

            if (!readAdministratorInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readAdministratorInputDto.IsDelete);
            }
            var count = 0;
            var listAdmins = new List<Administrator>();
            var listAdminType = adminTypeRepository.GetList(a => a.IsDelete != 1);

            if (!readAdministratorInputDto.IgnorePaging && readAdministratorInputDto.Page != 0 && readAdministratorInputDto.PageSize != 0)
            {
                listAdmins = adminRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readAdministratorInputDto.Page, readAdministratorInputDto.PageSize, ref count);
            }
            else
            {
                listAdmins = adminRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            listAdmins.ForEach(admins =>
            {
                var isAdminType = admins.IsSuperAdmin == 1 ? "是" : "否";
                admins.IsSuperAdminDescription = isAdminType;

                var adminType = listAdminType.SingleOrDefault(a => a.TypeId.Equals(admins.Type));
                admins.TypeName = adminType == null ? "" : adminType.TypeName;

                var adminDelete = admins.IsDelete == 1 ? "是" : "否";
                admins.DeleteDescription = adminDelete;

            });

            var listSouce = EntityMapper.MapList<Administrator, ReadAdministratorOutputDto>(listAdmins);

            return new ListOutputDto<ReadAdministratorOutputDto>
            {
                listSource = listSouce,
                total = count
            };
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateNewPwdByOldPwd(UpdateAdministratorInputDto updateAdministratorInputDto)
        {
            try
            {
                updateAdministratorInputDto.Password = encrypt.Encryption(updateAdministratorInputDto.Password, EncryptionLevel.Enhanced);

                adminRepository.Update(a => new Administrator()
                {
                    Password = updateAdministratorInputDto.Password
                }, a => a.Account == updateAdministratorInputDto.Account);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="createAdministratorInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto AddAdmin(CreateAdministratorInputDto createAdministratorInputDto)
        {
            try
            {
                createAdministratorInputDto.Password = dataProtector.Protect(createAdministratorInputDto.Password);
                adminRepository.Insert(EntityMapper.Map<CreateAdministratorInputDto, Administrator>(createAdministratorInputDto));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdAdmin(UpdateAdministratorInputDto updateAdministratorInputDto)
        {
            try
            {
                updateAdministratorInputDto.Password = dataProtector.Protect(updateAdministratorInputDto.Password);
                adminRepository.Insert(EntityMapper.Map<UpdateAdministratorInputDto, Administrator>(updateAdministratorInputDto));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="deleteAdministratorInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DelAdmin(DeleteAdministratorInputDto deleteAdministratorInputDto)
        {
            try
            {
                adminRepository.Delete(EntityMapper.Map<DeleteAdministratorInputDto, Administrator>(deleteAdministratorInputDto));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="readAdministratorInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadAdministratorOutputDto> GetAdminInfoByAdminAccount(ReadAdministratorInputDto readAdministratorInputDto)
        {
            var adminInfo = adminRepository.GetSingle(a => a.Account.Equals(readAdministratorInputDto.Account));
            if (adminInfo != null)
            {
                var adminType = adminTypeRepository.GetSingle(a => a.TypeId.Equals(adminInfo.Type));
                adminInfo.TypeName = adminType.TypeName;
            }

            var source = EntityMapper.Map<Administrator, ReadAdministratorOutputDto>(adminInfo);

            return new SingleOutputDto<ReadAdministratorOutputDto> { Source = source };
        }

        /// <summary>
        /// 获取所有管理员类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadAdministratorTypeOutputDto> GetAllAdminTypes(ReadAdministratorTypeInputDto readAdministratorTypeInputDto)
        {
            var listAdminTypes = adminTypeRepository.GetList(a => a.IsDelete != 1);

            var listSouce = EntityMapper.MapList<AdministratorType, ReadAdministratorTypeOutputDto>(listAdminTypes);

            return new ListOutputDto<ReadAdministratorTypeOutputDto>
            {
                listSource = listSouce
            };
        }

        /// <summary>
        /// 添加管理员类型
        /// </summary>
        /// <param name="createAdministratorTypeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto AddAdminType(CreateAdministratorTypeInputDto createAdministratorTypeInputDto)
        {
            try
            {
                adminTypeRepository.Insert(EntityMapper.Map<CreateAdministratorTypeInputDto, AdministratorType>(createAdministratorTypeInputDto));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新管理员类型
        /// </summary>
        /// <param name="updateAdministratorTypeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdAdminType(UpdateAdministratorTypeInputDto updateAdministratorTypeInputDto)
        {
            try
            {
                adminTypeRepository.Insert(EntityMapper.Map<UpdateAdministratorTypeInputDto, AdministratorType>(updateAdministratorTypeInputDto));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除管理员类型
        /// </summary>
        /// <param name="deleteAdministratorTypeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DelAdminType(DeleteAdministratorTypeInputDto deleteAdministratorTypeInputDto)
        {
            try
            {
                adminTypeRepository.Delete(EntityMapper.Map<DeleteAdministratorTypeInputDto, AdministratorType>(deleteAdministratorTypeInputDto));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新管理员账户
        /// </summary>
        /// <param name="updateAdministratorInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdAccount(UpdateAdministratorInputDto updateAdministratorInputDto)
        {
            try
            {
                updateAdministratorInputDto.IsDelete = updateAdministratorInputDto.IsDelete == 0 ? 1 : 0;
                adminRepository.Update(a => new Administrator()
                {
                    IsDelete = updateAdministratorInputDto.IsDelete,
                    DataChgDate = updateAdministratorInputDto.DataChgDate
                }, a => a.Account == updateAdministratorInputDto.Account);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
    }
}
