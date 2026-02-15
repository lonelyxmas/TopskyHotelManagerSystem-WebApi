using EOM.TSHotelManagement.Contract;

namespace EOM.TSHotelManagement.Service
{
    public interface ICustomerAccountService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="readCustomerAccountInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadCustomerAccountOutputDto> Login(ReadCustomerAccountInputDto readCustomerAccountInputDto);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="readCustomerAccountInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadCustomerAccountOutputDto> Register(ReadCustomerAccountInputDto readCustomerAccountInputDto);
    }
}
