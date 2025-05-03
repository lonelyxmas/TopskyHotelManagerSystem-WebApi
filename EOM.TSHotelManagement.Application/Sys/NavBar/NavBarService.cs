using EOM.TSHotelManagement.Common.Contract;
using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Common.Util;
using EOM.TSHotelManagement.EntityFramework;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 导航控件模块
    /// </summary>
    public class NavBarService : INavBarService
    {
        /// <summary>
        /// 导航控件
        /// </summary>
        private readonly GenericRepository<NavBar> navBarRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navBarRepository"></param>
        public NavBarService(GenericRepository<NavBar> navBarRepository)
        {
            this.navBarRepository = navBarRepository;
        }

        /// <summary>
        /// 导航控件列表
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadNavBarOutputDto> NavBarList()
        {
            var navBarList = navBarRepository.GetList(a => a.IsDelete != 1);

            var listSource = EntityMapper.MapList<NavBar, ReadNavBarOutputDto>(navBarList);

            return new ListOutputDto<ReadNavBarOutputDto>
            {
                listSource = listSource,
                total = listSource.Count
            };
        }
    }
}
