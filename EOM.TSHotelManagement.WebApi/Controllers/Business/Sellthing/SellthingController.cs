using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
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
        [HttpGet]
        public ListOutputDto<ReadSellThingOutputDto> SelectSellThingAll([FromQuery] ReadSellThingInputDto sellThing = null)
        {
            return sellService.SelectSellThingAll(sellThing);
        }

        /// <summary>
        /// 查询所有商品(包括库存为0/已删除)
        /// </summary>
        /// <param name="sellThing"></param>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadSellThingOutputDto> GetSellThings([FromQuery] ReadSellThingInputDto sellThing = null)
        {
            return sellService.GetSellThings(sellThing);
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="updateSellThingInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateSellThing([FromBody] UpdateSellThingInputDto updateSellThingInputDto)
        {
            return sellService.UpdateSellThing(updateSellThingInputDto);
        }

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="sellThing"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateSellthingInfo([FromBody] UpdateSellThingInputDto sellThing)
        {
            return sellService.UpdateSellthingInfo(sellThing);
        }

        /// <summary>
        /// 撤回客户消费信息
        /// </summary>
        /// <param name="deleteSellThingInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteSellthing([FromQuery] DeleteSellThingInputDto deleteSellThingInputDto)
        {
            return sellService.DeleteSellthing(deleteSellThingInputDto);
        }

        /// <summary>
        /// 根据商品编号删除商品信息
        /// </summary>
        /// <param name="deleteSellThingInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteSellThingBySellNo([FromQuery] DeleteSellThingInputDto deleteSellThingInputDto)
        {
            return sellService.DeleteSellThingBySellNo(deleteSellThingInputDto);
        }

        /// <summary>
        /// 根据商品名称和价格查询商品编号
        /// </summary>
        /// <param name="readSellThingInputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadSellThingOutputDto> SelectSellThingByNameAndPrice([FromQuery] ReadSellThingInputDto readSellThingInputDto)
        {
            return sellService.SelectSellThingByNameAndPrice(readSellThingInputDto);
        }

        /// <summary>
        /// 根据商品编号查询商品信息
        /// </summary>
        /// <param name="readSellThingInputDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ReadSellThingOutputDto SelectSellInfoBySellNo([FromQuery] ReadSellThingInputDto readSellThingInputDto)
        {
            return sellService.SelectSellInfoBySellNo(readSellThingInputDto);
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto InsertSellThing([FromBody] CreateSellThingInputDto st)
        {
            return sellService.InsertSellThing(st);
        }
    }
}


