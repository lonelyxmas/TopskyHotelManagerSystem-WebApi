using EOM.TSHotelManagement.Application;
using EOM.TSHotelManagement.Common.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOM.TSHotelManagement.WebApi.Controllers
{
    /// <summary>
    /// 工具类控制器
    /// </summary>
    public class UtilityController : ControllerBase
    {
        private readonly IUtilService utilService;

        public UtilityController(IUtilService utilService)
        {
            this.utilService = utilService;
        }

        /// <summary>
        /// 查询身份证号码
        /// </summary>
        /// <param name="readCardCodeInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public SingleOutputDto<ReadCardCodeOutputDto> SelectCardCode([FromBody] ReadCardCodeInputDto readCardCodeInputDto)
        {
            return utilService.SelectCardCode(readCardCodeInputDto);
        }

        /// <summary>
        /// 检测版本号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ReadApplicationVersionOutputDto CheckBaseVersion()
        {
            return utilService.CheckBaseVersion();
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto AddLog([FromBody] CreateOperationLogInputDto opr)
        {
            return utilService.AddLog(opr);
        }

        /// <summary>
        /// 查询所有操作日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListOutputDto<ReadOperationLogOutputDto> SelectOperationlogAll([FromQuery] ReadOperationLogInputDto readOperationLogInputDto)
        {
            return utilService.SelectOperationlogAll(readOperationLogInputDto);
        }

        /// <summary>
        /// 删除时间范围的操作日志
        /// </summary>
        /// <param name="readOperationLogInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteOperationlogByRange([FromBody] ReadOperationLogInputDto readOperationLogInputDto)
        {
            return utilService.DeleteOperationlogByRange(readOperationLogInputDto);
        }

        /// <summary>
        /// 删除操作日志
        /// </summary>
        /// <param name="deleteOperationLogInputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseOutputDto DeleteOperationlog([FromBody] DeleteOperationLogInputDto deleteOperationLogInputDto)
        {
            return utilService.DeleteOperationlog(deleteOperationLogInputDto);
        }
    }
}
