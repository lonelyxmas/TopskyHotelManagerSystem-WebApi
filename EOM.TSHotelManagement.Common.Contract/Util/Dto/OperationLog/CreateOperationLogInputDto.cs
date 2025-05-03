using EOM.TSHotelManagement.Common.Core;

namespace EOM.TSHotelManagement.Common.Contract
{
    public class CreateOperationLogInputDto : BaseInputDto
    {
        public DateTime OperationTime { get; set; }
        public string LogContent { get; set; }
        public string OperationAccount { get; set; }
        public LogLevel LogLevel { get; set; }
        public string SoftwareVersion { get; set; }
        public string LoginIpAddress { get; set; }
    }
}

