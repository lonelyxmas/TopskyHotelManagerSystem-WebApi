using EOM.TSHotelManagement.Contract;
using EOM.TSHotelManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    public class CustomerAccountController : ControllerBase
    {
        private readonly ICustomerAccountService _customerAccountService;

        public CustomerAccountController(ICustomerAccountService customerAccountService)
        {
            _customerAccountService = customerAccountService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="readCustomerAccountInputDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public SingleOutputDto<ReadCustomerAccountOutputDto> Login([FromBody] ReadCustomerAccountInputDto readCustomerAccountInputDto)
        {
            return _customerAccountService.Login(readCustomerAccountInputDto);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="readCustomerAccountInputDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public SingleOutputDto<ReadCustomerAccountOutputDto> Register([FromBody] ReadCustomerAccountInputDto readCustomerAccountInputDto)
        {
            return _customerAccountService.Register(readCustomerAccountInputDto);
        }
    }
}
