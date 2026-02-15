using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 基础信息控制器
    /// </summary>
    public class BaseController : ControllerBase
    {
        private readonly IBaseService baseService;

        public BaseController(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        #region 预约类型模块

        /// <summary>
        /// 查询所有预约类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<EnumDto> SelectReserTypeAll()
        {
            return baseService.SelectReserTypeAll();
        }

        #endregion

        #region 性别模块

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<EnumDto> SelectGenderTypeAll()
        {
            return baseService.SelectGenderTypeAll();
        }
        #endregion

        #region 面貌模块

        [HttpGet]
        public ListOutputDto<EnumDto> SelectWorkerFeatureAll()
        {
            return baseService.SelectWorkerFeatureAll();
        }

        #endregion

        #region 房间状态模块
        /// <summary>
        /// 获取所有房间状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<EnumDto> SelectRoomStateAll()
        {
            return baseService.SelectRoomStateAll();
        }
        #endregion

        #region 职位模块

        [HttpGet]
        public ListOutputDto<ReadPositionOutputDto> SelectPositionAll([FromQuery] ReadPositionInputDto position = null)
        {
            return baseService.SelectPositionAll(position);
        }

        [HttpGet]
        public SingleOutputDto<ReadPositionOutputDto> SelectPosition([FromQuery] ReadPositionInputDto position)
        {
            return baseService.SelectPosition(position);
        }

        [HttpPost]
        public BaseResponse AddPosition([FromBody] CreatePositionInputDto position)
        {
            return baseService.AddPosition(position);
        }

        [HttpPost]
        public BaseResponse DelPosition([FromBody] DeletePositionInputDto position)
        {
            return baseService.DelPosition(position);
        }

        [HttpPost]
        public BaseResponse UpdPosition([FromBody] UpdatePositionInputDto position)
        {
            return baseService.UpdPosition(position);
        }

        #endregion

        #region 民族模块

        [HttpGet]
        public ListOutputDto<ReadNationOutputDto> SelectNationAll([FromQuery] ReadNationInputDto nation = null)
        {
            return baseService.SelectNationAll(nation);
        }

        [HttpGet]
        public SingleOutputDto<ReadNationOutputDto> SelectNation([FromQuery] ReadNationInputDto nation)
        {
            return baseService.SelectNation(nation);
        }

        [HttpPost]
        public BaseResponse AddNation([FromBody] CreateNationInputDto nation)
        {
            return baseService.AddNation(nation);
        }

        [HttpPost]
        public BaseResponse DelNation([FromBody] DeleteNationInputDto nation)
        {
            return baseService.DelNation(nation);
        }

        [HttpPost]
        public BaseResponse UpdNation([FromBody] UpdateNationInputDto nation)
        {
            return baseService.UpdNation(nation);
        }

        #endregion

        #region 学历模块

        [HttpGet]
        public ListOutputDto<ReadEducationOutputDto> SelectEducationAll([FromQuery] ReadEducationInputDto education = null)
        {
            return baseService.SelectEducationAll(education);
        }

        [HttpGet]
        public SingleOutputDto<ReadEducationOutputDto> SelectEducation([FromQuery] ReadEducationInputDto education)
        {
            return baseService.SelectEducation(education);
        }

        [HttpPost]
        public BaseResponse AddEducation([FromBody] CreateEducationInputDto education)
        {
            return baseService.AddEducation(education);
        }

        [HttpPost]
        public BaseResponse DelEducation([FromBody] DeleteEducationInputDto education)
        {
            return baseService.DelEducation(education);
        }

        [HttpPost]
        public BaseResponse UpdEducation([FromBody] UpdateEducationInputDto education)
        {
            return baseService.UpdEducation(education);
        }

        #endregion

        #region 部门模块

        [HttpGet]
        public ListOutputDto<ReadDepartmentOutputDto> SelectDeptAllCanUse()
        {
            return baseService.SelectDeptAllCanUse();
        }

        [HttpGet]
        public ListOutputDto<ReadDepartmentOutputDto> SelectDeptAll([FromQuery] ReadDepartmentInputDto readDepartmentInputDto)
        {
            return baseService.SelectDeptAll(readDepartmentInputDto);
        }

        [HttpGet]
        public SingleOutputDto<ReadDepartmentOutputDto> SelectDept([FromQuery] ReadDepartmentInputDto dept)
        {
            return baseService.SelectDept(dept);
        }

        [HttpPost]
        public BaseResponse AddDept([FromBody] CreateDepartmentInputDto dept)
        {
            return baseService.AddDept(dept);
        }

        [HttpPost]
        public BaseResponse DelDept([FromBody] DeleteDepartmentInputDto dept)
        {
            return baseService.DelDept(dept);
        }

        [HttpPost]
        public BaseResponse UpdDept([FromBody] UpdateDepartmentInputDto dept)
        {
            return baseService.UpdDept(dept);
        }

        #endregion

        #region 客户类型模块

        [HttpGet]
        public ListOutputDto<ReadCustoTypeOutputDto> SelectCustoTypeAllCanUse()
        {
            return baseService.SelectCustoTypeAllCanUse();
        }

        [HttpGet]
        public ListOutputDto<ReadCustoTypeOutputDto> SelectCustoTypeAll([FromQuery] ReadCustoTypeInputDto readCustoTypeInputDto)
        {
            return baseService.SelectCustoTypeAll(readCustoTypeInputDto);
        }

        [HttpGet]
        public SingleOutputDto<ReadCustoTypeOutputDto> SelectCustoTypeByTypeId([FromQuery] ReadCustoTypeInputDto custoType)
        {
            return baseService.SelectCustoTypeByTypeId(custoType);
        }

        [HttpPost]
        public BaseResponse InsertCustoType([FromBody] CreateCustoTypeInputDto custoType)
        {
            return baseService.InsertCustoType(custoType);
        }

        [HttpPost]
        public BaseResponse DeleteCustoType([FromBody] DeleteCustoTypeInputDto custoType)
        {
            return baseService.DeleteCustoType(custoType);
        }

        [HttpPost]
        public BaseResponse UpdateCustoType([FromBody] UpdateCustoTypeInputDto custoType)
        {
            return baseService.UpdateCustoType(custoType);
        }

        #endregion

        #region 证件类型模块

        [HttpGet]
        public ListOutputDto<ReadPassportTypeOutputDto> SelectPassPortTypeAllCanUse()
        {
            return baseService.SelectPassPortTypeAllCanUse();
        }

        [HttpGet]
        public ListOutputDto<ReadPassportTypeOutputDto> SelectPassPortTypeAll([FromQuery] ReadPassportTypeInputDto readPassportTypeInputDto)
        {
            return baseService.SelectPassPortTypeAll(readPassportTypeInputDto);
        }

        [HttpGet]
        public SingleOutputDto<ReadPassportTypeOutputDto> SelectPassPortTypeByTypeId([FromQuery] ReadPassportTypeInputDto passPortType)
        {
            return baseService.SelectPassPortTypeByTypeId(passPortType);
        }

        [HttpPost]
        public BaseResponse InsertPassPortType([FromBody] CreatePassportTypeInputDto passPortType)
        {
            return baseService.InsertPassPortType(passPortType);
        }

        [HttpPost]
        public BaseResponse DeletePassPortType([FromBody] DeletePassportTypeInputDto portType)
        {
            return baseService.DeletePassPortType(portType);
        }

        [HttpPost]
        public BaseResponse UpdatePassPortType([FromBody] UpdatePassportTypeInputDto portType)
        {
            return baseService.UpdatePassPortType(portType);
        }

        #endregion

        #region 奖惩类型模块

        [HttpGet]
        public ListOutputDto<ReadRewardPunishmentTypeOutputDto> SelectRewardPunishmentTypeAllCanUse()
        {
            return baseService.SelectRewardPunishmentTypeAllCanUse();
        }

        [HttpGet]
        public ListOutputDto<ReadRewardPunishmentTypeOutputDto> SelectRewardPunishmentTypeAll([FromQuery] ReadRewardPunishmentTypeInputDto readRewardPunishmentTypeInputDto)
        {
            return baseService.SelectRewardPunishmentTypeAll(readRewardPunishmentTypeInputDto);
        }

        [HttpGet]
        public SingleOutputDto<ReadRewardPunishmentTypeOutputDto> SelectRewardPunishmentTypeByTypeId([FromQuery] ReadRewardPunishmentTypeInputDto readRewardPunishmentTypeInputDto)
        {
            return baseService.SelectRewardPunishmentTypeByTypeId(readRewardPunishmentTypeInputDto);
        }

        [HttpPost]
        public BaseResponse InsertRewardPunishmentType([FromBody] CreateRewardPunishmentTypeInputDto createRewardPunishmentTypeInputDto)
        {
            return baseService.InsertRewardPunishmentType(createRewardPunishmentTypeInputDto);
        }

        [HttpPost]
        public BaseResponse DeleteRewardPunishmentType([FromBody] DeleteRewardPunishmentTypeInputDto deleteRewardPunishmentTypeInputDto)
        {
            return baseService.DeleteRewardPunishmentType(deleteRewardPunishmentTypeInputDto);
        }

        [HttpPost]
        public BaseResponse UpdateRewardPunishmentType([FromBody] UpdateRewardPunishmentTypeInputDto updateRewardPunishmentTypeInputDto)
        {
            return baseService.UpdateRewardPunishmentType(updateRewardPunishmentTypeInputDto);
        }

        #endregion

        #region 公告类型模块

        /// <summary>
        /// 查询所有公告类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadAppointmentNoticeTypeOutputDto> SelectAppointmentNoticeTypeAll([FromQuery] ReadAppointmentNoticeTypeInputDto readAppointmentNoticeTypeInputDto)
        {
            return baseService.SelectAppointmentNoticeTypeAll(readAppointmentNoticeTypeInputDto);
        }

        /// <summary>
        /// 添加公告类型
        /// </summary>
        /// <param name="createAppointmentNoticeTypeInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseResponse CreateAppointmentNoticeType([FromBody] CreateAppointmentNoticeTypeInputDto createAppointmentNoticeTypeInputDto)
        {
            return baseService.CreateAppointmentNoticeType(createAppointmentNoticeTypeInputDto);
        }

        /// <summary>
        /// 删除公告类型
        /// </summary>
        /// <param name="deleteAppointmentNoticeTypeInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseResponse DeleteAppointmentNoticeType([FromBody] DeleteAppointmentNoticeTypeInputDto deleteAppointmentNoticeTypeInputDto)
        {
            return baseService.DeleteAppointmentNoticeType(deleteAppointmentNoticeTypeInputDto);
        }

        /// <summary>
        /// 更新公告类型
        /// </summary>
        /// <param name="updateAppointmentNoticeTypeInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseResponse UpdateAppointmentNoticeType([FromBody] UpdateAppointmentNoticeTypeInputDto updateAppointmentNoticeTypeInputDto)
        {
            return baseService.UpdateAppointmentNoticeType(updateAppointmentNoticeTypeInputDto);
        }

        #endregion
    }
}
