namespace EOM.TSHotelManagement.Common.Contract
{
    public class UpdateCustomerInputDto : BaseInputDto
    {
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerGender { get; set; }
        public int PassportId { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdCardNumber { get; set; }
        public string CustomerAddress { get; set; }
        public int CustomerType { get; set; }
    }
}

