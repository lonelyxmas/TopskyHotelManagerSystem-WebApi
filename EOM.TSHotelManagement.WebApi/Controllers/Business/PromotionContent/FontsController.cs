using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 酒店宣传联动内容控制器
    /// </summary>
    public class FontsController : ControllerBase
    {
        /// <summary>
        /// 酒店宣传联动内容
        /// </summary>
        private readonly IPromotionContentService fontsService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontsService"></param>
        public FontsController(IPromotionContentService fontsService)
        {
            this.fontsService = fontsService;
        }

        /// <summary>
        /// 查询所有宣传联动内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadPromotionContentOutputDto> SelectPromotionContentAll([FromQuery] ReadPromotionContentInputDto readPromotionContentInputDto)
        {
            return fontsService.SelectPromotionContentAll(readPromotionContentInputDto);
        }

        /// <summary>
        /// 查询所有宣传联动内容(跑马灯)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadPromotionContentOutputDto> SelectPromotionContents()
        {
            return fontsService.SelectPromotionContents();
        }

        /// <summary>
        /// 添加宣传联动内容
        /// </summary>
        /// <param name="createPromotionContentInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto AddPromotionContent([FromBody]CreatePromotionContentInputDto createPromotionContentInputDto)
        {
            return fontsService.AddPromotionContent(createPromotionContentInputDto);
        }

        /// <summary>
        /// 删除宣传联动内容
        /// </summary>
        /// <param name="deletePromotionContentInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeletePromotionContent([FromBody] DeletePromotionContentInputDto deletePromotionContentInputDto)
        {
            return fontsService.DeletePromotionContent(deletePromotionContentInputDto);
        }

        /// <summary>
        /// 更新宣传联动内容
        /// </summary>
        /// <param name="updatePromotionContentInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdatePromotionContent([FromBody] UpdatePromotionContentInputDto updatePromotionContentInputDto)
        {
            return fontsService.UpdatePromotionContent(updatePromotionContentInputDto);
        }
    }
}
