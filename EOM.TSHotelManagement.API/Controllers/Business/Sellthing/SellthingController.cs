using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Service;
using EOM.TSHotelManagement.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 商品消费控制器
    /// </summary>
    public class SellthingController : ControllerBase
    {
        private readonly ISellService sellService;

        public SellthingController(ISellService sellService)
        {
            this.sellService = sellService;
        }

        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <param name="sellThing"></param>
        /// <returns></returns>
        [RequirePermission("goodsmanagement.view")]
        [HttpGet]
        public ListOutputDto<ReadSellThingOutputDto> SelectSellThingAll([FromQuery] ReadSellThingInputDto sellThing = null)
        {
            return sellService.SelectSellThingAll(sellThing);
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="updateSellThingInputDto"></param>
        /// <returns></returns>
        [RequirePermission("goodsmanagement.update")]
        [HttpPost]
        public BaseResponse UpdateSellThing([FromBody] UpdateSellThingInputDto updateSellThingInputDto)
        {
            return sellService.UpdateSellthing(updateSellThingInputDto);
        }

        /// <summary>
        /// 撤回客户消费信息
        /// </summary>
        /// <param name="deleteSellThingInputDto"></param>
        /// <returns></returns>
        [RequirePermission("goodsmanagement.delete")]
        [HttpPost]
        public BaseResponse DeleteSellthing([FromBody] DeleteSellThingInputDto deleteSellThingInputDto)
        {
            return sellService.DeleteSellthing(deleteSellThingInputDto);
        }

        /// <summary>
        /// 根据商品名称和价格查询商品编号
        /// </summary>
        /// <param name="readSellThingInputDto"></param>
        /// <returns></returns>
        [RequirePermission("goodsmanagement.view")]
        [HttpGet]
        public SingleOutputDto<ReadSellThingOutputDto> SelectSellThingByNameAndPrice([FromQuery] ReadSellThingInputDto readSellThingInputDto)
        {
            return sellService.SelectSellThingByNameAndPrice(readSellThingInputDto);
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        [RequirePermission("goodsmanagement.create")]
        [HttpPost]
        public BaseResponse InsertSellThing([FromBody] CreateSellThingInputDto st)
        {
            return sellService.InsertSellThing(st);
        }
    }
}


