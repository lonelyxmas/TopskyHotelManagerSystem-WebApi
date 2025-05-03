using EOM.TSHotelManagement.Common.Contract;
using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Common.Util;
using EOM.TSHotelManagement.EntityFramework;
using jvncorelib.EntityLib;
using SqlSugar;

namespace EOM.TSHotelManagement.Application
{
    /// <summary>
    /// 工具接口实现类
    /// </summary>
    public class UtilService : IUtilService
    {
        /// <summary>
        /// 卡片代码
        /// </summary>
        private readonly GenericRepository<CardCode> cardCodesRepository;

        /// <summary>
        /// 应用检测
        /// </summary>
        private readonly GenericRepository<ApplicationVersion> applicationRepository;

        /// <summary>
        /// 操作日志
        /// </summary>
        private readonly GenericRepository<OperationLog> operationLogRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardCodesRepository"></param>
        /// <param name="applicationRepository"></param>
        /// <param name="operationLogRepository"></param>
        public UtilService(GenericRepository<CardCode> cardCodesRepository, GenericRepository<ApplicationVersion> applicationRepository, GenericRepository<OperationLog> operationLogRepository)
        {
            this.cardCodesRepository = cardCodesRepository;
            this.applicationRepository = applicationRepository;
            this.operationLogRepository = operationLogRepository;
        }

        /// <summary>
        /// 查询身份证号码
        /// </summary>
        /// <param name="readCardCodeInputDto"></param>
        /// <returns></returns>
        public SingleOutputDto<ReadCardCodeOutputDto> SelectCardCode(ReadCardCodeInputDto readCardCodeInputDto)
        {
            if (!readCardCodeInputDto.IdentityCardNumber.IsNullOrEmpty())
            {
                var cardid = readCardCodeInputDto.IdentityCardNumber.Substring(0, 6).ToString();
                var cardcodes = cardCodesRepository.GetSingle(a => a.AreaCode == cardid);
                var source = EntityMapper.Map<CardCode, ReadCardCodeOutputDto>(cardcodes);
                return new SingleOutputDto<ReadCardCodeOutputDto>() { Source = source };
            }
            return new SingleOutputDto<ReadCardCodeOutputDto>();
        }

        /// <summary>
        /// 检测版本号
        /// </summary>
        /// <returns></returns>
        public ReadApplicationVersionOutputDto CheckBaseVersion()
        {
            var source = EntityMapper.Map<ApplicationVersion, ReadApplicationVersionOutputDto>(applicationRepository.GetSingle(a => a.Id == 1));

            return source;
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        public BaseOutputDto AddLog(CreateOperationLogInputDto opr)
        {
            try
            {
                operationLogRepository.Insert(EntityMapper.Map<CreateOperationLogInputDto, OperationLog>(opr));
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto();
        }

        /// <summary>
        /// 查询所有操作日志
        /// </summary>
        /// <returns></returns>
        public ListOutputDto<ReadOperationLogOutputDto> SelectOperationlogAll(ReadOperationLogInputDto readOperationLogInputDto)
        {
            List<OperationLog> operationLogs = new List<OperationLog>();

            var where = Expressionable.Create<OperationLog>();

            if (!readOperationLogInputDto.IsDelete.IsNullOrEmpty())
            {
                where = where.And(a => a.IsDelete == readOperationLogInputDto.IsDelete);
            }
            if (!readOperationLogInputDto.LogLevel.IsNullOrEmpty())
            {
                if (readOperationLogInputDto.LogLevel.HasValue)
                {
                    where = where.And(a => a.LogLevel == (LogLevel)readOperationLogInputDto.LogLevel.Value);
                }
            }

            var count = 0;
            if (readOperationLogInputDto.Page != 0 && readOperationLogInputDto.PageSize != 0)
            {
                operationLogs = operationLogRepository.AsQueryable().Where(where.ToExpression()).OrderByDescending(a => a.OperationTime).ToPageList(readOperationLogInputDto.Page, readOperationLogInputDto.PageSize, ref count);
            }
            else
            {
                operationLogs = operationLogRepository.AsQueryable().Where(where.ToExpression()).OrderByDescending(a => a.OperationTime).ToList();
            }

            operationLogs.ForEach(source =>
            {
                source.LogLevelName = source.LogLevel == LogLevel.Normal ? LocalizationHelper.GetLocalizedString("INFO", "常规操作") : source.LogLevel == LogLevel.Warning ? LocalizationHelper.GetLocalizedString("WARNING", "敏感操作") : LocalizationHelper.GetLocalizedString("ERROR", "严重操作");
            });

            var listSource = EntityMapper.MapList<OperationLog, ReadOperationLogOutputDto>(operationLogs);

            return new ListOutputDto<ReadOperationLogOutputDto> { listSource = listSource, total = count };
        }

        /// <summary>
        /// 删除指定时间范围的操作日志
        /// </summary>
        /// <returns></returns>
        public BaseOutputDto DeleteOperationlogByRange(ReadOperationLogInputDto readOperationLogInputDto)
        {
            try
            {
                var where = Expressionable.Create<OperationLog>();

                if (readOperationLogInputDto.StartTime != DateTime.MinValue && readOperationLogInputDto.EndTime != DateTime.MinValue)
                {
                    where = where.And(a => a.OperationTime >= readOperationLogInputDto.StartTime && a.OperationTime <= readOperationLogInputDto.EndTime);
                }

                var logsToDelete = operationLogRepository.AsQueryable().Where(where.ToExpression()).ToList();

                if (logsToDelete.Any())
                {
                    operationLogRepository.Delete(logsToDelete);
                }
            }
            catch (Exception ex)
            {
                return new BaseOutputDto { Message = LocalizationHelper.GetLocalizedString(ex.Message, ex.Message), StatusCode = StatusCodeConstants.InternalServerError };
            }
            return new BaseOutputDto { Message = "操作日志删除成功", StatusCode = StatusCodeConstants.Success };
        }

        /// <summary>
        /// 删除操作日志
        /// </summary>
        /// <returns></returns>
        public BaseOutputDto DeleteOperationlog(DeleteOperationLogInputDto deleteOperationLogInputDto)
        {
            var result = operationLogRepository.Delete(a => a.OperationId == deleteOperationLogInputDto.OperationId);

            if (result)
            {
                return new BaseOutputDto(StatusCodeConstants.Success, LocalizationHelper.GetLocalizedString("Delete Operation Log Success", "操作日志删除成功"));
            }
            else
            {
                return new BaseOutputDto(StatusCodeConstants.InternalServerError, LocalizationHelper.GetLocalizedString("Delete Operation Log Failed", "操作日志删除失败"));
            }
        }

    }
}
