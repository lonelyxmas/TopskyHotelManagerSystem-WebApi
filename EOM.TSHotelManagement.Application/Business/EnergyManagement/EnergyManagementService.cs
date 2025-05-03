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
    /// 水电信息接口实现类
    /// </summary>
    public class EnergyManagementService : IEnergyManagementService
    {
        /// <summary>
        /// 水电信息
        /// </summary>
        private readonly GenericRepository<EnergyManagement> wtiRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wtiRepository"></param>
        public EnergyManagementService(GenericRepository<EnergyManagement> wtiRepository)
        {
            this.wtiRepository = wtiRepository;
        }

        /// <summary>
        /// 根据条件查询水电费信息
        /// </summary>
        /// <param name="readEnergyManagementInputDto">Dto</param>
        /// <returns>符合条件的水电费信息列表</returns>
        public ListOutputDto<ReadEnergyManagementOutputDto> SelectEnergyManagementInfo(ReadEnergyManagementInputDto readEnergyManagementInputDto)
        {
            var where = Expressionable.Create<EnergyManagement>();

            if (!string.IsNullOrEmpty(readEnergyManagementInputDto.RoomNo))
            {
                where = where.And(a => a.RoomNumber.Equals(readEnergyManagementInputDto.RoomNo));
            }

            if (!readEnergyManagementInputDto.UseDate.IsNullOrEmpty())
            {
                where = where.And(a => a.StartDate >= readEnergyManagementInputDto.UseDate.Value);
            }

            if (!readEnergyManagementInputDto.EndDate.IsNullOrEmpty())
            {
                where = where.And(a => a.EndDate >= readEnergyManagementInputDto.EndDate.Value);
            }

            if (!readEnergyManagementInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readEnergyManagementInputDto.IsDelete);
            }

            var count = 0;

            var listSource = wtiRepository.AsQueryable()
                .Where(where.ToExpression()).ToPageList(readEnergyManagementInputDto.Page, readEnergyManagementInputDto.PageSize, ref count);

            var readEnergies = EntityMapper.MapList<EnergyManagement, ReadEnergyManagementOutputDto>(listSource);

            return new ListOutputDto<ReadEnergyManagementOutputDto> { listSource = readEnergies, total = count };
        }

        /// <summary>
        /// 添加水电费信息
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public BaseOutputDto InsertEnergyManagementInfo(CreateEnergyManagementInputDto w)
        {
            try
            {
                if (wtiRepository.IsAny(a => a.InformationId == w.InformationNumber))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("information number already exist.", "信息编号已存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                var result = wtiRepository.Insert(EntityMapper.Map<CreateEnergyManagementInputDto, EnergyManagement>(w));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Insert Energy Management Failed", "水电信息添加失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Insert Energy Management Failed", "水电信息添加失败"));
            }

            return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Insert Energy Management Success", "水电信息添加成功"));
        }

        /// <summary>
        /// 修改水电费信息
        /// </summary>
        /// <param name="w">包含要修改的数据，以及EnergyManagementNo作为查询条件</param>
        /// <returns></returns>
        public BaseOutputDto UpdateEnergyManagementInfo(UpdateEnergyManagementInputDto w)
        {
            try
            {
                if (!wtiRepository.IsAny(a => a.InformationId == w.InformationId))
                {
                    return new BaseOutputDto() { Message = LocalizationHelper.GetLocalizedString("information number does not exist.", "信息编号不存在"), StatusCode = StatusCodeConstants.InternalServerError };
                }
                var result = wtiRepository.Update(EntityMapper.Map<UpdateEnergyManagementInputDto, EnergyManagement>(w));

                if (result)
                {
                    return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Update Energy Management Success", "水电费信息更新成功"));
                }
                else
                {
                    return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Energy Management Failed", "水电费信息更新失败"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Update Energy Management Failed", "水电费信息更新失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Update Energy Management Failed", "水电费信息更新失败"));
            }
        }

        /// <summary>
        /// 根据房间编号、使用时间删除水电费信息
        /// </summary>
        /// <returns></returns>
        public BaseOutputDto DeleteEnergyManagementInfo(DeleteEnergyManagementInputDto hydroelectricity)
        {
            try
            {
                var result = wtiRepository.Update(EntityMapper.Map<DeleteEnergyManagementInputDto, EnergyManagement>(hydroelectricity));

                if (result)
                {
                    return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Delete Energy Management Success", "水电费信息删除成功"));
                }
                else
                {
                    return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Energy Management Failed", "水电费信息删除失败"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(LocalizationHelper.GetLocalizedString("Delete Energy Management Failed", "水电费信息删除失败"), ex);
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Energy Management Failed", "水电费信息删除失败"));
            }
        }
    }
}
