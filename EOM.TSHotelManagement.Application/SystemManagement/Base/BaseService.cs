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
    /// 基础信息接口实现类
    /// </summary>
    public class BaseService : IBaseService
    {
        /// <summary>
        /// 员工信息
        /// </summary>
        private readonly GenericRepository<Employee> workerRepository;

        /// <summary>
        /// 学历类型
        /// </summary>
        private readonly GenericRepository<Education> educationRepository;

        /// <summary>
        /// 民族类型
        /// </summary>
        private readonly GenericRepository<Nation> nationRepository;

        /// <summary>
        /// 部门
        /// </summary>
        private readonly GenericRepository<Department> deptRepository;

        /// <summary>
        /// 职务
        /// </summary>
        private readonly GenericRepository<Position> positionRepository;

        /// <summary>
        /// 证件类型
        /// </summary>
        private readonly GenericRepository<PassportType> passPortTypeRepository;

        /// <summary>
        /// 客户类型
        /// </summary>
        private readonly GenericRepository<CustoType> custoTypeRepository;

        /// <summary>
        /// 奖惩类型
        /// </summary>
        private readonly GenericRepository<RewardPunishmentType> goodbadTypeRepository;

        /// <summary>
        /// 基础URL
        /// </summary>
        private readonly GenericRepository<SystemInformation> baseRepository;

        /// <summary>
        /// 公告类型
        /// </summary>
        private readonly GenericRepository<AppointmentNoticeType> appointmentNoticeTypeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerRepository"></param>
        /// <param name="educationRepository"></param>
        /// <param name="nationRepository"></param>
        /// <param name="deptRepository"></param>
        /// <param name="positionRepository"></param>
        /// <param name="passPortTypeRepository"></param>
        /// <param name="custoTypeRepository"></param>
        /// <param name="goodbadTypeRepository"></param>
        /// <param name="baseRepository"></param>
        /// <param name="appointmentNoticeTypeRepository"></param>
        public BaseService(GenericRepository<Employee> workerRepository, GenericRepository<Education> educationRepository, GenericRepository<Nation> nationRepository, GenericRepository<Department> deptRepository, GenericRepository<Position> positionRepository, GenericRepository<PassportType> passPortTypeRepository, GenericRepository<CustoType> custoTypeRepository, GenericRepository<RewardPunishmentType> goodbadTypeRepository, GenericRepository<SystemInformation> baseRepository, GenericRepository<AppointmentNoticeType> appointmentNoticeTypeRepository)
        {
            this.workerRepository = workerRepository;
            this.educationRepository = educationRepository;
            this.nationRepository = nationRepository;
            this.deptRepository = deptRepository;
            this.positionRepository = positionRepository;
            this.passPortTypeRepository = passPortTypeRepository;
            this.custoTypeRepository = custoTypeRepository;
            this.goodbadTypeRepository = goodbadTypeRepository;
            this.baseRepository = baseRepository;
            this.appointmentNoticeTypeRepository = appointmentNoticeTypeRepository;
        }

        #region 性别模块

        /// <summary>
        /// 查询所有性别类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<EnumDto> SelectGenderTypeAll()
        {
            var helper = new EnumHelper();
            var enumList = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToList();

            return new ListOutputDto<EnumDto>
            {
                listSource = enumList,
                total = enumList.Count
            };
        }

        #endregion

        #region 面貌模块

        /// <summary>
        /// 查询所有面貌类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<EnumDto> SelectWorkerFeatureAll()
        {
            var helper = new EnumHelper();
            var enumList = Enum.GetValues(typeof(PoliticalAffiliation))
                .Cast<PoliticalAffiliation>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToList();

            return new ListOutputDto<EnumDto>
            {
                listSource = enumList,
                total = enumList.Count
            };
        }

        #endregion

        #region 房间状态模块  
        /// <summary>  
        /// 获取所有房间状态  
        /// </summary>  
        /// <returns></returns>  
        public ListOutputDto<EnumDto> SelectRoomStateAll()
        {
            var helper = new EnumHelper();
            var enumList = Enum.GetValues(typeof(RoomState))
                .Cast<RoomState>()
                .Select(e => new EnumDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    Description = helper.GetEnumDescription(e)
                })
                .ToList();

            return new ListOutputDto<EnumDto>
            {
                listSource = enumList,
                total = enumList.Count
            };
        }
        #endregion

        #region 职位模块

        /// <summary>
        /// 查询所有职位类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadPositionOutputDto> SelectPositionAll(ReadPositionInputDto positionInputDto = null)
        {
            var where = Expressionable.Create<Position>();

            if (positionInputDto != null && !positionInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == positionInputDto.IsDelete);
            }
            if (positionInputDto != null && !positionInputDto.PositionName.IsNullOrEmpty())
            {
                where = where.And(a => a.PositionName.Contains(positionInputDto.PositionName));
            }
            var count = 0;
            var positions = new List<Position>();

            if (!positionInputDto.IgnorePaging && positionInputDto.Page != 0 && positionInputDto.PageSize != 0)
            {
                positions = positionRepository.AsQueryable().Where(where.ToExpression()).ToPageList(positionInputDto.Page, positionInputDto.PageSize, ref count);
            }
            else
            {
                positions = positionRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var result = EntityMapper.MapList<Position, ReadPositionOutputDto>(positions);
            return new ListOutputDto<ReadPositionOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 查询职位类型
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadPositionOutputDto> SelectPosition(ReadPositionInputDto positionInputDto)
        {
            var position = positionRepository.GetSingle(a => a.Id == positionInputDto.PositionId);
            var result = EntityMapper.Map<Position, ReadPositionOutputDto>(position);
            return new SingleOutputDto<ReadPositionOutputDto> { Source = result };
        }

        /// <summary>
        /// 添加职位类型
        /// </summary>
        /// <param name="createPositionInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto AddPosition(CreatePositionInputDto createPositionInputDto)
        {
            var position = EntityMapper.Map<CreatePositionInputDto, Position>(createPositionInputDto);
            var result = positionRepository.Insert(position);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 删除职位类型
        /// </summary>
        /// <param name="deletePositionInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DelPosition(DeletePositionInputDto deletePositionInputDto)
        {
            var result = positionRepository.Update(a => new Position()
            {
                IsDelete = deletePositionInputDto.IsDelete,
                DataChgUsr = deletePositionInputDto.DataChgUsr,
                DataChgDate = deletePositionInputDto.DataChgDate
            }, a => a.PositionNumber == deletePositionInputDto.PositionNumber);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 更新职位类型
        /// </summary>
        /// <param name="updatePositionInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdPosition(UpdatePositionInputDto updatePositionInputDto)
        {
            var result = positionRepository.Update(a => new Position()
            {
                PositionName = updatePositionInputDto.PositionName,
                DataChgUsr = updatePositionInputDto.DataChgUsr,
                DataChgDate = updatePositionInputDto.DataChgDate
            }, a => a.PositionNumber == updatePositionInputDto.PositionNumber);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        #endregion

        #region 民族模块

        /// <summary>
        /// 查询所有民族类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadNationOutputDto> SelectNationAll(ReadNationInputDto nationInputDto)
        {
            var nations = new List<Nation>();

            var where = Expressionable.Create<Nation>();

            if (nationInputDto != null && !nationInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == nationInputDto.IsDelete);
            }
            var count = 0;

            if (!nationInputDto.IgnorePaging && nationInputDto.Page != 0 && nationInputDto.PageSize != 0)
            {
                nations = nationRepository.AsQueryable().Where(where.ToExpression()).ToPageList(nationInputDto.Page, nationInputDto.PageSize, ref count);
            }
            else
            {
                nations = nationRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var result = EntityMapper.MapList<Nation, ReadNationOutputDto>(nations);
            return new ListOutputDto<ReadNationOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 查询民族类型
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadNationOutputDto> SelectNation(ReadNationInputDto nationInputDto)
        {
            var nation = nationRepository.GetSingle(a => a.Id == nationInputDto.NationId);
            var result = EntityMapper.Map<Nation, ReadNationOutputDto>(nation);
            return new SingleOutputDto<ReadNationOutputDto> { Source = result };
        }

        /// <summary>
        /// 添加民族类型
        /// </summary>
        /// <param name="createNationInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto AddNation(CreateNationInputDto createNationInputDto)
        {
            var nation = EntityMapper.Map<CreateNationInputDto, Nation>(createNationInputDto);
            var result = nationRepository.Insert(nation);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 删除民族类型
        /// </summary>
        /// <param name="deleteNationInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DelNation(DeleteNationInputDto deleteNationInputDto)
        {
            var result = nationRepository.Update(a => new Nation()
            {
                IsDelete = deleteNationInputDto.IsDelete,
                DataChgUsr = deleteNationInputDto.DataChgUsr,
                DataChgDate = deleteNationInputDto.DataChgDate
            }, a => a.NationNumber == deleteNationInputDto.NationNumber);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 更新民族类型
        /// </summary>
        /// <param name="updateNationInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdNation(UpdateNationInputDto updateNationInputDto)
        {
            var result = nationRepository.Update(a => new Nation()
            {
                NationName = updateNationInputDto.NationName,
                DataChgUsr = updateNationInputDto.DataChgUsr,
                DataChgDate = updateNationInputDto.DataChgDate
            }, a => a.NationNumber == updateNationInputDto.NationNumber);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        #endregion

        #region 学历模块

        /// <summary>
        /// 查询所有学历类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadEducationOutputDto> SelectEducationAll(ReadEducationInputDto educationInputDto = null)
        {
            var where = Expressionable.Create<Education>();

            if (!educationInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == educationInputDto.IsDelete);
            }
            var count = 0;
            var educations = new List<Education>();

            if (!educationInputDto.IgnorePaging && educationInputDto.Page != 0 && educationInputDto.PageSize != 0)
            {
                educations = educationRepository.AsQueryable().Where(where.ToExpression()).ToPageList(educationInputDto.Page, educationInputDto.PageSize, ref count);
            }
            else
            {
                educations = educationRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var result = EntityMapper.MapList<Education, ReadEducationOutputDto>(educations);
            return new ListOutputDto<ReadEducationOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 查询学历类型
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadEducationOutputDto> SelectEducation(ReadEducationInputDto educationInputDto)
        {
            var education = educationRepository.GetSingle(a => a.EducationNumber == educationInputDto.EducationNumber);
            var result = EntityMapper.Map<Education, ReadEducationOutputDto>(education);
            return new SingleOutputDto<ReadEducationOutputDto> { Source = result };
        }

        /// <summary>
        /// 添加学历类型
        /// </summary>
        /// <param name="createEducationInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto AddEducation(CreateEducationInputDto createEducationInputDto)
        {
            var education = EntityMapper.Map<CreateEducationInputDto, Education>(createEducationInputDto);
            var result = educationRepository.Insert(education);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 删除学历类型
        /// </summary>
        /// <param name="deleteEducationInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DelEducation(DeleteEducationInputDto deleteEducationInputDto)
        {
            var result = educationRepository.Update(a => new Education()
            {
                IsDelete = deleteEducationInputDto.IsDelete,
                DataChgUsr = deleteEducationInputDto.DataChgUsr,
                DataChgDate = deleteEducationInputDto.DataChgDate
            }, a => a.EducationNumber == deleteEducationInputDto.EducationNumber);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 更新学历类型
        /// </summary>
        /// <param name="education"></param>
        /// <returns></returns>
        public BaseOutputDto UpdEducation(UpdateEducationInputDto education)
        {
            var result = educationRepository.Update(a => new Education()
            {
                EducationName = education.EducationName,
                DataChgUsr = education.DataChgUsr,
                DataChgDate = education.DataChgDate
            }, a => a.EducationNumber == education.EducationNumber);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        #endregion

        #region 部门模块

        /// <summary>
        /// 查询所有部门类型(可用)
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadDepartmentOutputDto> SelectDeptAllCanUse()
        {
            var depts = deptRepository.GetList(a => a.IsDelete != 1);
            var result = EntityMapper.MapList<Department, ReadDepartmentOutputDto>(depts);
            return new ListOutputDto<ReadDepartmentOutputDto> { listSource = result, total = result.Count };
        }

        /// <summary>
        /// 查询所有部门类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadDepartmentOutputDto> SelectDeptAll(ReadDepartmentInputDto readDepartmentInputDto)
        {
            var where = Expressionable.Create<Department>();
            if (!readDepartmentInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readDepartmentInputDto.IsDelete);
            }
            var count = 0;
            var depts = new List<Department>();

            if (!readDepartmentInputDto.IgnorePaging && readDepartmentInputDto.Page != 0 && readDepartmentInputDto.PageSize != 0)
            {
                depts = deptRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readDepartmentInputDto.Page, readDepartmentInputDto.PageSize, ref count);
            }
            else
            {
                depts = deptRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var parentDepartmentNumbers = depts.Where(a => !a.ParentDepartmentNumber.IsNullOrEmpty()).Select(a => a.ParentDepartmentNumber).ToList();
            var parentDepartments = deptRepository.GetList(a => parentDepartmentNumbers.Contains(a.DepartmentNumber));
            var departmentLeaderNumbers = depts.Where(a => !a.DepartmentLeader.IsNullOrEmpty()).Select(a => a.DepartmentLeader).ToList();
            var departmentLeaders = workerRepository.GetList(a => departmentLeaderNumbers.Contains(a.EmployeeId));

            depts.ForEach(source =>
            {
                var parentDepartment = parentDepartments.SingleOrDefault(a => a.DepartmentNumber.Equals(source.ParentDepartmentNumber));
                source.ParentDepartmentName = parentDepartment.IsNullOrEmpty() ? "" : parentDepartment.DepartmentName;

                var departmentLeader = departmentLeaders.SingleOrDefault(a => a.EmployeeId.Equals(source.DepartmentLeader));
                source.LeaderName = departmentLeader.IsNullOrEmpty() ? "" : departmentLeader.EmployeeName;
            });
            var result = EntityMapper.MapList<Department, ReadDepartmentOutputDto>(depts);
            return new ListOutputDto<ReadDepartmentOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 查询部门类型
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadDepartmentOutputDto> SelectDept(ReadDepartmentInputDto dept)
        {
            var department = deptRepository.GetSingle(a => a.Id == dept.Id);
            var result = EntityMapper.Map<Department, ReadDepartmentOutputDto>(department);
            return new SingleOutputDto<ReadDepartmentOutputDto> { Source = result };
        }

        /// <summary>
        /// 添加部门类型
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public BaseOutputDto AddDept(CreateDepartmentInputDto dept)
        {
            var department = EntityMapper.Map<CreateDepartmentInputDto, Department>(dept);
            var result = deptRepository.Insert(department);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 删除部门类型
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public BaseOutputDto DelDept(DeleteDepartmentInputDto dept)
        {
            var result = deptRepository.Update(a => new Department()
            {
                IsDelete = dept.IsDelete,
                DataChgUsr = dept.DataChgUsr,
                DataChgDate = dept.DataChgDate
            }, a => a.Id == dept.Id);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 更新部门类型
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public BaseOutputDto UpdDept(UpdateDepartmentInputDto dept)
        {
            var result = deptRepository.Update(a => new Department()
            {
                DepartmentName = dept.DepartmentName,
                DepartmentDescription = dept.DepartmentDescription,
                DepartmentLeader = dept.DepartmentLeader,
                ParentDepartmentNumber = dept.ParentDepartmentNumber,
                DepartmentCreationDate = dept.DepartmentCreationDate,
                DataChgUsr = dept.DataChgUsr,
                DataChgDate = dept.DataChgDate
            }, a => a.DepartmentNumber == dept.DepartmentNumber);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        #endregion

        #region 客户类型模块

        /// <summary>
        /// 查询所有客户类型(可用)
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadCustoTypeOutputDto> SelectCustoTypeAllCanUse()
        {
            var custoTypes = custoTypeRepository.GetList(a => a.IsDelete != 1);
            var result = EntityMapper.MapList<CustoType, ReadCustoTypeOutputDto>(custoTypes);
            return new ListOutputDto<ReadCustoTypeOutputDto> { listSource = result, total = result.Count };
        }

        /// <summary>
        /// 查询所有客户类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadCustoTypeOutputDto> SelectCustoTypeAll(ReadCustoTypeInputDto readCustoTypeInputDto)
        {
            var where = Expressionable.Create<CustoType>();
            if (!readCustoTypeInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readCustoTypeInputDto.IsDelete);
            }
            var count = 0;
            var custoTypes = new List<CustoType>();

            if (!readCustoTypeInputDto.IgnorePaging && readCustoTypeInputDto.Page != 0 && readCustoTypeInputDto.PageSize != 0)
            {
                custoTypes = custoTypeRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readCustoTypeInputDto.Page, readCustoTypeInputDto.PageSize, ref count);
            }
            else
            {
                custoTypes = custoTypeRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var result = EntityMapper.MapList<CustoType, ReadCustoTypeOutputDto>(custoTypes);
            return new ListOutputDto<ReadCustoTypeOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 根据客户类型ID查询类型名称
        /// </summary>
        /// <param name="custoType"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadCustoTypeOutputDto> SelectCustoTypeByTypeId(ReadCustoTypeInputDto custoType)
        {
            var custoTypeEntity = custoTypeRepository.GetSingle(a => a.CustomerType == custoType.CustomerType && a.IsDelete != 1);
            var result = EntityMapper.Map<CustoType, ReadCustoTypeOutputDto>(custoTypeEntity);
            return new SingleOutputDto<ReadCustoTypeOutputDto> { Source = result };
        }

        /// <summary>
        /// 添加客户类型
        /// </summary>
        /// <param name="custoType"></param>
        /// <returns></returns>
        public BaseOutputDto InsertCustoType(CreateCustoTypeInputDto custoType)
        {
            var custoTypeEntity = EntityMapper.Map<CreateCustoTypeInputDto, CustoType>(custoType);
            var result = custoTypeRepository.Insert(custoTypeEntity);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 删除客户类型
        /// </summary>
        /// <param name="custoType"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteCustoType(DeleteCustoTypeInputDto custoType)
        {
            var result = custoTypeRepository.Update(a => new CustoType()
            {
                IsDelete = 1,
                DataChgUsr = custoType.DataChgUsr
            }, a => a.CustomerType == custoType.CustomerType);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 更新客户类型
        /// </summary>
        /// <param name="custoType"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateCustoType(UpdateCustoTypeInputDto custoType)
        {
            var result = custoTypeRepository.Update(a => new CustoType()
            {
                CustomerTypeName = custoType.CustomerTypeName,
                Discount = custoType.Discount,
                DataChgUsr = custoType.DataChgUsr,
                DataChgDate = custoType.DataChgDate
            }, a => a.CustomerType == custoType.CustomerType);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        #endregion

        #region 证件类型模块

        /// <summary>
        /// 查询所有证件类型(可用)
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadPassportTypeOutputDto> SelectPassPortTypeAllCanUse()
        {
            var passPortTypes = passPortTypeRepository.GetList(a => a.IsDelete != 1);
            var result = EntityMapper.MapList<PassportType, ReadPassportTypeOutputDto>(passPortTypes);
            return new ListOutputDto<ReadPassportTypeOutputDto> { listSource = result, total = result.Count };
        }

        /// <summary>
        /// 查询所有证件类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadPassportTypeOutputDto> SelectPassPortTypeAll(ReadPassportTypeInputDto readPassportTypeInputDto)
        {
            var where = Expressionable.Create<PassportType>();
            if (!readPassportTypeInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readPassportTypeInputDto.IsDelete);
            }
            var count = 0;
            var passPortTypes = new List<PassportType>();

            if (!readPassportTypeInputDto.IgnorePaging && readPassportTypeInputDto.Page != 0 && readPassportTypeInputDto.PageSize != 0)
            {
                passPortTypes = passPortTypeRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readPassportTypeInputDto.Page, readPassportTypeInputDto.PageSize, ref count);
            }
            else
            {
                passPortTypes = passPortTypeRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var result = EntityMapper.MapList<PassportType, ReadPassportTypeOutputDto>(passPortTypes);
            return new ListOutputDto<ReadPassportTypeOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 根据证件类型ID查询类型名称
        /// </summary>
        /// <param name="passPortType"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadPassportTypeOutputDto> SelectPassPortTypeByTypeId(ReadPassportTypeInputDto passPortType)
        {
            var passPortTypeEntity = passPortTypeRepository.GetSingle(a => a.PassportId == passPortType.PassportId && a.IsDelete != 1);
            var result = EntityMapper.Map<PassportType, ReadPassportTypeOutputDto>(passPortTypeEntity);
            return new SingleOutputDto<ReadPassportTypeOutputDto> { Source = result };
        }

        /// <summary>
        /// 添加证件类型
        /// </summary>
        /// <param name="passPortType"></param>
        /// <returns></returns>
        public BaseOutputDto InsertPassPortType(CreatePassportTypeInputDto passPortType)
        {
            var passPortTypeEntity = EntityMapper.Map<CreatePassportTypeInputDto, PassportType>(passPortType);
            var result = passPortTypeRepository.Insert(passPortTypeEntity);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 删除证件类型
        /// </summary>
        /// <param name="portType"></param>
        /// <returns></returns>
        public BaseOutputDto DeletePassPortType(DeletePassportTypeInputDto portType)
        {
            var result = passPortTypeRepository.Update(a => new PassportType()
            {
                IsDelete = 1,
                DataChgUsr = portType.DataChgUsr,
            }, a => a.PassportId == portType.PassportId);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 更新证件类型
        /// </summary>
        /// <param name="portType"></param>
        /// <returns></returns>
        public BaseOutputDto UpdatePassPortType(UpdatePassportTypeInputDto portType)
        {
            var result = passPortTypeRepository.Update(a => new PassportType()
            {
                PassportName = portType.PassportName,
                DataChgUsr = portType.DataChgUsr,
                DataChgDate = portType.DataChgDate
            }, a => a.PassportId == portType.PassportId);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        #endregion

        #region 奖惩类型模块

        /// <summary>
        /// 查询所有奖惩类型(可用)
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRewardPunishmentTypeOutputDto> SelectRewardPunishmentTypeAllCanUse()
        {
            var gBTypes = goodbadTypeRepository.GetList(a => a.IsDelete != 1);
            var result = EntityMapper.MapList<RewardPunishmentType, ReadRewardPunishmentTypeOutputDto>(gBTypes);
            return new ListOutputDto<ReadRewardPunishmentTypeOutputDto> { listSource = result, total = result.Count };
        }

        /// <summary>
        /// 查询所有奖惩类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadRewardPunishmentTypeOutputDto> SelectRewardPunishmentTypeAll(ReadRewardPunishmentTypeInputDto readRewardPunishmentTypeInputDto)
        {
            var where = Expressionable.Create<RewardPunishmentType>();
            if (!readRewardPunishmentTypeInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readRewardPunishmentTypeInputDto.IsDelete);
            }
            var count = 0;
            var gBTypes = new List<RewardPunishmentType>();

            if (!readRewardPunishmentTypeInputDto.IgnorePaging && readRewardPunishmentTypeInputDto.Page != 0 && readRewardPunishmentTypeInputDto.PageSize != 0)
            {
                gBTypes = goodbadTypeRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readRewardPunishmentTypeInputDto.Page, readRewardPunishmentTypeInputDto.PageSize, ref count);
            }
            else
            {
                gBTypes = goodbadTypeRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var result = EntityMapper.MapList<RewardPunishmentType, ReadRewardPunishmentTypeOutputDto>(gBTypes);
            return new ListOutputDto<ReadRewardPunishmentTypeOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 根据奖惩类型ID查询类型名称
        /// </summary>
        /// <param name="gBType"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadRewardPunishmentTypeOutputDto> SelectRewardPunishmentTypeByTypeId(ReadRewardPunishmentTypeInputDto gBType)
        {
            var gBTypeEntity = goodbadTypeRepository.GetSingle(a => a.RewardPunishmentTypeId == gBType.RewardPunishmentTypeId && a.IsDelete != 1);
            var result = EntityMapper.Map<RewardPunishmentType, ReadRewardPunishmentTypeOutputDto>(gBTypeEntity);
            return new SingleOutputDto<ReadRewardPunishmentTypeOutputDto> { Source = result };
        }

        /// <summary>
        /// 添加奖惩类型
        /// </summary>
        /// <param name="gBType"></param>
        /// <returns></returns>
        public BaseOutputDto InsertRewardPunishmentType(CreateRewardPunishmentTypeInputDto gBType)
        {
            var gBTypeEntity = EntityMapper.Map<CreateRewardPunishmentTypeInputDto, RewardPunishmentType>(gBType);
            var result = goodbadTypeRepository.Insert(gBTypeEntity);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 删除奖惩类型
        /// </summary>
        /// <param name="gBType"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteRewardPunishmentType(DeleteRewardPunishmentTypeInputDto gBType)
        {
            var result = goodbadTypeRepository.Update(a => new RewardPunishmentType()
            {
                IsDelete = 1,
                DataChgUsr = gBType.DataChgUsr,
            }, a => a.RewardPunishmentTypeId == gBType.RewardPunishmentTypeId);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        /// <summary>
        /// 更新奖惩类型
        /// </summary>
        /// <param name="gBType"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateRewardPunishmentType(UpdateRewardPunishmentTypeInputDto gBType)
        {
            var result = goodbadTypeRepository.Update(a => new RewardPunishmentType()
            {
                RewardPunishmentTypeName = gBType.RewardPunishmentTypeName,
                DataChgUsr = gBType.DataChgUsr,
                DataChgDate = gBType.DataChgDate
            }, a => a.RewardPunishmentTypeId == gBType.RewardPunishmentTypeId);
            return new BaseOutputDto { StatusCode = result ? StatusCodeConstants.Success : StatusCodeConstants.InternalServerError };
        }

        #endregion

        #region URL模块

        /// <summary>
        /// 基础URL
        /// </summary>
        /// <returns></returns>
        public SingleOutputDto<ReadSystemInformationOutputDto> GetBase()
        {
            var baseTemp = baseRepository.GetSingle(a => a.UrlNumber == 1);
            var result = EntityMapper.Map<SystemInformation, ReadSystemInformationOutputDto>(baseTemp);
            return new SingleOutputDto<ReadSystemInformationOutputDto> { Source = result };
        }

        #endregion

        #region 公告类型模块

        /// <summary>
        /// 查询所有公告类型
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadAppointmentNoticeTypeOutputDto> SelectAppointmentNoticeTypeAll(ReadAppointmentNoticeTypeInputDto readAppointmentNoticeTypeInputDto)
        {
            var listSource = new List<AppointmentNoticeType>();

            var where = Expressionable.Create<AppointmentNoticeType>();

            if (!readAppointmentNoticeTypeInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readAppointmentNoticeTypeInputDto.IsDelete);
            }

            var count = 0;

            if (!readAppointmentNoticeTypeInputDto.IgnorePaging && readAppointmentNoticeTypeInputDto.Page != 0 && readAppointmentNoticeTypeInputDto.PageSize != 0)
            {
                listSource = appointmentNoticeTypeRepository.AsQueryable().Where(where.ToExpression())
                    .ToPageList(readAppointmentNoticeTypeInputDto.Page, readAppointmentNoticeTypeInputDto.PageSize, ref count);
            }
            else
            {
                listSource = appointmentNoticeTypeRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            var result = EntityMapper.MapList<AppointmentNoticeType, ReadAppointmentNoticeTypeOutputDto>(listSource);

            return new ListOutputDto<ReadAppointmentNoticeTypeOutputDto> { listSource = result, total = count };
        }

        /// <summary>
        /// 添加公告类型
        /// </summary>
        /// <param name="createAppointmentNoticeTypeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto CreateAppointmentNoticeType(CreateAppointmentNoticeTypeInputDto createAppointmentNoticeTypeInputDto)
        {
            try
            {
                if (appointmentNoticeTypeRepository.IsAny(a => a.NoticeTypeNumber == createAppointmentNoticeTypeInputDto.NoticeTypeNumber))
                {
                    return new BaseOutputDto { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString("appointment notice number already exits.","公告类型编号已存在") };
                }
                appointmentNoticeTypeRepository.Insert(EntityMapper.Map<CreateAppointmentNoticeTypeInputDto, AppointmentNoticeType>(createAppointmentNoticeTypeInputDto));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("insert appointment notice failed.", "公告类型添加失败"), ex);
                return new BaseOutputDto { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString("insert appointment notice failed.", "公告类型添加失败") };
            }

            return new BaseOutputDto { StatusCode = StatusCodeConstants.Success, Message = LocalizationHelper.GetLocalizedString("insert appointment notice successful.", "公告类型添加成功") };
        }

        /// <summary>
        /// 删除公告类型
        /// </summary>
        /// <param name="deleteAppointmentNoticeTypeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteAppointmentNoticeType(DeleteAppointmentNoticeTypeInputDto deleteAppointmentNoticeTypeInputDto)
        {
            try
            {
                if (!appointmentNoticeTypeRepository.IsAny(a => a.NoticeTypeNumber == deleteAppointmentNoticeTypeInputDto.NoticeTypeNumber))
                {
                    return new BaseOutputDto { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString("appointment notice number does not already.", "公告类型编号不存在") };
                }
                appointmentNoticeTypeRepository.Update(a => new AppointmentNoticeType 
                {
                    IsDelete = 1,
                    DataChgUsr = deleteAppointmentNoticeTypeInputDto.DataChgUsr,
                    DataChgDate = deleteAppointmentNoticeTypeInputDto.DataChgDate
                }, a => a.NoticeTypeNumber == deleteAppointmentNoticeTypeInputDto.NoticeTypeNumber);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("delete appointment notice failed.", "公告类型删除失败"), ex);
                return new BaseOutputDto { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString("delete appointment notice failed.", "公告类型删除失败") };
            }

            return new BaseOutputDto { StatusCode = StatusCodeConstants.Success, Message = LocalizationHelper.GetLocalizedString("delete appointment notice successful.", "公告类型删除成功") };
        }

        /// <summary>
        /// 更新公告类型
        /// </summary>
        /// <param name="updateAppointmentNoticeTypeInputDto"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateAppointmentNoticeType(UpdateAppointmentNoticeTypeInputDto updateAppointmentNoticeTypeInputDto)
        {
            try
            {
                if (!appointmentNoticeTypeRepository.IsAny(a => a.NoticeTypeNumber == updateAppointmentNoticeTypeInputDto.NoticeTypeNumber))
                {
                    return new BaseOutputDto { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString("appointment notice number does not already.", "公告类型编号不存在") };
                }
                appointmentNoticeTypeRepository.Update(EntityMapper.Map<UpdateAppointmentNoticeTypeInputDto, AppointmentNoticeType>(updateAppointmentNoticeTypeInputDto));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("update appointment notice failed.", "公告类型更新失败"), ex);
                return new BaseOutputDto { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString("update appointment notice failed.", "公告类型更新失败") };
            }

            return new BaseOutputDto { StatusCode = StatusCodeConstants.Success, Message = LocalizationHelper.GetLocalizedString("update appointment notice successful.", "公告类型更新成功") };
        }

        #endregion
    }
}
