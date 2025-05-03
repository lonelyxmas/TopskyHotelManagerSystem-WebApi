using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using EOM.TSHotelManagement.Common.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 用户信息控制器
    /// </summary>
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        private readonly ICustomerService customerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerService"></param>
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="custo"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto InsertCustomerInfo([FromBody] CreateCustomerInputDto custo)
        {
            return customerService.InsertCustomerInfo(custo);
        }

        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="custo"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdCustomerInfo([FromBody] UpdateCustomerInputDto custo)
        {
            return customerService.UpdCustomerInfo(custo);
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="custo"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DelCustomerInfo([FromBody] DeleteCustomerInputDto custo)
        {
            return customerService.DelCustomerInfo(custo);
        }

        /// <summary>
        /// 更新客户类型(即会员等级)
        /// </summary>
        /// <param name="updateCustomerInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto UpdCustomerTypeByCustoNo([FromBody] UpdateCustomerInputDto updateCustomerInputDto)
        {
            return customerService.UpdCustomerTypeByCustoNo(updateCustomerInputDto);
        }

        /// <summary>
        /// 查询酒店盈利情况（用于报表）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CustoSpend> SelectAllMoney()
        {
            return customerService.SelectAllMoney();
        }

        /// <summary>
        /// 查询所有客户信息
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public ListOutputDto<Customer> SelectCustoAll([FromQuery] int pageIndex, int pageSize, bool onlyVip = false)
        //{
        //    return customerService.SelectCustoAll(pageIndex, pageSize, onlyVip);
        //}

        /// <summary>
        /// 查询所有客户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadCustomerOutputDto> SelectCustomers(ReadCustomerInputDto custo)
        {
            return customerService.SelectCustomers(custo);
        }

        /// <summary>
        /// 查询指定客户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SingleOutputDto<ReadCustomerOutputDto> SelectCustoByInfo([FromQuery] ReadCustomerInputDto custo)
        {
            return customerService.SelectCustoByInfo(custo);
        }

        /// <summary>
        /// 根据客户编号查询客户信息
        /// </summary>
        /// <param name="CustoNo"></param>
        /// <returns></returns>
        //[HttpGet]
        //public Customer SelectCardInfoByCustoNo([FromQuery] string CustoNo)
        //{
        //    return customerService.SelectCardInfoByCustoNo(CustoNo);
        //}

    }
}
