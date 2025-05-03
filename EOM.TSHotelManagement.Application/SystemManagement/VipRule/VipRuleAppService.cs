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
    /// 会员等级规则功能模块接口实现类
    /// </summary>
    public class VipRuleAppService : IVipRuleAppService
    {
        /// <summary>
        /// 会员等级规则
        /// </summary>
        private readonly GenericRepository<VipLevelRule> vipRuleRepository;

        /// <summary>
        /// 客户类型
        /// </summary>
        private readonly GenericRepository<CustoType> custoTypeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipRuleRepository"></param>
        /// <param name="custoTypeRepository"></param>
        public VipRuleAppService(GenericRepository<VipLevelRule> vipRuleRepository, GenericRepository<CustoType> custoTypeRepository)
        {
            this.vipRuleRepository = vipRuleRepository;
            this.custoTypeRepository = custoTypeRepository;
        }

        /// <summary>
        /// 查询会员等级规则列表
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadVipLevelRuleOutputDto> SelectVipRuleList(ReadVipLevelRuleInputDto readVipLevelRuleInputDto)
        {
            List<VipLevelRule> vipRules = new List<VipLevelRule>();
            var where = Expressionable.Create<VipLevelRule>();
            if (!readVipLevelRuleInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readVipLevelRuleInputDto.IsDelete);
            }
            var count = 0;
            var listSource = vipRuleRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readVipLevelRuleInputDto.Page, readVipLevelRuleInputDto.PageSize, ref count);

            var listUserType = custoTypeRepository.GetList(a => a.IsDelete != 1);

            listSource.ForEach(source =>
            {
                var userType = listUserType.SingleOrDefault(a => a.CustomerType == source.VipLevelId);
                source.VipLevelName = userType == null ? "" : userType.CustomerTypeName;
            });

            var viprules = EntityMapper.MapList<VipLevelRule, ReadVipLevelRuleOutputDto>(listSource);

            return new ListOutputDto<ReadVipLevelRuleOutputDto>
            {
                listSource = viprules,
                total = count
            };
        }

        /// <summary>
        /// 查询会员等级规则
        /// </summary>
        /// <param name="vipRule"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadVipLevelRuleOutputDto> SelectVipRule(ReadVipLevelRuleInputDto vipRule)
        {
            VipLevelRule vipRule1 = new VipLevelRule();

            var source = vipRuleRepository.GetSingle(a => a.RuleSerialNumber.Equals(vipRule.RuleSerialNumber));

            var userType = custoTypeRepository.GetSingle(a => a.CustomerType == source.VipLevelId);
            source.VipLevelName = userType == null ? "" : userType.CustomerTypeName;

            var vipLevel = EntityMapper.Map<VipLevelRule, ReadVipLevelRuleOutputDto>(source);

            return new SingleOutputDto<ReadVipLevelRuleOutputDto>
            {
                Source = vipLevel
            };
        }

        /// <summary>
        /// 添加会员等级规则
        /// </summary>
        /// <param name="vipRule"></param>
        /// <returns></returns>
        public BaseOutputDto AddVipRule(CreateVipLevelRuleInputDto vipRule)
        {
            try
            {
                vipRuleRepository.Insert(new VipLevelRule()
                {
                    RuleSerialNumber = vipRule.RuleSerialNumber,
                    RuleName = vipRule.RuleName,
                    RuleValue = vipRule.RuleValue,
                    VipLevelId = vipRule.VipLevelId,
                    IsDelete = 0,
                    DataInsUsr = vipRule.DataInsUsr,
                });
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除会员等级规则
        /// </summary>
        /// <param name="vipRule"></param>
        /// <returns></returns>
        public BaseOutputDto DelVipRule(DeleteVipLevelRuleInputDto vipRule)
        {
            try
            {
                vipRuleRepository.Update(a => new VipLevelRule
                {
                    IsDelete = 1,
                    DataChgUsr = vipRule.DataChgUsr,
                }, a => a.RuleSerialNumber == vipRule.RuleSerialNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新会员等级规则
        /// </summary>
        /// <param name="vipRule"></param>
        /// <returns></returns>
        public BaseOutputDto UpdVipRule(UpdateVipLevelRuleInputDto vipRule)
        {
            try
            {
                vipRuleRepository.Update(a => new VipLevelRule
                {
                    RuleName = vipRule.RuleName,
                    RuleValue = vipRule.RuleValue,
                    IsDelete = vipRule.IsDelete,
                    DataChgUsr = vipRule.DataChgUsr,
                    DataChgDate = vipRule.DataChgDate
                }, a => a.RuleSerialNumber == vipRule.RuleSerialNumber);
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }
    }
}
