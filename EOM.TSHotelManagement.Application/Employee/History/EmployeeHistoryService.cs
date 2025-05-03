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

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 员工履历接口实现类
    /// </summary>
    public class EmployeeHistoryService : IEmployeeHistoryService
    {
        /// <summary>
        /// 员工履历
        /// </summary>
        private readonly GenericRepository<EmployeeHistory> workerHistoryRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerHistoryRepository"></param>
        public EmployeeHistoryService(GenericRepository<EmployeeHistory> workerHistoryRepository)
        {
            this.workerHistoryRepository = workerHistoryRepository;
        }

        /// <summary>
        /// 根据工号添加员工履历
        /// </summary>
        /// <param name="workerHistory"></param>
        /// <returns></returns>
        public BaseOutputDto AddHistoryByEmployeeId(CreateEmployeeHistoryInputDto workerHistory)
        {
            try
            {
                workerHistoryRepository.Insert(EntityMapper.Map<CreateEmployeeHistoryInputDto, EmployeeHistory>(workerHistory));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 根据工号查询履历信息
        /// </summary>
        /// <param name="wid"></param>
        /// <returns></returns>
        public ListOutputDto<ReadEmployeeHistoryOutputDto> SelectHistoryByEmployeeId(ReadEmployeeHistoryInputDto wid)
        {
            List<EmployeeHistory> why = new List<EmployeeHistory>();
            why = workerHistoryRepository.GetList(a => a.IsDelete != 1 && a.EmployeeId == wid.EmployeeId);
            var source = EntityMapper.MapList<EmployeeHistory, ReadEmployeeHistoryOutputDto>(why);
            return new ListOutputDto<ReadEmployeeHistoryOutputDto> { listSource = source, total = source.Count };
        }
    }
}
