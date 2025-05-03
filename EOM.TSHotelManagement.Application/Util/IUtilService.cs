using EOM.TSHotelManagement.Common.Contract;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 工具类接口
    /// </summary>
    public interface IUtilService
    {
        /// <summary>
        /// 查询身份证号码
        /// </summary>
        /// <param name="readCardCodeInputDto"></param>
        /// <returns></returns>
        SingleOutputDto<ReadCardCodeOutputDto> SelectCardCode(ReadCardCodeInputDto readCardCodeInputDto);

        /// <summary>
        /// 检测版本号
        /// </summary>
        /// <returns></returns>
        ReadApplicationVersionOutputDto CheckBaseVersion();

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        BaseOutputDto AddLog(CreateOperationLogInputDto opr);

        /// <summary>
        /// 查询所有操作日志
        /// </summary>
        /// <returns></returns>
        ListOutputDto<ReadOperationLogOutputDto> SelectOperationlogAll(ReadOperationLogInputDto readOperationLogInputDto);

        /// <summary>
        /// 删除指定时间范围的操作日志
        /// </summary>
        /// <returns></returns>
        BaseOutputDto DeleteOperationlogByRange(ReadOperationLogInputDto readOperationLogInputDto);

        /// <summary>
        /// 删除操作日志
        /// </summary>
        /// <returns></returns>
        BaseOutputDto DeleteOperationlog(DeleteOperationLogInputDto deleteOperationLogInputDto);
    }
}
