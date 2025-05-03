using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 会员规则控制器
    /// </summary>
    public class VipRuleController : ControllerBase
    {
        private readonly IVipRuleAppService vipRuleAppService;

        public VipRuleController(IVipRuleAppService vipRuleAppService)
        {
            this.vipRuleAppService = vipRuleAppService;
        }

        /// <summary>
        /// 查询会员等级规则列表
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadVipLevelRuleOutputDto> SelectVipRuleList([FromQuery] ReadVipLevelRuleInputDto inputDto)
        {
            return vipRuleAppService.SelectVipRuleList(inputDto);
        }

        /// <summary>
        /// 查询会员等级规则
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadVipLevelRuleOutputDto> SelectVipRule([FromQuery] ReadVipLevelRuleInputDto inputDto)
        {
            return vipRuleAppService.SelectVipRule(inputDto);
        }

        /// <summary>
        /// 添加会员等级规则
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto AddVipRule([FromBody] CreateVipLevelRuleInputDto inputDto)
        {
            return vipRuleAppService.AddVipRule(inputDto);
        }

        /// <summary>
        /// 删除会员等级规则
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DelVipRule([FromBody] DeleteVipLevelRuleInputDto inputDto)
        {
            return vipRuleAppService.DelVipRule(inputDto);
        }

        /// <summary>
        /// 更新会员等级规则
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdVipRule([FromBody] UpdateVipLevelRuleInputDto inputDto)
        {
            return vipRuleAppService.UpdVipRule(inputDto);
        }
    }
}


