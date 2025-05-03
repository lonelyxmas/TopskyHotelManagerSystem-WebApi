namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadCustomerInputDto : ListInputDto
    {
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string IdCardNumber { get; set; }
    }
}

