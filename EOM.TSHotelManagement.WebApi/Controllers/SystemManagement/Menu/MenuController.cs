using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using EOM.TSHotelManagement.Common.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;

        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        /// <summary>
        /// 查询所有菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadMenuOutputDto> SelectMenuAll(ReadMenuInputDto readMenuInputDto)
        {
            return menuService.SelectMenuAll(readMenuInputDto);
        }

        /// <summary>
        /// 构建菜单树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<MenuViewModel> BuildMenuAll([FromBody] BaseInputDto baseInputDto)
        {
            return menuService.BuildMenuAll(baseInputDto);
        }

        /// <summary>
        /// 插入菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto InsertMenu([FromBody] CreateMenuInputDto menu)
        {
            return menuService.InsertMenu(menu);
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdateMenu([FromBody] UpdateMenuInputDto menu)
        {
            return menuService.UpdateMenu(menu);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteMenu([FromBody] DeleteMenuInputDto menu)
        {
            return menuService.DeleteMenu(menu);
        }
    }
}
