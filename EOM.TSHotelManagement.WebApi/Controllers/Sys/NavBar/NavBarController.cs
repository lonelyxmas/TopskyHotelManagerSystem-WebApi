using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 导航控件模块控制器
    /// </summary>
    public class NavBarController : ControllerBase
    {
        /// <summary>
        /// 导航控件
        /// </summary>
        private readonly INavBarService navBarService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navBarService"></param>
        public NavBarController(INavBarService navBarService)
        {
            this.navBarService = navBarService;
        }

        /// <summary>
        /// 导航控件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadNavBarOutputDto> NavBarList()
        {
            return navBarService.NavBarList();
        }

    }
}
