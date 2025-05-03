using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 资产信息控制器
    /// </summary>
    public class AssetController : ControllerBase
    {
        /// <summary>
        /// 资产信息
        /// </summary>
        private readonly IAssetService assetService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetService"></param>
        public AssetController(IAssetService assetService)
        {
            this.assetService = assetService;
        }

        /// <summary>
        /// 添加资产信息
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto AddAssetInfo([FromBody] CreateAssetInputDto asset)
        {
            return assetService.AddAssetInfo(asset);
        }

        /// <summary>
        /// 查询资产信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadAssetOutputDto> SelectAssetInfoAll([FromQuery] ReadAssetInputDto asset)
        {
            return assetService.SelectAssetInfoAll(asset);
        }

        /// <summary>
        /// 更新资产信息
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdAssetInfo([FromBody] UpdateAssetInputDto asset)
        {
            return assetService.UpdAssetInfo(asset);
        }

        /// <summary>
        /// 删除资产信息
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DelAssetInfo([FromBody] DeleteAssetInputDto asset)
        {
            return assetService.DelAssetInfo(asset);
        }

    }
}
