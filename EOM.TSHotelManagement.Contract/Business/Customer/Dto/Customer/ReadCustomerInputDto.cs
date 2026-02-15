using EOM.TSHotelManagement.Common;

namespace EOM.TSHotelManagement.Contract
{
    public class ReadCustomerInputDto : ListInputDto
    {
        public string? CustomerName { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? IdCardNumber { get; set; }
        public int? CustomerGender { get; set; }
        public int? CustomerType { get; set; }
        public DateRangeDto DateRangeDto { get; set; }
    }
}

