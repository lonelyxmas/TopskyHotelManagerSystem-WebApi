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
    /// 员工打卡接口实现类
    /// </summary>
    public class EmployeeCheckService : IEmployeeCheckService
    {
        /// <summary>
        /// 员工打卡
        /// </summary>
        private readonly GenericRepository<EmployeeCheck> workerCheckRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerCheckRepository"></param>
        public EmployeeCheckService(GenericRepository<EmployeeCheck> workerCheckRepository)
        {
            this.workerCheckRepository = workerCheckRepository;
        }

        /// <summary>
        /// 根据员工编号查询其所有的打卡记录
        /// </summary>
        /// <param name="wid"></param>
        /// <returns></returns>
        public ListOutputDto<ReadEmployeeCheckOutputDto> SelectCheckInfoByEmployeeId(ReadEmployeeCheckInputDto wid)
        {
            List<EmployeeCheck> workerChecks = new List<EmployeeCheck>();

            var where = Expressionable.Create<EmployeeCheck>();

            if (!wid.EmployeeId.IsNullOrEmpty())
            {
                where = where.And(a => a.EmployeeId == wid.EmployeeId);
            }

            if (!wid.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == wid.IsDelete);
            }

            var count = 0;

            if (wid.Page != 0 && wid.PageSize != 0)
            {
                workerChecks = workerCheckRepository.AsQueryable().Where(where.ToExpression())
                    .OrderByDescending(a => a.CheckTime)
                    .ToPageList(wid.Page, wid.PageSize, ref count);
            }
            else
            {
                workerChecks = workerCheckRepository.AsQueryable().Where(where.ToExpression())
                    .OrderByDescending(a => a.CheckTime)
                    .ToList();
            }

            workerChecks.ForEach(source =>
            {
                source.CheckStatusDescription = source.CheckStatus == 0 ? "打卡成功" : "打卡失败";
            });
            var source = EntityMapper.MapList<EmployeeCheck, ReadEmployeeCheckOutputDto>(workerChecks);

            return new ListOutputDto<ReadEmployeeCheckOutputDto>
            {
                listSource = source,
                total = count
            };
        }


        /// <summary>
        /// 查询员工签到天数
        /// </summary>
        /// <param name="wkn"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadEmployeeCheckOutputDto> SelectWorkerCheckDaySumByEmployeeId(ReadEmployeeCheckInputDto wkn)
        {
            var checkDay = 0;
            try
            {
                checkDay = workerCheckRepository.AsQueryable().Count(a => a.EmployeeId == wkn.EmployeeId && a.IsDelete != 1);
            }
            catch (Exception)
            {
                return new SingleOutputDto<ReadEmployeeCheckOutputDto> { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString("Query employee sign-in days failed", "查询员工签到天数失败") };
            }

            return new SingleOutputDto<ReadEmployeeCheckOutputDto>
            {
                Source = new ReadEmployeeCheckOutputDto
                {
                    CheckDay = checkDay
                }
            };
        }


        /// <summary>
        /// 查询今天员工是否已签到
        /// </summary>
        /// <param name="wkn"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadEmployeeCheckOutputDto> SelectToDayCheckInfoByWorkerNo(ReadEmployeeCheckInputDto wkn)
        {
            var isChecked = false;
            try
            {
                var today = DateTime.Today;
                isChecked = workerCheckRepository.AsQueryable()
                    .Any(a => a.EmployeeId == wkn.EmployeeId
                           && a.IsDelete != 1
                           && a.CheckTime >= today && a.CheckTime < today.AddDays(1));
            }
            catch (Exception ex)
            {
                return new SingleOutputDto<ReadEmployeeCheckOutputDto> { StatusCode = StatusCodeConstants.InternalServerError, Message = LocalizationHelper.GetLocalizedString($"Error:\n{ex.Message}", $"错误:\n{ex.Message}") };
            }
            return new SingleOutputDto<ReadEmployeeCheckOutputDto> { Source = new ReadEmployeeCheckOutputDto { IsChecked = isChecked } };
        }

        /// <summary>
        /// 添加员工打卡数据
        /// </summary>
        /// <param name="workerCheck"></param>
        /// <returns></returns>
        public BaseOutputDto AddCheckInfo(CreateEmployeeCheckInputDto workerCheck)
        {
            try
            {
                workerCheckRepository.Insert(EntityMapper.Map<CreateEmployeeCheckInputDto, EmployeeCheck>(workerCheck));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
    }
}
