
namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadOperationLogInputDto : ListInputDto
    {
        public string OperationId { get; set; }
        public int? LogLevel { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

