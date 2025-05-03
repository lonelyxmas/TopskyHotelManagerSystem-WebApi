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
    /// 员工奖惩记录接口实现类
    /// </summary>
    public class RewardPunishmentService : IRewardPunishmentService
    {
        /// <summary>
        /// 员工奖惩记录
        /// </summary>
        private readonly GenericRepository<EmployeeRewardPunishment> rewardPunishmentRepository;

        /// <summary>
        /// 管理员记录
        /// </summary>
        private readonly GenericRepository<Administrator> adminRepository;

        /// <summary>
        /// 奖惩类型
        /// </summary>
        private readonly GenericRepository<RewardPunishmentType> goodbadTypeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rewardPunishmentRepository"></param>
        /// <param name="adminRepository"></param>
        /// <param name="goodbadTypeRepository"></param>
        public RewardPunishmentService(GenericRepository<EmployeeRewardPunishment> rewardPunishmentRepository, GenericRepository<Administrator> adminRepository, GenericRepository<RewardPunishmentType> goodbadTypeRepository)
        {
            this.rewardPunishmentRepository = rewardPunishmentRepository;
            this.adminRepository = adminRepository;
            this.goodbadTypeRepository = goodbadTypeRepository;
        }

        /// <summary>
        /// 添加员工奖惩记录
        /// </summary>
        /// <param name="goodBad"></param>
        /// <returns></returns>
        public BaseOutputDto AddRewardPunishment(CreateEmployeeRewardPunishmentInputDto goodBad)
        {
            try
            {
                rewardPunishmentRepository.Insert(EntityMapper.Map<CreateEmployeeRewardPunishmentInputDto, EmployeeRewardPunishment>(goodBad));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 根据工号查找所有的奖惩记录信息
        /// </summary>
        /// <param name="wn"></param>
        /// <returns></returns>
        public ListOutputDto<ReadEmployeeRewardPunishmentOutputDto> SelectAllRewardPunishmentByEmployeeId(ReadEmployeeRewardPunishmentInputDto wn)
        {
            var where = Expressionable.Create<EmployeeRewardPunishment>();

            if (!wn.EmployeeId.IsNullOrEmpty())
            {
                where = where.And(a => a.EmployeeId == wn.EmployeeId);
            }

            if (!wn.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == wn.IsDelete);
            }

            //查询所有超级管理员
            List<Administrator> admins = new List<Administrator>();
            admins = adminRepository.GetList(a => a.IsDelete != 1);
            List<RewardPunishmentType> gBTypes = new List<RewardPunishmentType>();
            gBTypes = goodbadTypeRepository.GetList(a => a.IsDelete != 1);
            List<EmployeeRewardPunishment> gb = new List<EmployeeRewardPunishment>();

            var count = 0;

            if (wn.Page != 0 && wn.PageSize != 0)
            {
                gb = rewardPunishmentRepository.AsQueryable().Where(where.ToExpression())
                    .OrderByDescending(a => a.RewardPunishmentTime)
                    .ToPageList(wn.Page, wn.PageSize, ref count);
            }
            else
            {
                gb = rewardPunishmentRepository.AsQueryable().Where(where.ToExpression())
                    .OrderByDescending(a => a.RewardPunishmentTime)
                    .ToList();
            }

            gb.ForEach(source =>
            {
                //奖惩类型
                var gbType = gBTypes.SingleOrDefault(a => a.RewardPunishmentTypeId == source.RewardPunishmentType);
                source.RewardPunishmentTypeName = gbType.RewardPunishmentTypeName.IsNullOrEmpty() ? "" : gbType.RewardPunishmentTypeName;

                //操作人
                var admin = admins.SingleOrDefault(a => a.Account == source.RewardPunishmentOperator);
                source.OperatorName = admin.Name.IsNullOrEmpty() ? "" : admin.Name;
            });

            return new ListOutputDto<ReadEmployeeRewardPunishmentOutputDto>
            {
                listSource = EntityMapper.MapList<EmployeeRewardPunishment, ReadEmployeeRewardPunishmentOutputDto>(gb),
                total = count
            };
        }
    }
}
