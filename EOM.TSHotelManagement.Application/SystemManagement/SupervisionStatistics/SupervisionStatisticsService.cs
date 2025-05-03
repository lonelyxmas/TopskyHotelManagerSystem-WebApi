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
    /// 监管统计接口实现类
    /// </summary>
    public class SupervisionStatisticsService : ISupervisionStatisticsService
    {
        /// <summary>
        /// 监管统计
        /// </summary>
        private readonly GenericRepository<SupervisionStatistics> checkInfoRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkInfoRepository"></param>
        public SupervisionStatisticsService(GenericRepository<SupervisionStatistics> checkInfoRepository)
        {
            this.checkInfoRepository = checkInfoRepository;
        }

        /// <summary>
        /// 查询所有监管统计信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadSupervisionStatisticsOutputDto> SelectSupervisionStatisticsAll(ReadSupervisionStatisticsInputDto readSupervisionStatisticsInputDto)
        {
            List<SupervisionStatistics> cif = new List<SupervisionStatistics>();

            var where = Expressionable.Create<SupervisionStatistics>();

            where = where.And(a => a.IsDelete == readSupervisionStatisticsInputDto.IsDelete);
            var count = 0;
            cif = checkInfoRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readSupervisionStatisticsInputDto.Page, readSupervisionStatisticsInputDto.PageSize, ref count);
            var deptId = cif.Select(a => a.SupervisingDepartment).ToList();
            var depts = checkInfoRepository.Change<Department>().GetList(a => deptId.Contains(a.DepartmentNumber));
            cif.ForEach(c =>
            {
                var dept = depts.SingleOrDefault(a => a.DepartmentNumber == c.SupervisingDepartment);
                c.SupervisingDepartmentName = !dept.IsNullOrEmpty() ? dept.DepartmentName : string.Empty;
            });

            var listSource = EntityMapper.MapList<SupervisionStatistics, ReadSupervisionStatisticsOutputDto>(cif);

            return new ListOutputDto<ReadSupervisionStatisticsOutputDto>
            {
                listSource = listSource,
                total = listSource.Count
            };
        }

        /// <summary>
        /// 插入监管统计信息
        /// </summary>
        /// <param name="checkInfo"></param>
        /// <returns></returns>
        public BaseOutputDto InsertSupervisionStatistics(CreateSupervisionStatisticsInputDto checkInfo)
        {
            try
            {
                checkInfoRepository.Insert(EntityMapper.Map<CreateSupervisionStatisticsInputDto, SupervisionStatistics>(checkInfo));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新监管统计信息
        /// </summary>
        /// <param name="checkInfo"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateSupervisionStatistics(UpdateSupervisionStatisticsInputDto checkInfo)
        {
            try
            {
                checkInfoRepository.Update(a => new SupervisionStatistics
                {
                    SupervisingDepartment = checkInfo.SupervisingDepartment,
                    SupervisionLoss = checkInfo.SupervisionLoss,
                    SupervisionScore = checkInfo.SupervisionScore,
                    SupervisionAdvice = checkInfo.SupervisionAdvice,
                    SupervisionStatistician = checkInfo.SupervisionStatistician,
                    SupervisionProgress = checkInfo.SupervisionProgress,
                    IsDelete = 0,
                    DataChgUsr = checkInfo.DataChgUsr,
                    DataChgDate = checkInfo.DataChgDate
                }, a => a.StatisticsNumber == checkInfo.StatisticsNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除监管统计信息
        /// </summary>
        /// <param name="checkInfo"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteSupervisionStatistics(DeleteSupervisionStatisticsInputDto checkInfo)
        {
            try
            {
                checkInfoRepository.Update(a => new SupervisionStatistics
                {
                    IsDelete = 1,
                    DataChgUsr = checkInfo.DataChgUsr,
                    DataChgDate = checkInfo.DataChgDate
                }, a => a.StatisticsNumber == checkInfo.StatisticsNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
    }
}
