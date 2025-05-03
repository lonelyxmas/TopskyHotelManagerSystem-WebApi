namespace EOM.TSHotelManagement.Common.Contract
{
    public class ReadCustoTypeOutputDto
    {
        public int Id { get; set; }
        public int CustomerType { get; set; }
        public string CustomerTypeName { get; set; }
        public decimal Discount { get; set; }
    }
}

