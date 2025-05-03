using EOM.TSHotelManagement.Common.Contract;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 导航控件模块接口
    /// </summary>
    public interface INavBarService
    {
        /// <summary>
        /// 导航控件列表
        /// </summary>
        /// <returns></returns>
        ListOutputDto<ReadNavBarOutputDto> NavBarList();
    }
}