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
using Microsoft.Extensions.Logging;

namespace EOM.TSHotelManagement.Service
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
        private readonly ILogger<EnergyManagementService> _logger;

        public EnergyManagementService(GenericRepository<EnergyManagement> wtiRepository, ILogger<EnergyManagementService> logger)
        {
            this.wtiRepository = wtiRepository;
            _logger = logger;
        }

        /// <summary>
        /// 根据条件查询水电费信息
        /// </summary>
        /// <param name="readEnergyManagementInputDto">Dto</param>
        /// <returns>符合条件的水电费信息列表</returns>
        public ListOutputDto<ReadEnergyManagementOutputDto> SelectEnergyManagementInfo(ReadEnergyManagementInputDto readEnergyManagementInputDto)
        {
            var where = SqlFilterBuilder.BuildExpression<EnergyManagement, ReadEnergyManagementInputDto>(readEnergyManagementInputDto);

            var count = 0;
            var Data = new List<EnergyManagement>();

            if (readEnergyManagementInputDto.IgnorePaging)
            {
                Data = wtiRepository.AsQueryable()
                    .Where(where.ToExpression()).ToList();
                count = Data.Count;
            }
            else
            {
                Data = wtiRepository.AsQueryable()
                    .Where(where.ToExpression()).ToPageList(readEnergyManagementInputDto.Page, readEnergyManagementInputDto.PageSize, ref count);
            }

            var readEnergies = EntityMapper.MapList<EnergyManagement, ReadEnergyManagementOutputDto>(Data);

            return new ListOutputDto<ReadEnergyManagementOutputDto>
            {
                Data = new PagedData<ReadEnergyManagementOutputDto>
                {
                    Items = readEnergies,
                    TotalCount = count
                }
            };
        }

        /// <summary>
        /// 添加水电费信息
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public BaseResponse InsertEnergyManagementInfo(CreateEnergyManagementInputDto w)
        {
            try
            {
                if (wtiRepository.IsAny(a => a.InformationId == w.InformationNumber))
                {
                    return new BaseResponse() { Message = LocalizationHelper.GetLocalizedString("information number already exist.", "信息编号已存在"), Code = BusinessStatusCode.InternalServerError };
                }
                var result = wtiRepository.Insert(EntityMapper.Map<CreateEnergyManagementInputDto, EnergyManagement>(w));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LocalizationHelper.GetLocalizedString("Insert Energy Management Failed", "水电信息添加失败"));
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Insert Energy Management Failed", "水电信息添加失败"));
            }

            return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Insert Energy Management Success", "水电信息添加成功"));
        }

        /// <summary>
        /// 修改水电费信息
        /// </summary>
        /// <param name="w">包含要修改的数据，以及EnergyManagementNo作为查询条件</param>
        /// <returns></returns>
        public BaseResponse UpdateEnergyManagementInfo(UpdateEnergyManagementInputDto w)
        {
            try
            {
                if (!wtiRepository.IsAny(a => a.InformationId == w.InformationId))
                {
                    return new BaseResponse() { Message = LocalizationHelper.GetLocalizedString("information number does not exist.", "信息编号不存在"), Code = BusinessStatusCode.InternalServerError };
                }
                var result = wtiRepository.Update(EntityMapper.Map<UpdateEnergyManagementInputDto, EnergyManagement>(w));

                if (result)
                {
                    return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Update Energy Management Success", "水电费信息更新成功"));
                }
                else
                {
                    return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Update Energy Management Failed", "水电费信息更新失败"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LocalizationHelper.GetLocalizedString("Update Energy Management Failed", "水电费信息更新失败"));
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Update Energy Management Failed", "水电费信息更新失败"));
            }
        }

        /// <summary>
        /// 根据房间编号、使用时间删除水电费信息
        /// </summary>
        /// <returns></returns>
        public BaseResponse DeleteEnergyManagementInfo(DeleteEnergyManagementInputDto hydroelectricity)
        {
            try
            {
                if (hydroelectricity?.DelIds == null || !hydroelectricity.DelIds.Any())
                {
                    return new BaseResponse
                    {
                        Code = BusinessStatusCode.BadRequest,
                        Message = LocalizationHelper.GetLocalizedString("Parameters Invalid", "参数错误")
                    };
                }

                var energyManagements = wtiRepository.GetList(a => hydroelectricity.DelIds.Contains(a.Id));

                if (!energyManagements.Any())
                {
                    return new BaseResponse
                    {
                        Code = BusinessStatusCode.NotFound,
                        Message = LocalizationHelper.GetLocalizedString("Energy Management Information Not Found", "水电费信息未找到")
                    };
                }

                var result = wtiRepository.SoftDeleteRange(energyManagements);

                if (result)
                {
                    return new BaseResponse(BusinessStatusCode.Success, LocalizationHelper.GetLocalizedString("Delete Energy Management Success", "水电费信息删除成功"));
                }
                else
                {
                    return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Energy Management Failed", "水电费信息删除失败"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LocalizationHelper.GetLocalizedString("Delete Energy Management Failed", "水电费信息删除失败"));
                return new BaseResponse(BusinessStatusCode.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Energy Management Failed", "水电费信息删除失败"));
            }
        }
    }
}
