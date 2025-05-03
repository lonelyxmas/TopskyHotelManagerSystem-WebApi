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
    /// 菜单接口实现类
    /// </summary>
    public class MenuService : IMenuService
    {
        /// <summary>
        /// 菜单
        /// </summary>
        private readonly GenericRepository<Menu> menuRepository;

        /// <summary>
        /// JWT助手
        /// </summary>
        private readonly JWTHelper jWTHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuRepository"></param>
        /// <param name="jWTHelper"></param>
        public MenuService(GenericRepository<Menu> menuRepository, JWTHelper jWTHelper)
        {
            this.menuRepository = menuRepository;
            this.jWTHelper = jWTHelper;
        }

        /// <summary>
        /// 构建菜单树
        /// </summary>
        /// <returns></returns>
        public List<MenuViewModel> BuildMenuAll(BaseInputDto baseInputDto)
        {
            var token = baseInputDto.UserToken;

            var number = jWTHelper.GetSerialNumber(token);

            List<Menu> allMenus = menuRepository.GetList(a => a.IsDelete != 1).OrderBy(a => a.Id).ToList();

            List<MenuViewModel> result = BuildMenuTree(allMenus, null);

            return result;
        }

        /// <summary>
        /// 查询所有菜单信息
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadMenuOutputDto> SelectMenuAll(ReadMenuInputDto readMenuInputDto)
        {
            var where = Expressionable.Create<Menu>();

            if (!readMenuInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readMenuInputDto.IsDelete);
            }
            if (readMenuInputDto.SearchParent)
            {
                where = where.And(a => !a.Parent.HasValue);
            }

            var count = 0;
            List<Menu> allMenus = new List<Menu>();

            if (!readMenuInputDto.IgnorePaging && readMenuInputDto.Page != 0 && readMenuInputDto.PageSize != 0)
            {
                allMenus = menuRepository.AsQueryable().Where(where.ToExpression()).ToPageList(readMenuInputDto.Page, readMenuInputDto.PageSize, ref count);
            }
            else
            {
                allMenus = menuRepository.AsQueryable().Where(where.ToExpression()).ToList();
            }

            List<ReadMenuOutputDto> result = EntityMapper.MapList<Menu, ReadMenuOutputDto>(allMenus);

            return new ListOutputDto<ReadMenuOutputDto>
            {
                listSource = result,
                total = count
            };
        }

        /// <summary>
        /// 插入菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public BaseOutputDto InsertMenu(CreateMenuInputDto menu)
        {
            try
            {
                if (menuRepository.IsAny(a => a.Key == menu.Key && a.IsDelete == 0))
                {
                    return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString("This menu already exists.", "菜单已存在。"), StatusCode = StatusCodeConstants.InternalServerError };
                }

                menuRepository.Insert(EntityMapper.Map<CreateMenuInputDto, Menu>(menu));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public BaseOutputDto UpdateMenu(UpdateMenuInputDto menu)
        {
            try
            {
                menuRepository.Update(EntityMapper.Map<UpdateMenuInputDto, Menu>(menu));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public BaseOutputDto DeleteMenu(DeleteMenuInputDto menu)
        {
            try
            {
                menuRepository.Update(EntityMapper.Map<DeleteMenuInputDto, Menu>(menu));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 递归构建菜单树
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<MenuViewModel> BuildMenuTree(List<Menu> menus, int? parentId)
        {
            List<MenuViewModel> result = new List<MenuViewModel>();

            var filteredMenus = menus.Where(m => m.Parent == parentId).ToList();

            foreach (var menu in filteredMenus)
            {
                MenuViewModel viewModel = new MenuViewModel
                {
                    Key = menu.Key,
                    Title = menu.Title,
                    Path = menu.Path,
                    Icon = menu.Icon,
                    Children = BuildMenuTree(menus, menu.Id)
                };
                if (viewModel.Children.Count == 0)
                {
                    viewModel.Children = null;
                }
                result.Add(viewModel);
            }

            return result;
        }
    }
}
